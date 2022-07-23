using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : Pickup
{
    [SerializeField]
    private int _healAmount = 1;

    protected override void OnPickup(PlayerCharacter playerCharacter)
    {
        playerCharacter.Health.Heal(_healAmount);
    }
}
