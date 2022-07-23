using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformCarryZone : MonoBehaviour
{
    [SerializeField]
    private Transform _parentObject;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        /*
        MovementKM movement = collision.gameObject.GetComponent<MovementKM>();
        if (movement != null)
        {
            _passengers.Add(movement);
        }
        */
        collision.transform.SetParent(_parentObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        /*
        MovementKM movement = collision.gameObject.GetComponent<MovementKM>();
        if (movement != null)
        {
            _passengers.Remove(movement);
        }
        */
        collision.transform.SetParent(null);
    }
}
