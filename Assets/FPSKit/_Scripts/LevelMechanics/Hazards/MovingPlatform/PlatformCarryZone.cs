using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformCarryZone : MonoBehaviour
{
    [SerializeField]
    private Transform _parentObject;

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Carry");

        collision.transform.SetParent(_parentObject);
    }

    private void OnTriggerExit(Collider collision)
    {
        Debug.Log("Drop");

        collision.transform.SetParent(null);
    }
}
