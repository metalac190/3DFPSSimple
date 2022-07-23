using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnOverlap : MonoBehaviour
{
    [SerializeField] private LayerMask _layerToDamage;
    [SerializeField] private float _damageTouchFrequency = .2f;
    [SerializeField] private int _touchDamage = 5;

    [SerializeField] private Transform _touchDamageCheck;
    [SerializeField] private float _touchDamageWidth;
    [SerializeField] private float _touchDamageHeight;

    float _lastTouchDamageTime = 0;

    private Vector2 _touchDamageBotLeft;
    private Vector2 _touchDamageTopRight;

    private void FixedUpdate()
    {
        CheckTouchDamage();
    }

    private void CheckTouchDamage()
    {
        // if we've gone long enough without being damaged
        if (Time.time >= _lastTouchDamageTime + _damageTouchFrequency)
        {
            // create bounds
            _touchDamageBotLeft.Set(_touchDamageCheck.position.x - (_touchDamageWidth / 2),
                _touchDamageCheck.position.y - (_touchDamageHeight / 2));
            _touchDamageTopRight.Set(_touchDamageCheck.position.x + (_touchDamageWidth / 2),
                _touchDamageCheck.position.y + (_touchDamageHeight / 2));
            // test with bounds
            Collider2D[] colliders = Physics2D.OverlapAreaAll(_touchDamageBotLeft, _touchDamageTopRight, _layerToDamage);
            foreach(Collider2D collider in colliders)
            {
                Debug.Log("Apply Damage to player");
                Health health = collider.GetComponent<Health>();
                health?.Damage(_touchDamage);
            }

            _lastTouchDamageTime = Time.time;
        }
    }

    private void OnDrawGizmos()
    {
        Vector2 botLeft = new Vector2(_touchDamageCheck.position.x - (_touchDamageWidth / 2),
            _touchDamageCheck.position.y - (_touchDamageHeight / 2));
        Vector2 botRight = new Vector2(_touchDamageCheck.position.x + (_touchDamageWidth / 2),
            _touchDamageCheck.position.y - (_touchDamageHeight / 2));
        Vector2 topRight = new Vector2(_touchDamageCheck.position.x + (_touchDamageWidth / 2),
            _touchDamageCheck.position.y + (_touchDamageHeight / 2));
        Vector2 topLeft = new Vector2(_touchDamageCheck.position.x - (_touchDamageWidth / 2),
            _touchDamageCheck.position.y + (_touchDamageHeight / 2));

        Gizmos.color = Color.red;
        Gizmos.DrawLine(botLeft, botRight);
        Gizmos.DrawLine(botRight, topRight);
        Gizmos.DrawLine(topRight, topLeft);
        Gizmos.DrawLine(topLeft, botLeft);
    }

    /*
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Knockback Enter");
        // if we've gone long enough without being damaged
        if (Time.time >= _lastTouchDamageTime + _touchDamageCooldown)
        {
            // if it's the player, apply damage/knockback and reset cooldown
            if (collision.gameObject.CompareTag("Player"))
            {
                Debug.Log("Apply Damage to player");
                Health health = collision.gameObject.GetComponent<Health>();
                if (health != null)
                {
                    health.TakeDamage(_touchDamage);
                }

                _lastTouchDamageTime = Time.time;
            }
        }
    }
    */
}
