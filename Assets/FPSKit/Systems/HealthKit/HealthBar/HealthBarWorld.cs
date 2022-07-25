using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarWorld : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField][Tooltip("Set width of Healthbar for per-unit variation")]
    float _width = 1500;
    [SerializeField] [Tooltip("Set height of Healthbar for per-unit variation")]
    float _height = 500;
    [SerializeField]
    [Tooltip("Reposition with offset in case visuals get in the way")]
    private Vector3 _localOffset = new Vector3(0, 2.5f, 0);
    [SerializeField]
    [Tooltip("Orients healthbar to always face player camera")]
    private bool _faceCamera = true;

    [Header("Animation")]
    [SerializeField] float _animateSpeed = .3f;
    private float _targetHealth = 1;

    [Header("Dependencies")]
    [SerializeField] private Health _health = null;
    [SerializeField] private RectTransform _canvasTransform;
    [SerializeField] private RectTransform _panel;
    [SerializeField] private Image _fillBar = null;

    private Camera _camera;

    
    private void Awake()
    {
        ResizeHealthBar();
    }

    private void Start()
    {
        _camera = Camera.main;
        // ensure starting health gets updated graphically at the beginning
        if (_health != null)
        {
            UpdateHealthBar(_health.Max, _health.Current);
        }
    }

    private void OnEnable()
    {
        if(_health != null)
        {
            _health.HealthChanged += OnHealthChanged;
        }
        else
        {
            Debug.LogWarning("No health script assigned on the healthbar: " + gameObject.name);
        }
    }

    private void OnDisable()
    {
        if (_health != null)
        {
            _health.HealthChanged -= OnHealthChanged;
        }
        else
        {
            Debug.LogWarning("No health script assigned on the healthbar: " + gameObject.name);
        }
    }

    private void Update()
    {
        if (_faceCamera)
        {
            transform.rotation = Quaternion.LookRotation(transform.position - _camera.transform.position);
        }
        // if the fill amount is not near the target, we still need to animate
        if(Mathf.Approximately(_fillBar.fillAmount, _targetHealth) == false)
        {
            _fillBar.fillAmount = Mathf.MoveTowards(_fillBar.fillAmount,
                _targetHealth, _animateSpeed * Time.deltaTime);

        }
    }

    private void ResizeHealthBar()
    {
        _panel.sizeDelta = new Vector3(_width, _height);
        _canvasTransform.localPosition = _localOffset; 
    }

    public void UpdateHealthBar(float maxHealth, float currentHealth)
    {
        _targetHealth = currentHealth / maxHealth;
    }

    private void OnHealthChanged(int newHealth)
    {
        UpdateHealthBar(_health.Max, newHealth);
    }
}
