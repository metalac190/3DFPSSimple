using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : TriggerVolume
{
    [Header("Save Settings")]
    [SerializeField]
    [Tooltip("You can position the respawn point to be different than " +
        "the save volume. This is useful for getting exact placement for respawning" +
        " the player.")]
    private Transform _newSpawnPoint;
    [SerializeField] private ParticleSystem _saveParticlePrefab;
    [SerializeField] private AudioClip _saveSound;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void TriggerEntered(GameObject enteredObject)
    {
        Debug.Log("Set new spawn point");
        // if we're not in the layer, return
        PlayerCharacter player = enteredObject.GetComponent<PlayerCharacter>();
        if (player != null)
        {
            GameSession.Instance.SavePlayerData(_newSpawnPoint.transform.position, player);
            if(_saveParticlePrefab != null)
            {
                Instantiate(_saveParticlePrefab, _newSpawnPoint.transform.position, 
                    Quaternion.identity);
            }
            if (_saveSound != null)
                AudioHelper.PlayClip2D(_saveSound, 1);
        }
    }
}
