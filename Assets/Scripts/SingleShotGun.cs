using Photon.Pun;
using UnityEngine;

public class SingleShotGun : Gun
{
    [SerializeField] Camera _camera;

    PhotonView PV;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
    public override void Use()
    {
        Shoot();
    }

    void Shoot()
    {
        Ray ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        ray.origin = _camera.transform.position;

        if(Physics.Raycast(ray, out RaycastHit hit))
        {
            var gunInfo = (GunInfo) itemInfo;
            hit.collider.gameObject.GetComponent<IDamageable>()?.TakeDamage(gunInfo.damage);

            PV.RPC("RPC_Shoot", RpcTarget.All, hit.point, hit.normal);
        }
    }

    [PunRPC]
    void RPC_Shoot(Vector3 hitPosition, Vector3 hitNormal)
    {
        Collider[] colliders = Physics.OverlapSphere(hitPosition, 0.3f);
        if(colliders.Length != 0 )
        {
            GameObject bulletImpactObj = Instantiate(bulletImpactPrefab, hitPosition + hitNormal * 0.01f, Quaternion.LookRotation(hitNormal, Vector3.up) * bulletImpactPrefab.transform.rotation);
            Destroy(bulletImpactObj, 2f);
            bulletImpactObj.transform.SetParent(colliders[0].transform);
        }
    }
}
