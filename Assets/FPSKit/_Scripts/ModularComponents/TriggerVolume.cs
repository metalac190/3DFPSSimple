using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class TriggerVolume : MonoBehaviour
{
    [SerializeField][Tooltip("Objects in these layers will activate" +
        "this trigger")]
    private LayerMask _layersDetected = -1;

    protected Collider Collider = null;

    protected virtual void Awake()
    {
        // ensure it's marked as trigger
        Collider = GetComponent<Collider>();
        Collider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider otherCollider)
    {
        // if we're not in the layer, return
        if (!PhysicsHelper.IsInLayerMask(otherCollider.gameObject, _layersDetected)) { return; }

        TriggerEntered(otherCollider.gameObject);
    }

    private void OnTriggerExit(Collider otherCollider)
    {
        // if we're not in the layer, return
        if (!PhysicsHelper.IsInLayerMask(otherCollider.gameObject, _layersDetected)) { return; }

        TriggerExited(otherCollider.gameObject);
    }

    protected virtual void TriggerEntered(GameObject objectEntered)
    {

    }

    protected virtual void TriggerExited(GameObject objectEntered)
    {

    }
}
