using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using TMPro;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerController : MonoBehaviourPunCallbacks, IDamageable
{
    [SerializeField] GameObject cameraHolder;
    [SerializeField] Transform body;
    [SerializeField] Animator animator;

    [SerializeField] GameObject ui;
    [SerializeField] TextMeshProUGUI healthUI;

    [SerializeField] float mouseSensitivity;
    [SerializeField] float sprintSpeed;
    [SerializeField] float walkSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float smoothTime;

    [SerializeField] float clampValue = 14.5f;

    [SerializeField] Item[] items;
    [SerializeField] GameObject[] guns;
    int itemIndex;
    int previousItemIndex = -1;

    float verticalLookRotation;
    bool grounded;
    Vector3 smoothMoveVelocity;
    Vector3 moveAmount;

    Rigidbody rb;

    PhotonView PV;

    const float maxHealth = 100f;
    float currentHealth = maxHealth;

    PlayerManager playerManager;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        PV = GetComponent<PhotonView>();

        playerManager = PhotonView.Find((int)PV.InstantiationData[0]).GetComponent<PlayerManager>();
    }

    void Start()
    {
        SetSettings();

        if (PV.IsMine)
        {
            EquipeItem(0);

            //Hide body from the player
            body.GetChild(0).gameObject.SetActive(false);
            body.GetChild(1).gameObject.SetActive(false);
            body.GetChild(2).gameObject.SetActive(false);
            guns[0].transform.parent.parent.gameObject.SetActive(false);
        }
        else
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(rb);
            Destroy(ui);

            //Hide player gun from other players
            for(int i=0; i<items.Length; i++)
            {
                items[i].itemGameObject.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (!PV.IsMine) return;

        Look();
        Move();
        Jump();
        Animation();

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

        if(Input.GetMouseButtonDown(0))
        {
            items[itemIndex].Use();
        }

        //Die when fall from the world
        if(transform.position.y < -10f)
        {
            Die();
        }
    }

    void SetSettings()
    {
        //Mouse Sensitivity
        mouseSensitivity = PlayerPrefs.GetFloat(SettingsManager.SENSITIVITY_KEY, 1f);
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

        // Clamp in the map
        Vector3 newPosition = transform.position;
        newPosition.x = Mathf.Clamp(newPosition.x, -clampValue, clampValue);
        newPosition.z = Mathf.Clamp(newPosition.z, -clampValue, clampValue);

        transform.position = newPosition;
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            AudioManager.Instance.Play("Jump");
            rb.AddForce(transform.up * jumpForce);
        }
    }

    void Animation()
    {
        //Animation
        animator.SetBool("isWalking", Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0);
        animator.SetBool("isRunning", Input.GetKey(KeyCode.LeftShift));

        if (Input.GetKeyDown(KeyCode.Space) && grounded)
            animator.SetTrigger("jump");

        //Dance
        if (Input.GetKeyDown(KeyCode.O))
            animator.SetTrigger("hiphop");

        if (Input.GetKeyDown(KeyCode.P))
            animator.SetTrigger("hiphopGirly");
    }

    void EquipeItem(int _index)
    {
        if (_index == previousItemIndex) return;

        itemIndex = _index;
        items[itemIndex].itemGameObject.SetActive(true);
        //Second object item
        guns[itemIndex].SetActive(true);

        if (previousItemIndex != -1)
        {
            items[previousItemIndex].itemGameObject.SetActive(false);
            //Second object item
            guns[previousItemIndex].SetActive(false);
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
        if (!changedProps.ContainsKey("itemIndex")) return;

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

    public void TakeDamage(float damage)
    {
        PV.RPC(nameof(RPC_TakeDamage), PV.Owner, damage);
    }

    [PunRPC]
    void RPC_TakeDamage(float damage, PhotonMessageInfo info)
    {
        AudioManager.Instance.Play("Damages");
        currentHealth -= damage;

        healthUI.text = Mathf.Ceil(currentHealth).ToString();

        if (currentHealth <= 0)
        {
            Die();
            PlayerManager.Find(info.Sender).GetKill();
        }
    }

    void Die() 
    {
        playerManager.Die();
    }
}
