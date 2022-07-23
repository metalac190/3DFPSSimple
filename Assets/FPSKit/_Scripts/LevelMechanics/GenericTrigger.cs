using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GenericTrigger : TriggerVolume
{
    [SerializeField]
    [Tooltip("If true, this trigger will only fire once. " +
    "If false it can be triggered each time the volume is entered")]
    private bool _oneShot = true;
    [SerializeField]
    [Tooltip("Only this specific object will activate this trigger")]
    private GameObject _specificTriggerObject = null;

    public UnityEvent Entered;
    public UnityEvent Exited;

    [Header("Gizmo Settings")]
    [SerializeField]
    private bool _displayGizmos = false;
    [SerializeField]
    private bool _showOnlyWhileSelected = true;
    [SerializeField]
    private Color _gizmoColor = Color.green;

    private bool _alreadyEntered = false;

    protected override void TriggerEntered(GameObject objectEntered)
    {
        // if it's a oneshot and we've already fired it, ignore
        if (_oneShot && _alreadyEntered) { return; }
        // if we have a specified object and this is not that object, ignore
        if (_specificTriggerObject != null
            && objectEntered != _specificTriggerObject) { return; }

        Entered.Invoke();
    }

    protected override void TriggerExited(GameObject objectEntered)
    {
        Exited.Invoke();
    }

    public void ResetTrigger()
    {
        _alreadyEntered = false;
    }

    private void OnDrawGizmos()
    {
        if (_displayGizmos && _showOnlyWhileSelected == false)
        {
            DrawTriggerBounds();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (_displayGizmos && _showOnlyWhileSelected)
        {
            DrawTriggerBounds();
        }
    }

    private void DrawTriggerBounds()
    {
        // if we don't have a collider, attempt to find it
        if (Collider == null)
        {
            Collider = GetComponent<Collider>();
            // if we still don't have it, it's an error. Disable
            if (Collider == null)
            {
                Debug.LogWarning("No collider on " + gameObject.name + ". Disabling");
                gameObject.SetActive(false);
            }
        }
        // set the color and draw it
        Gizmos.color = _gizmoColor;
        Vector3 position = new Vector3(transform.position.x + Collider.bounds.center.x,
            transform.position.y + Collider.bounds.center.y, 0);
        Gizmos.DrawCube(position, Collider.bounds.size);
    }
}
