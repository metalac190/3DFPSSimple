using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDScreen : MonoBehaviour
{
    [Header("HUD Screen")]
    [SerializeField]
    private Canvas _canvas;

    Coroutine _displayRoutine;

    public virtual void Display()
    {
        if (_displayRoutine != null)
            StopCoroutine(_displayRoutine);

        if (_canvas.gameObject.activeSelf == false)
            _canvas.gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        if (_displayRoutine != null)
            StopCoroutine(_displayRoutine);

        if (_canvas.gameObject.activeSelf == true)
            _canvas.gameObject.SetActive(false);
    }

    public virtual void DisplayForDuration(float duration)
    {
        if (_displayRoutine != null)
            StopCoroutine(_displayRoutine);
        _displayRoutine = StartCoroutine(DisplayRoutine(duration));    
    }

    private IEnumerator DisplayRoutine(float duration)
    {
        _canvas.gameObject.SetActive(true);

        yield return new WaitForSeconds(duration);

        _canvas.gameObject.SetActive(false);
    }
}
