using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blaster : Weapon
{
    [Header("Blaster")]
    [SerializeField] private float _shootDistance = 100;
    [SerializeField] private LayerMask _shootableLayers = -1;
    [SerializeField] private ParticleSystem _impactParticlePrefab = null;

    private Transform _rayOrigin;

    private void Awake()
    {
        // we don't REALLY want to fire from the weapon muzzle with raycasts,
        // we want to fire from center of camera
        _rayOrigin = Camera.main.transform;
    }

    public override void Shoot()
    {
        // do the default shoot
        base.Shoot();
        // additionally do a hitscan instead of a projectile
        // for debugging
        Debug.DrawRay(_rayOrigin.position, 
            transform.forward * _shootDistance);
        // if we hit something shootable
        RaycastHit hitInfo;
        if (Physics.Raycast(_rayOrigin.position, transform.forward, 
            out hitInfo, _shootDistance, _shootableLayers))
        {
            ApplyHit(hitInfo);
        }
    }

    private void ApplyHit(RaycastHit hitInfo)
    {
        // test hitInfo if we should apply damage
        Health health = hitInfo.collider.gameObject.GetComponent<Health>();
        if(health != null)
        {
            health.Damage(Damage);
        }
        // otherwise just apply impact effects
        if(_impactParticlePrefab != null)
        {
            ParticleSystem newParticle = Instantiate(_impactParticlePrefab, 
                hitInfo.point, Quaternion.identity);
            newParticle.transform.forward = hitInfo.normal * -1f;
        }
    }
}
