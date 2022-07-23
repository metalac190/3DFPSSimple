using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectiblePickup : Pickup
{
    [Header("Collectible Settings")]
    [SerializeField]
    private int _collectibleValue = 1;

    protected override void OnPickup(PlayerCharacter playerCharacter)
    {
        playerCharacter.Inventory.Collectibles += _collectibleValue;
    }
}
