using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class Pickup : MonoBehaviour
{
    // this is our template method. Subclasses must implement
    protected abstract void OnPickup(PlayerCharacter playerCharacter);

    [Header("FX")]
    [SerializeField] ParticleSystem _pickupParticlePrefab = null;
    [SerializeField] AudioClip _pickupSound = null;

    // Reset gets called whenever you add a component to an object
    private void Reset()
    {
        // set isTrigger in the Inspector, just in case Designer forgot
        Collider collider = GetComponent<Collider>();
        collider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        // guard clause
        PlayerCharacter playerCharacter = other.attachedRigidbody.GetComponent<PlayerCharacter>();
        if (playerCharacter == null) { return; }

        // found the player! call our abstract method and supporting feedback
        OnPickup(playerCharacter);

        if (_pickupSound != null)
        {
            AudioHelper.PlayClip2D(_pickupSound, 1);
        }

        if (_pickupParticlePrefab != null)
        {
            SpawnParticle(_pickupParticlePrefab);
        }

        Disable();
    }

    void SpawnParticle(ParticleSystem pickupParticles)
    {
        ParticleSystem newParticles =
            Instantiate(pickupParticles, transform.position, Quaternion.identity);

        newParticles.Play();
    }

    // allow override in case subclass wants to object pool, etc.
    protected virtual void Disable()
    {
        gameObject.SetActive(false);
    }
}
