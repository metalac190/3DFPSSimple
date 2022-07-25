using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Effects play on objects that take damage
/// </summary>

public class HealthFX : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Health _health = null;
    [SerializeField] private MeshRenderer _hitMesh = null;

    [Header("General Settings")]
    [SerializeField][Tooltip("Use this for making particles bigger for large destructible objects")]
    private float _particleScale = 1;

    [Header("Hit FX")]
    [SerializeField] private Color _hitColor = Color.red;
    [SerializeField] private AudioClip _hitSound;
    [SerializeField] private ParticleSystem _hitParticles;

    [Header("Death FX")]
    [SerializeField] private AudioClip _destroySound = null;
    [SerializeField] private ParticleSystem _destroyParticles = null;

    private HitFlash3D _hitFlash = null;

    private void Awake()
    {
        if(_hitMesh != null)
            _hitFlash = new HitFlash3D(this, _hitMesh, _hitColor);
    }

    private void OnEnable()
    {
        if(_health != null)
        {
            _health.Damaged.AddListener(PlayDamagedFX);
            _health.Died.AddListener(PlayDiedFX);
        }
    }

    private void OnDisable()
    {
        if (_health != null)
        {
            _health.Damaged.AddListener(PlayDamagedFX);
            _health.Died.RemoveListener(PlayDiedFX);
        }
    }

    private void PlayDamagedFX(int damage)
    {
        if (_hitMesh != null && _hitFlash != null)
            _hitFlash.Flash();
        if (_hitParticles != null)
        {
            ParticleSystem newParticles = Instantiate
                (_hitParticles, transform.position, Quaternion.identity);
            newParticles.transform.localScale = new Vector3
                (_particleScale, _particleScale, _particleScale);
        }
        if (_hitSound != null)
            PlayClip3D(_hitSound, transform.position, 1);
    }

    private void PlayDiedFX(GameObject deathObject)
    {
        if (_destroyParticles != null)
        {
            ParticleSystem newParticles = Instantiate
                (_destroyParticles, transform.position, Quaternion.identity);
            newParticles.transform.localScale = new Vector3
                (_particleScale, _particleScale, _particleScale);
        }
        if (_destroySound != null)
            PlayClip3D(_destroySound, deathObject.transform.position, 1);
    }

    private AudioSource PlayClip3D(AudioClip clip, Vector3 position,
        float volume = 1, float pitchRange = .03f)
    {
        pitchRange = Mathf.Clamp(pitchRange, 0, 1);
        // create
        GameObject audioObject = new GameObject("3DAudio");
        AudioSource audioSource = audioObject.AddComponent<AudioSource>();
        //configure
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.pitch = 1 + Random.Range(-pitchRange, pitchRange);
        audioSource.spatialBlend = 1;
        audioObject.transform.position = position;
        // activate
        audioSource.Play();
        Object.Destroy(audioObject, clip.length);
        // return in case the caller wants to do other things
        return audioSource;
    }
}
