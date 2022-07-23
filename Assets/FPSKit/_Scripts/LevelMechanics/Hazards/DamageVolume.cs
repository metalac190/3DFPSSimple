using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageVolume : TriggerVolume
{
    [Header("Damage Settings")]
    [SerializeField]
    private int _damage;

    protected override void TriggerEntered(GameObject objectEntered)
    {
        Health health = objectEntered.GetComponent<Health>();
        Debug.Log("Take Damage");
        if (health != null)
        {
            health.Damage(_damage);
            /*
            if (_damageParticlePrefab != null)
                Instantiate(_damageParticlePrefab, transform.position, transform.rotation);
            if (_damageSound != null)
                AudioHelper.PlayClip2D(_damageSound, 1);
            */
        }
    }

    protected override void TriggerExited(GameObject objectEntered)
    {
        base.TriggerExited(objectEntered);
    }
}
