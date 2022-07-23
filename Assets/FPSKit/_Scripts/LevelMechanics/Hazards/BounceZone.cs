using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceZone : TriggerVolume
{
    [Header("Bounce Settings")]
    [SerializeField]
    [Tooltip("If true this bounces in opposite direction, instead of indicated direction")]
    private bool _bounceOpposite = false;
    [SerializeField]
    private float _bounceAmount = 35;
    [SerializeField]
    private float _bounceDuration = .5f;
    [SerializeField]
    [Tooltip("TODO not yet functional - Can affected object movign during push. Use this for more controlled knockback.")]
    private bool _canMoveDuring = true;

    [Header("Bounce FX")]
    [SerializeField] private AudioClip _bounceSound;
    [SerializeField] private ParticleSystem _bounceParticles;

    protected override void TriggerEntered(GameObject enteredObject)
    {
        // if it's the player, do player specific things
        // add bounce logic here
        IPushable pushable = enteredObject.GetComponent<IPushable>();
        if(pushable != null)
        {
            Vector3 bounceDirection = transform.up;
            // if we've flagged knockback, get reverse direction instead
            if (_bounceOpposite)
            {
                bounceDirection = PhysicsHelper.ReverseVector
                    (transform.position, enteredObject.transform.position);
            }

            pushable.Push(bounceDirection, _bounceAmount, _bounceDuration, _canMoveDuring);

            if (_bounceParticles != null)
                Instantiate(_bounceParticles, transform.position, transform.rotation);
            if (_bounceSound != null)
                AudioHelper.PlayClip2D(_bounceSound, 1);
        }
    }
}
