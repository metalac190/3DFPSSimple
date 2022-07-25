using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private LayerMask _layersDetected = -1;
    [SerializeField] private float _speed = 8;
    [SerializeField][Tooltip("Allow us to destroy projectiles if they have existed too long, for optimization")]
    private float _destroyAfterSeconds = 4;

    public float Speed => _speed;

    [Header("FX")]
    [SerializeField] private ParticleSystem _explodeParticles;
    [SerializeField] private AudioClip _explodeSound;

    [Header("Dependencies")]
    [SerializeField] private Rigidbody _rb;
    public Rigidbody RB => _rb;

    public int Damage { get; set; }

    private void Awake()
    {
        // destroy this object if it has existed too long
        Invoke(nameof(DestroyFX), _destroyAfterSeconds);
    }

    private void FixedUpdate()
    {
        Move(_rb);
    }

    protected virtual void Move(Rigidbody rb)
    {
        _rb.velocity = transform.forward * _speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        // if we're not in the layer, return
        if (!PhysicsHelper.IsInLayerMask(other.gameObject, _layersDetected)) { return; }

        Invoke(nameof(DestroyFX),.05f);
    }

    void DestroyFX()
    {
        if (_explodeParticles != null)
            Instantiate(_explodeParticles, transform.position, Quaternion.identity);
        if (_explodeSound != null)
            AudioHelper.PlayClip3D(_explodeSound, transform.position, 1);
        Destroy(gameObject);
    }
}
