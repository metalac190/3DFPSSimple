using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterSeconds : MonoBehaviour
{
    [SerializeField]
    private float _lifeTime = 2;

    private void Awake()
    {
        Destroy(gameObject, _lifeTime);
    }
}
