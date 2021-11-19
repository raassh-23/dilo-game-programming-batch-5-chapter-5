using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bird : MonoBehaviour
{
    public enum BirdState { Idle, Thrown, HitSomething }

    public GameObject parent;
    public Rigidbody2D rigidbody2d;
    public CircleCollider2D circleCollider;

    public UnityAction OnBirdDestroyed = delegate { };
    public UnityAction<Bird> OnBirdShot = delegate { };

    private BirdState _state;
    public BirdState State { get { return _state; } }

    private float _minVelocity = 0.65f;
    private bool _flagDestroy = false;

    private void Start()
    {
        rigidbody2d.bodyType = RigidbodyType2D.Kinematic;
        circleCollider.enabled = false;
        _state = BirdState.Idle;
    }

    private void FixedUpdate()
    {
        if (_state == BirdState.Idle &&
            rigidbody2d.velocity.sqrMagnitude >= _minVelocity)
        {
            _state = BirdState.Thrown;
        }

        if ((_state == BirdState.Thrown || _state == BirdState.HitSomething) &&
            rigidbody2d.velocity.sqrMagnitude < _minVelocity &&
            !_flagDestroy)
        {
            _flagDestroy = true;
            StartCoroutine(DestroyAfter(2));
        }
    }

    private IEnumerator DestroyAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }

    public void MoveTo(Vector2 target, GameObject parent)
    {
        gameObject.transform.SetParent(parent.transform);
        gameObject.transform.position = target;
    }

    public void Shoot(Vector2 velocity, float distance, float speed)
    {
        circleCollider.enabled = true;
        rigidbody2d.bodyType = RigidbodyType2D.Dynamic;
        rigidbody2d.velocity = velocity * speed * distance;
        OnBirdShot(this);
    }

    public void OnDestroy()
    {
        if (_state == BirdState.Thrown || _state == BirdState.HitSomething)
        {
            OnBirdDestroyed();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        _state = BirdState.HitSomething;
    }

    public virtual void OnTap()
    {
        // Nothing
    }
}
