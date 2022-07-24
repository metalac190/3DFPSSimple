using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootTriggerFX : MonoBehaviour
{
    [SerializeField] private ShootTrigger _shootTrigger = null;
    [SerializeField] private Color _shotColor = Color.red;
    [SerializeField] private AudioClip _triggerSound = null;
    [SerializeField] private MeshRenderer _shootMesh = null;

    private HitFlash3D _hitFlash = null;

    private void Awake()
    {
        _hitFlash = new HitFlash3D(this, _shootMesh, _shotColor);
    }

    private void OnEnable()
    {
        _shootTrigger.ShotA.AddListener(PlayShotFX);
        _shootTrigger.ShotB.AddListener(PlayShotFX);
    }

    private void OnDisable()
    {
        _shootTrigger.ShotA.RemoveListener(PlayShotFX);
        _shootTrigger.ShotB.RemoveListener(PlayShotFX);
    }

    private void PlayShotFX()
    {
        // FX
        _hitFlash.Flash();
        AudioHelper.PlayClip2D(_triggerSound, 1);
    }


}
