using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillVolume : TriggerVolume
{
    protected override void TriggerEntered(GameObject newObject)
    {
        Health health = newObject.GetComponent<Health>();
        if (health != null)
        {
            health.Kill();
        }
    }
}
