using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SentryEnemyController : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField]
    [Tooltip("Duration that enemy will keep attacking player " +
        " while player is still hidden, before giving up")]
    private float _durationUntilReturnToIdle = 2;
    [SerializeField] private Color _idleEyeColor = Color.green;
    [SerializeField] private Color _aggroEyeColor = Color.red;
    // properties
    public float DurationUntilReturnToIdle => _durationUntilReturnToIdle;
    public Color IdleEyeColor => _idleEyeColor;
    public Color AggroEyeColor => _aggroEyeColor;

    [Header("Projectiles")]
    [SerializeField] private Projectile _projectilePrefab;
    [SerializeField] private int _damage = 3;
    [SerializeField]
    [Tooltip("Number of projectiles fired per second")]
    private float _fireRate = 1;
    [SerializeField]
    [Range(0, 1)] [Tooltip("Closer to 1 is perfect line, Closer to 0 is wide cone")]
    private float _accuracy = .75f;

    public Projectile ProjectilePrefab => _projectilePrefab;
    public int Damage => _damage;
    public float FireRate => _fireRate;
    public float Accuracy => _accuracy;

    [Header("Dependencies")]
    [SerializeField] private PlayerDetector _playerDetector;
    [SerializeField] private MeshRenderer _eyeRenderer;
    [SerializeField] private Health _health;
    [SerializeField] private Rigidbody _rb;
    // properties
    public PlayerDetector PlayerDetector => _playerDetector;
    public MeshRenderer EyeRenderer => _eyeRenderer;
    public Health Health => _health;

    public Rigidbody RB => _rb;

    public Vector3 StartingPosition { get; private set; }

    protected bool IsAggro { get; private set; }

    private void Awake()
    {
        StartingPosition = transform.position;
    }
}
