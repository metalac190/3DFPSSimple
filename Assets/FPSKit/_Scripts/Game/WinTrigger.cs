using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class WinTrigger : TriggerVolume
{
    [Header("Win Settings")]
    [SerializeField]
    private AudioClip _winSound;
    [SerializeField]
    private ParticleSystem _winParticlePrefab;

    public UnityEvent Won;

    protected override void TriggerEntered(GameObject enteredObject)
    {
        Debug.Log("WIN");
        if (_winSound != null)
            AudioHelper.PlayClip2D(_winSound, 1);
        if (_winParticlePrefab != null)
            Instantiate(_winParticlePrefab, transform.position, Quaternion.identity);

        Won.Invoke();
    }
}
