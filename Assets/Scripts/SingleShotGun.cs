using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleShotGun : Gun
{

    [SerializeField] Camera cam;

    public override void Use()
    {
        Shoot();
    }

    void Shoot()
    {
        Ray ray = cam.ScreenPointToRay(new Vector3(0.5f, 0.5f));
        ray.origin = cam.transform.position;
        if(Physics.Raycast(ray, out RaycastHit hit))
        {
            hit.collider.gameObject.GetComponent<IDamageable>()?.TakeDamage(((GunInfo)itemInfo).damage); //Si el objeto tiene el script IDamageable, le hace daño al objeto que ha sido golpeado.
        }
    }
}
