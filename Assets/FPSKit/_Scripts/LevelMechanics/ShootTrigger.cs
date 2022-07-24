using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShootTrigger : MonoBehaviour
{
    [SerializeField]
    private bool _isOneShot;
    [SerializeField]
    private bool _isToggle;


    [Tooltip("Calls this event on first shot, repeating every other")]
    public UnityEvent ShotA;
    [Tooltip("Calls this event on alternate shot, repeating every other")]
    public UnityEvent ShotB;

    [Header("Dependencies")]
    [SerializeField] private Health _health;

    private bool _usedOneShot = false;
    private bool _isActivated = false;

    private void Awake()
    {
        _health.Max = 1;
        _health.Current = 1;
        _health.HasInfiniteHealth = true;
        _health.HitInvulDuration = 0;
    }

    private void OnEnable()
    {
        _health.Damaged.AddListener(TriggerShotActivation);
    }

    private void OnDisable()
    {
        _health.Damaged.RemoveListener(TriggerShotActivation);
    }

    private void TriggerShotActivation(int damage)
    {
        Debug.Log("Shoot Trigger Activated!");
        // if this is a one shot and we've already done it, don't act again
        if(_isOneShot && _usedOneShot) { return; }
        // if this is a one shot, make sure we flag that we've used it so we know in the future
        if(_isOneShot && _usedOneShot == false)
        {
            _usedOneShot = true;
        }
        // toggle our bool to decide which event to fire
        _isActivated = !_isActivated;
        if (_isActivated)
        {
            ShotA?.Invoke();
        }
        else
        {
            ShotB?.Invoke();
        }

    }
}
