using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private Inventory _inventory;

    public Health Health => _health;
    public Inventory Inventory => _inventory;
}
