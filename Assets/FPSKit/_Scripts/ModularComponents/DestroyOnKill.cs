using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class DestroyOnKill : MonoBehaviour
{
    [Header("Optional")]
    [SerializeField]
    private ParticleSystem _killParticlesPrefab;

    [SerializeField]
    private GameObject _spawnPrefabOnDeath;

    private Health _health;

    private void Awake()
    {
        _health = GetComponent<Health>();
    }

    private void OnEnable()
    {
        _health.Died.AddListener(OnDied);
    }

    private void OnDisable()
    {
        _health.Died.RemoveListener(OnDied);
    }

    private void OnDied(GameObject deathObject)
    {
        PlayVFX();
        SpawnObject();

        Destroy(gameObject);
    }

    private void PlayVFX()
    {
        if (_killParticlesPrefab != null)
        {
            ParticleSystem killParticles = Instantiate(_killParticlesPrefab,
                transform.position, Quaternion.identity);
            killParticles.Play();
        }
        /*
        if (_killSFX != null)
        {
            _killSFX.PlayOneShot(transform.position);
        }
        */
    }

    private void SpawnObject()
    {
        if(_spawnPrefabOnDeath != null)
        {
            Debug.Log("Spawn Object");
            Instantiate(_spawnPrefabOnDeath, transform.position, Quaternion.identity);
        }
    }
}
