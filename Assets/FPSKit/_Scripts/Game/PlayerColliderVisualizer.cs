using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColliderVisualizer : MonoBehaviour
{
    [SerializeField]
    private CapsuleCollider _playerPrefabCollider;
    [SerializeField]
    private Color _wireframeColor = Color.cyan;

    private void OnDrawGizmosSelected()
    {
        if (_playerPrefabCollider != null)
        {
            Gizmos.color = _wireframeColor;
            Gizmos.DrawWireCube(transform.position + _playerPrefabCollider.center, 
                new Vector3(_playerPrefabCollider.radius * 2, 
                _playerPrefabCollider.height, 
                _playerPrefabCollider.radius * 2));
        }
    }
}
