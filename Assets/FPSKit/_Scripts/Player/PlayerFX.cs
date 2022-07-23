using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFX : MonoBehaviour
{
    [Header("Player FX")]
    [SerializeField] private AudioClip _damagedSound;
    [SerializeField] private ParticleSystem _damagedParticlePrefab;
    [SerializeField] private AudioClip _deathSound;
    [SerializeField] private ParticleSystem _deathParticlePrefab;

    [Header("Hit Flash")]
    [SerializeField] private Color _flashColor = Color.red;

    [Header("Dependencies")]
    [SerializeField] private Health _health;
    [SerializeField] private MeshRenderer _renderer;

    private HitFlash3D _hitFlash;

    private void Awake()
    {
        if (_renderer != null)
            _hitFlash = new HitFlash3D(this, _renderer, _flashColor, _health.HitInvulDuration);
    }

    private void OnEnable()
    {
        _health.Died.AddListener(OnDied);
        _health.Damaged.AddListener(OnDamaged);
    }

    private void OnDisable()
    {
        _health.Died.RemoveListener(OnDied);
        _health.Damaged.RemoveListener(OnDamaged);
    }

    public void OnDied(GameObject deathObject)
    {
        if(_deathParticlePrefab != null)
            Instantiate(_deathParticlePrefab, transform.position, Quaternion.identity);
        if (_deathSound != null)
            AudioHelper.PlayClip2D(_deathSound, 1);
        //CameraShake.Instance.ShakeCamera(.4f);
    }

    private void OnDamaged(int damaged)
    {
        if (_damagedSound != null)
            AudioHelper.PlayClip2D(_damagedSound, 1);
        if (_damagedParticlePrefab != null)
            Instantiate(_damagedParticlePrefab, transform.position, Quaternion.identity);

        _hitFlash?.Flash();
        //CameraShake.Instance.ShakeCamera(.4f);
    }

}
