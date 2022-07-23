using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveState
{
    Inactive,
    Moving,
    Paused
}

public class MoveBetweenPoints : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField][Tooltip("Seconds from start to end location")]
    private float _secondsUntilDestination = 1;
    [SerializeField][Tooltip("Duration in seconds to pause after reaching destination")]
    private float _pauseDuration = 1;

    [Header("General Settings")]
    [SerializeField][Tooltip("True will begin movement on scene start. Disable if you'd like to control" +
        " the start movement with a trigger event")]
    private bool _activateOnAwake = true;
    [SerializeField][Tooltip("Duration before starting movement. Use this to stagger timing while" +
        " retaining the desired speed")]
    private float _startDelay = 0;

    [Header("Dependencies")]
    [SerializeField]
    private Rigidbody _rb;
    [SerializeField]
    private Collider _objectCollider;
    [SerializeField]
    [Tooltip("Destination transform to move towards")]
    private Transform _endLocation;

    [Header("Gizmo Settings")]
    [SerializeField]
    private Color _endWireColor = Color.grey;

    private MoveState _moveState = MoveState.Inactive;

    private int _tripCounter = 1;
    private float _movingElapsedTime;
    private Vector3 _startPosition;
    private Vector3 _endPosition;
    private Coroutine _moveRoutine;
    // we need to save our movement change so we can carry other objects
    public Vector3 PreviousPosition { get; private set; }
    public Vector3 Velocity => (_rb.position - PreviousPosition) / Time.fixedDeltaTime;

    private void Awake()
    {
        _rb.isKinematic = true;
        _rb.interpolation = RigidbodyInterpolation.Interpolate;

        _startPosition = _rb.position;
        // error-check our end location
        if (_endLocation != null)
            _endPosition = _endLocation.position;
        else
        {
            _endPosition = _rb.position;
            Debug.LogWarning("No end location set on moving platform");
        }
        // always start in forward direction

        if (_activateOnAwake)
        {
            Activate();
        }
    }

    public void Activate()
    {
        _moveRoutine = StartCoroutine(MoveRoutine(_secondsUntilDestination));
    }

    public void Stop()
    {
        if (_moveRoutine != null)
            StopCoroutine(_moveRoutine);
        _moveState = MoveState.Inactive;
    }

    void Update()
    {
        //If the platform is moving, make sure we're counting it to the timer
        if (_moveState == MoveState.Moving)
        {
            // this only counts time moving, to ensure we're counting properly
            _movingElapsedTime += Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        PreviousPosition = _rb.position;
    }

    IEnumerator MoveRoutine(float secondsUntilDestination)
    {
        // check for start delay before beginning our loop
        if(_startDelay >= 0)
        {
            _moveState = MoveState.Paused;
            yield return new WaitForSeconds(_startDelay);
        }

        _moveState = MoveState.Moving;
        while (true)
        {
            switch (_moveState)
            {
                case MoveState.Moving:
                    Move(secondsUntilDestination);
                    break;
                case MoveState.Paused:
                    yield return new WaitForSeconds(_pauseDuration);
                    _moveState = MoveState.Moving;
                    break;
            }

            yield return new WaitForFixedUpdate();
        }
    }

    private void Move(float secondsUntilDestination)
    {
        float moveRatio = Mathf.PingPong(_movingElapsedTime / secondsUntilDestination, 1f);
        // saving previous position allows us to calculate the move vector for objects that need to be carried

        Vector3 newPosition = Vector3.Lerp(_startPosition, _endPosition,
            Mathf.SmoothStep(0f, 1f, moveRatio));

        _rb.MovePosition(newPosition);

        if ((_movingElapsedTime / _tripCounter) >= _secondsUntilDestination)
        {
            _moveState = MoveState.Paused;
            _tripCounter++;
        }
    }

    private void OnDrawGizmos()
    {
        if(_objectCollider == null || _endLocation == null) { return; }

        Gizmos.color = _endWireColor;
        // Draw differently if we're in playmode, since at start of play we save start/end
        // We do this because child object movement on transform can mess up the drawing
        if (Application.isPlaying)
        {
            // start
            Gizmos.DrawWireCube(_startPosition, _objectCollider.bounds.size);
            // end
            Gizmos.DrawWireCube(_endPosition, _objectCollider.bounds.size);
            // line
            Gizmos.DrawLine(_startPosition, _endPosition);
        }
        else
        {
            // start
            Gizmos.DrawWireCube(_objectCollider.transform.position, _objectCollider.bounds.size);
            // end
            Gizmos.DrawWireCube(_endLocation.position, _objectCollider.bounds.size);
            // draw path connection Gizmo
            Gizmos.DrawLine(_objectCollider.transform.position, _endLocation.position);
        }

    }
}
