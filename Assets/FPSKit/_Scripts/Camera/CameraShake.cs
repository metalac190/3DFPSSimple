using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ensure that the camera has 0 transforms by making it a child
/// of a positioner object. This way we can ensure we return back
/// to 0 after a shake
/// </summary>
public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }

    [SerializeField]
    private Camera _camera;

    [Header("Camera Shake")]
    [SerializeField]
    [Range(.01f, 1)]
    private float _magnitude = .4f;

    private Coroutine _shakeCameraRoutine = null;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        FindCameraIfEmpty();
        // ensure camera child object does not have transforms
        _camera.transform.localPosition = new Vector3(0, 0, 0);
        _camera.transform.Rotate(0, 0, 0);
        _camera.transform.localScale = new Vector3(1, 1, 1);
    }

    private void FindCameraIfEmpty()
    {
        if (_camera == null)
        {
            _camera = Camera.main;
            if (_camera == null)
            {
                Debug.LogError("No camera in scene!");
            }
        }
    }

    public void ShakeCamera(float duration)
    {
        if (_shakeCameraRoutine != null)
            StopCoroutine(_shakeCameraRoutine);
        _shakeCameraRoutine = StartCoroutine
            (ShakeCameraRoutine(duration));
    }

    IEnumerator ShakeCameraRoutine(float duration)
    {
        // ensure we start at initial position
        _camera.transform.localPosition = new Vector3(0, 0, 0);
        float elapsed = 0;
        // start our shake loop
        while (elapsed < duration)
        {
            // calculate random positions
            float x = Random.Range(-1f, 1f) * _magnitude;
            float y = Random.Range(-1f, 1f) * _magnitude;
            // set new camera shake position
            _camera.transform.localPosition = new Vector3(x, y, 0);
            // increase elapsed time
            elapsed += Time.deltaTime;
            // wait until next tick
            yield return null;
        }
        // ensure we return to original position
        _camera.transform.localPosition = new Vector3(0,0,0);
    }
}
