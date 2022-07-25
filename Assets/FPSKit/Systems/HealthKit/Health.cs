using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public event Action<int> HealthChanged;

    [Header("Health")]
    [SerializeField] private int _max = 10;
    [SerializeField] private int _current = 10;
    [SerializeField] [Tooltip("Sometimes we want to be able to hit things but we want them" +
        " to not actually take damage. Use this to receive hits but not subtract health")]
    private bool _hasInfiniteHealth = false;
    [SerializeField]
    [Tooltip("If this object has health but we don't want to damage it or receive hits at certain" +
        " points, use this")]
    private bool _isDamageable = true;
    [SerializeField] private float _hitInvulDuration = .5f;

    public UnityEvent<int> Damaged;
    public UnityEvent<GameObject> Died;

    public bool IsDamageable
    {
        get => _isDamageable;
        set => _isDamageable = value;
    }

    public int Current 
    {
        get => _current;
        set
        {
            value = Mathf.Clamp(value, 0, _max);
            if(value != _current)
            {
                HealthChanged?.Invoke(value);
            }
            _current = value;
        }
    }

    public int Max
    {
        get => _max;
        set
        {
            if (value < 1)
                value = 1;
            _max = value;
        }
    }

    public bool HasInfiniteHealth 
    {
        get => _hasInfiniteHealth;
        set { _hasInfiniteHealth = value; } 
    }

    public float HitInvulDuration
    {
        get => _hitInvulDuration;
        set
        {
            Math.Abs(value);
            _hitInvulDuration = value;
        }
    } 
    public bool IsHitInvul { get; private set; } = false;

    public virtual void Heal(int amount)
    {
        Current += amount;
    }

    public virtual void Damage(int amount)
    {
        if (!_isDamageable) return;
        if (IsHitInvul) return;
        // only subtract health if we don't have 'infinite'
        if(HasInfiniteHealth == false)
        {
            Current -= amount;
        }
        // either way, send damaged event
        Damaged?.Invoke(amount);
        //Debug.Log("Damaged. New Health " + Current);
        Current = Mathf.Clamp(Current, 0, _max);
        // start hit invulnerability
        IsHitInvul = true;
        Invoke(nameof(RemoveHitInvul), _hitInvulDuration);

        if(Current == 0)
        {
            Kill();
        }
    }

    public virtual void Kill()
    {
        Died?.Invoke(gameObject);
        // extra assurance to make sure we clean up events
        Damaged?.RemoveAllListeners();
        Died?.RemoveAllListeners();

        Destroy(gameObject);
        //gameObject.SetActive(false);
    }

    private void RemoveHitInvul()
    {
        IsHitInvul = false;
    }

    private IEnumerator HitInvulRoutine(float duration)
    {
        IsHitInvul = true;
        yield return new WaitForSeconds(duration);
        IsHitInvul = false;
    }
}
