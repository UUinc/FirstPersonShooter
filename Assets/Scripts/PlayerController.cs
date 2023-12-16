using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerController : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject cameraHolder;

    [SerializeField] float mouseSensitivity;
    [SerializeField] float sprintSpeed;
    [SerializeField] float walkSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float smoothTime;


    [SerializeField] Item[] items;
    int itemIndex;
    int previousItemIndex = -1;

    float verticalLookRotation;
    bool grounded;
    Vector3 smoothMoveVelocity;
    Vector3 moveAmount;

    Rigidbody rb;

    PhotonView PV;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        PV = GetComponent<PhotonView>();
    }

    void Start()
    {
        if(PV.IsMine)
        {
            EquipeItem(0);
        }
        else
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(rb);
        }
    }

    private void Update()
    {
        if (!PV.IsMine) return;

        Look();
        Move();
        Jump();

        for(int i = 0; i < items.Length; i++)
        {
            if(Input.GetKeyDown((i+1).ToString()))
            {
                EquipeItem(i); 
                break;
            }
        }

        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0f)
        {
            if(itemIndex >= items.Length - 1)
            {
                EquipeItem(0);
            }
            else
            {
                EquipeItem(itemIndex + 1);
            }
        }
        else if(Input.GetAxisRaw("Mouse ScrollWheel") < 0f)
        {
            if (itemIndex <= 0)
            {
                EquipeItem(items.Length - 1);
            }
            else
            {
                EquipeItem(itemIndex - 1);
            }
        }
    }

    void Look()
    {
        transform.Rotate(Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.deltaTime * 500 * Vector3.up);

        verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.deltaTime * 500;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

        cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
    }

    void Move()
    {
        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed), ref smoothMoveVelocity, smoothTime);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            rb.AddForce(transform.up * jumpForce);
        }
    }

    void EquipeItem(int _index)
    {
        if (_index == previousItemIndex) return;

        itemIndex = _index;
        items[itemIndex].itemGameObject.SetActive(true);

        if(previousItemIndex != -1)
        {
            items[previousItemIndex].itemGameObject.SetActive(false);
        }

        previousItemIndex = itemIndex;

        if(PV.IsMine)
        {
            Hashtable hash = new Hashtable();
            hash.Add("itemIndex", itemIndex);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if(!PV.IsMine && targetPlayer == PV.Owner)
        {
            EquipeItem((int)changedProps["itemIndex"]);
        }
    }

    public void SetGroundedState(bool _grounded)
    {
        grounded = _grounded;
    }

    private void FixedUpdate()
    {
        if (!PV.IsMine) return;
        rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
    }
}
