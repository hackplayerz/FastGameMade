using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(AudioSource))]
public class Ball : MonoBehaviour
{
    enum BallState
    {
        Play,
        Goal
    }
    
    [Header("Ball State")]
    [SerializeField] private float _speed;
    [SerializeField] private AudioClip _bounceClip;
    [SerializeField] private AudioClip _getPointClip;

    private BallState _state = BallState.Play;
    
    private AudioSource _audio;
    private Vector2 _moveDirection;
    private Rigidbody2D _rigidbody2D;
    private bool _isToPlayer = true;
    private float _firstSpeed;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _audio = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        _firstSpeed = _speed;
        ChangeDirection(Random.Range(0,100)/2 > 25 ? _isToPlayer : !_isToPlayer);
    }

    private void FixedUpdate()
    {
        if (_state.Equals(BallState.Play))
        {
            var velocity = _rigidbody2D.velocity;
            if (velocity.x * velocity.x < 16f)
            {
                velocity.x = 4 * (_isToPlayer ? 1 : -1);
            }

            if (velocity.y * velocity.y < 4f)
            {
                velocity.y *= 2;
            }

            velocity.x            = Mathf.Clamp(velocity.x, -20, 20);
            velocity.y            = Mathf.Clamp(velocity.y, -20, 20);
            _rigidbody2D.velocity = velocity;
            _moveDirection        = velocity;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag.Equals("Player"))
        {
            _audio.PlayOneShot(_bounceClip);
            _isToPlayer = false;
            ChangeDirection(_isToPlayer);
        }
        else if (other.collider.tag.Equals("AI"))
        {
            _audio.PlayOneShot(_bounceClip);
            _isToPlayer = true;
            ChangeDirection(_isToPlayer);
        }
        else if (other.collider.tag.Equals("Goal"))
        {
            bool isPlayerGoal = transform.position.x < 0;
            var position = transform.position;
            _state = BallState.Goal;
            _audio.PlayOneShot(_getPointClip);
            UiManager.Instance.GetPoint(isPlayerGoal);
            _rigidbody2D.position = position;
            _rigidbody2D.velocity =Vector2.zero;
            _rigidbody2D.isKinematic = true;
            StartCoroutine(Reset());
        }
        else
        {
            _audio.PlayOneShot(_bounceClip);
        }
    }

    private IEnumerator Reset()
    {
        yield return new WaitForSeconds(1f);
        _rigidbody2D.isKinematic = false;
        transform.position = Vector3.zero;
        _moveDirection = Vector2.zero;
        _speed = _firstSpeed;
        _state = BallState.Play;
        ChangeDirection(Random.Range(0,100)/2 > 25 ? _isToPlayer : !_isToPlayer);
    }

    private void ChangeDirection(bool isToPlayer)
    {
        float yDirection = Random.Range(.1f, .7f);
        var beforeSpeed = _speed;
        _speed = Random.Range(beforeSpeed / 2f, beforeSpeed * 2f);
        
        _moveDirection = new Vector2(isToPlayer ? 1 : -1, transform.position.y > 0 ? yDirection : -yDirection);
        
        _rigidbody2D.velocity = _moveDirection * _speed;
    }
}
