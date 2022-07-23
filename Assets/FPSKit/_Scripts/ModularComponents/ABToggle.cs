using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Alternate between 2 event calls given a delay. Goes indefinitely
/// until disabled. Plug method calls into the UnityEvents in the Inspector.
/// An example might be an Electricity barrier that enables, then disables, repeat.
/// </summary>
public class ABToggle : MonoBehaviour
{
    public UnityEvent ToggleA;
    public UnityEvent ToggleB;

    [SerializeField]
    private float _startOffsetDuration = 0;
    [SerializeField]
    private float _aDuration = 1;
    [SerializeField]
    private float _bDuration = 1;

    private Coroutine _toggleRoutine;

    private void OnEnable()
    {
        StartToggle();
    }

    private void OnDisable()
    {
        StopToggle();
    }

    public void StartToggle()
    {
        _toggleRoutine = StartCoroutine(ToggleObject());
    }

    public void StopToggle()
    {
        if (_toggleRoutine != null)
            StopCoroutine(_toggleRoutine);
    }

    private IEnumerator ToggleObject()
    {
        yield return new WaitForSeconds(_startOffsetDuration);
        // infinite loop sequence, until disabled
        while (true)
        {
            // on
            ToggleA.Invoke();
            yield return new WaitForSeconds(_aDuration);
            ToggleB.Invoke();
            yield return new WaitForSeconds(_bDuration);
        }
    }
}
