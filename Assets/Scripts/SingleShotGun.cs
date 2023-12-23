using Photon.Pun;
using UnityEngine;

public class SingleShotGun : Gun
{
    [SerializeField] Camera _camera;

    PhotonView PV;
    GameObject ImpactPrefab;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
    public override void Use()
    {
        Shoot();
    }

    void Animation()
    {
        Animator gunAnimator = transform.GetComponentInChildren<Animator>();
        gunAnimator.SetTrigger("shoot");
    }

    void Shoot()
    {
        Animation();

        Ray ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        ray.origin = _camera.transform.position;

        if(Physics.Raycast(ray, out RaycastHit hit))
        {
            var gunInfo = (GunInfo) itemInfo;
            hit.collider.gameObject.GetComponent<IDamageable>()?.TakeDamage(gunInfo.damage);

            //Impact prefab
            ImpactPrefab = hit.collider.gameObject.CompareTag("Player") ? bulletImpactPrefab : bulletImpactWallPrefab;

            PV.RPC("RPC_Shoot", RpcTarget.All, hit.point, hit.normal);
        }
    }

    [PunRPC]
    void RPC_Shoot(Vector3 hitPosition, Vector3 hitNormal)
    {
        Collider[] colliders = Physics.OverlapSphere(hitPosition, 0.3f);
        if (colliders.Length != 0)
        {
            GameObject bulletImpactObj = Instantiate(ImpactPrefab, hitPosition + hitNormal * 0.01f, Quaternion.LookRotation(hitNormal, Vector3.up) * bulletImpactPrefab.transform.rotation);
            Destroy(bulletImpactObj, 2f);
            bulletImpactObj.transform.SetParent(colliders[0].transform);
        }
    }
}
