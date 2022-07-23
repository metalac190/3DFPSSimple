 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Firing")]
    [SerializeField] private int _damage = 3;
    [SerializeField][Tooltip("Fires this many times per second")]
    private float _fireRate = .15f;
    public int Damage => _damage;
    public float FireRate => _fireRate;

    [Header("Projectile")]
    [SerializeField] private Projectile _projectilePrefab;
    [SerializeField] private Transform _projectileSpawnLocation;
    protected Projectile ProjectilePrefab => _projectilePrefab;
    protected Transform ProjectileSpawnLocation => _projectileSpawnLocation;

    [Header("FX")]
    [SerializeField] private ParticleSystem _shootParticlePrefab;
    [SerializeField] private AudioClip _shootSound;
    protected ParticleSystem ShootParticlePrefab => _shootParticlePrefab;
    protected AudioClip ShootSound => _shootSound;

    public bool IsOnCooldown { get; private set; } = false;

    public virtual void Shoot()
    {
        IsOnCooldown = true;
        Invoke(nameof(TakeOffCooldown), _fireRate);

        if(_projectilePrefab != null)
        {
            Projectile newProjectile = Instantiate
                (ProjectilePrefab,
                ProjectileSpawnLocation.position,
                ProjectileSpawnLocation.rotation);
            newProjectile.Damage = _damage;
        }

        ActivateShootFX();
    }

    public void TakeOffCooldown()
    {
        IsOnCooldown = false;
    }

    private void ActivateShootFX()
    {
        if(_shootParticlePrefab != null)
        {
            ParticleSystem burstParticle = Instantiate
                (ShootParticlePrefab,
                ProjectileSpawnLocation.position,
                Quaternion.identity);
            burstParticle.transform.SetParent(ProjectileSpawnLocation);
        }
        if(_shootSound != null)
        {
            AudioSource.PlayClipAtPoint(ShootSound, 
                ProjectileSpawnLocation.position);
        }
    }
}
