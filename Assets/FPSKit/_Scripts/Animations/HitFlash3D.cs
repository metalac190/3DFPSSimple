using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// This script will flash a UI Image component with the given parameters. Useful for creating
/// quick, animated UI flashes.
/// Created by: Adam Chandler
/// Make sure that you attach this script to an Image component. You can optionally call the
/// flash remotely and pass in new flash values, or you can predefine settings in the Inspector
/// </summary>

public class HitFlash3D
{
    private MeshRenderer _renderer;
    private MonoBehaviour _monobehaviour;

    private Color _flashColor;

    private float _flashInDuration = .05f;
    private float _flashHoldDuration;
    private float _flashOutDuration = .05f;

    private Color _startingColor;
    private Coroutine _flashRoutine = null;

    public HitFlash3D(MonoBehaviour monobehaviour, MeshRenderer renderer,
        Color flashColor, float flashDuration = .15f)
    {
        _monobehaviour = monobehaviour;
        _renderer = renderer;
        _flashColor = flashColor;
        _startingColor = _renderer.material.GetColor("_EmissionColor");

        CalculateFlashBlends(flashDuration);
    }

    private void CalculateFlashBlends(float flashDuration)
    {
        // if our flash isn't valid, don't do it
        if (flashDuration <= 0)
        {
            _flashInDuration = 0;
            _flashHoldDuration = 0;
            _flashOutDuration = 0;
        }
        // if we don't have enough time for transitions, just hold the flash
        else if (flashDuration < _flashInDuration + _flashOutDuration)
        {
            _flashInDuration = 0;
            _flashHoldDuration = flashDuration;
            _flashOutDuration = 0;
        }
        else
        {
            _flashHoldDuration = flashDuration - (_flashInDuration + _flashOutDuration);
        }
    }

    #region Public Functions

    public void Flash()
    {
        if (_flashInDuration <= 0) { return; }    // 0 speed wouldn't make sense

        if (_flashRoutine != null)
            StopFlash();
        _flashRoutine = _monobehaviour.StartCoroutine(FlashRoutine(_flashColor, _flashInDuration, 
            _flashHoldDuration, _flashOutDuration));
    }

    public void StopFlash()
    {
        if (_flashRoutine != null)
            _monobehaviour.StopCoroutine(_flashRoutine);

        SetInitialValues();
    }
    #endregion

    #region Private Functions
    IEnumerator FlashRoutine(Color flashColor, float flashInDuration, 
        float flashHoldDuration, float flashOutDuration)
    {
        _renderer.material.EnableKeyword("_EMISSION");
        // flash in
        for (float elapsed = 0; elapsed <= flashInDuration; elapsed += Time.deltaTime)
        {
            Color newColor = Color.Lerp(_startingColor, flashColor, elapsed / flashInDuration);
            _renderer.material.SetColor("_EmissionColor", newColor);
            yield return null;
        }
        // hold
        yield return new WaitForSeconds(_flashHoldDuration);
        // flash out
        for (float elapsed = 0; elapsed <= flashOutDuration; elapsed += Time.deltaTime)
        {
            Color newColor = Color.Lerp(flashColor, _startingColor, elapsed / flashOutDuration);
            _renderer.material.SetColor("_EmissionColor", newColor);
            yield return null;
        }
        SetInitialValues();
    }

    private void SetInitialValues()
    {
        _renderer.material.SetColor("_EmissionColor", _startingColor);
        _renderer.material.DisableKeyword("_EMISSION");
    }

    #endregion
}

