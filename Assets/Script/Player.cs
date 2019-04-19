using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] [Range(5,20)] private float _speed;
    private Transform _transform;

    private void Awake()
    {
        _transform =transform;
    }

    private void Update()
    {
        float axis = Input.GetAxisRaw("Vertical");
        _transform.Translate(Vector3.up * axis * _speed * Time.deltaTime);
        if (_transform.position.y > 5)
        {
            _transform.position = new Vector3(_transform.position.x,5);
        }

        if (_transform.position.y < -5)
        {
            _transform.position = new Vector3(_transform.position.x,-5);
        }
    }
}
