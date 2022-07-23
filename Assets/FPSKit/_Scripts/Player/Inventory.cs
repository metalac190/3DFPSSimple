using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Inventory : MonoBehaviour
{
    public event Action<int> CollectiblesChanged;
    public event Action<int> KeysChanged;

    public const int _collectiblesMaxHeld = 999;
    private int _collectibles = 0;
    public int Collectibles 
    {
        get => _collectibles;
        set
        {
            value = Mathf.Clamp(value, 0, _collectiblesMaxHeld);
            if(value != _collectibles)
            {
                CollectiblesChanged?.Invoke(value);
            }
            _collectibles = value;
        }
    }

    public const int _keyMaxHeld = 9;
    private int _keys = 0;
    public int Keys
    {
        get => _keys;
        set
        {
            value = Mathf.Clamp(value, 0, _keyMaxHeld);
            if (value != _keys)
            {
                KeysChanged?.Invoke(value);
            }
            _keys = value;
        }
    }
}
