using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// this class simulates a simple 'fill bar' for health, resources, etc.
/// Scale is between 0-1, so it auto calculates the normalized value with the parameters
/// </summary>
public class SimpleFillBar : MonoBehaviour
{
    [SerializeField] 
    private Image _fillImageUI;
    //[SerializeField]
    //private bool _animateDamage = true;

    public void SetScale(float newAmount, float maxAmount)
    {
        float newXScale = (1 / maxAmount) * newAmount;
        newXScale = Mathf.Clamp(newXScale, 0, 1);
        _fillImageUI.transform.localScale = new Vector3(newXScale, 1, 1);
    }

}
