using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class Ball : MonoBehaviour
{
    [SerializeField] private float _speed;    
    
    private Vector2 _moveDirection;
    private Rigidbody2D _rigidbody2D;
    private bool _isToPlayer = true;
    private float _firstSpeed;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        _firstSpeed = _speed;
        ChangeDirection(Random.Range(0,100)/2 > 25 ? _isToPlayer : !_isToPlayer);
    }

    private void FixedUpdate()
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
        velocity.x = Mathf.Clamp(velocity.x, -20, 20);
        velocity.y = Mathf.Clamp(velocity.y, -20, 20);
        _rigidbody2D.velocity = velocity;
        _moveDirection = velocity;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag.Equals("Player"))
        {
            _isToPlayer = false;
            ChangeDirection(_isToPlayer);
        }
        else if (other.collider.tag.Equals("AI"))
        {
            _isToPlayer = true;
            ChangeDirection(_isToPlayer);
        }
        else if (other.collider.tag.Equals("Goal"))
        {
            bool isPlayerGoal = transform.position.x < 0;
            
            UiManager.Instance.GetPoint(isPlayerGoal);
            Reset();
        }
    }

    private void Reset()
    {
        transform.position = Vector3.zero;
        _moveDirection = Vector2.zero;
        _speed = _firstSpeed;
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
