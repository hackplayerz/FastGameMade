using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AIController : MonoBehaviour
{
    enum AIState
    {
        Slow = 2,
        Fast = 9
    }
    [SerializeField]private AIState _aiState = AIState.Fast;

    private Transform _ballTransform;
    private Transform _transform;

    private void Awake()
    {
        _ballTransform = FindObjectOfType<Ball>().transform;
        _transform = transform;
    }

    private void Update()
    {
        var position = _transform.position;
        position = Vector2.Lerp(position, new Vector2(position.x, _ballTransform.position.y),
                                (float)_aiState/ 100);
        position.y = Mathf.Clamp(position.y, -5f, 5f);
        _transform.position = position;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag.Equals("Ball"))
        {
            _aiState = Random.Range(0, 100) < 30 ? AIState.Fast : AIState.Slow;
        }
    }
}
