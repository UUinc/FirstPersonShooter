using UnityEngine;

public class SingleShotGun : Gun
{
    [SerializeField] Camera _camera;
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
        }
    }
}
