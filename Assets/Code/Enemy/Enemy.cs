using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int _health = 10;
    [SerializeField] private float _xMaxSpeed = 3f;
    [SerializeField] private float _yMaxSpeed = 3f;
    [SerializeField] private float _xAccel = 0.1f;
    [SerializeField] private float _yAccel = 0.1f;
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private LayerMask _layerMask;

    public int DamageDealt;

    private Rigidbody2D _rigidBody;
    private Collider2D _boxCollider;
    private GameObject _target;
    private State _state;
    private enum State
    {
        Idle = 0,
        Running = 1
    }

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<Collider2D>();
        _target = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        MoveToTarget();
    }

    private void MoveToTarget()
    {
        SetXSpeed();
        SetYSpeed();
        JumpIfNeeded();
    }

    private void JumpIfNeeded()
    {
        bool targetIsAbove = _target.transform.position.y > this.transform.position.y + _boxCollider.bounds.size.y / 2; //only jump if player is above the top of the box
        //if (IsGrounded() && (targetIsAbove || IsBlockedByWall()))
        //{
        //    Jump();
        //}
    }

    private void SetXSpeed()
    {
        //Set horizontal speed
        //if player is left of enemy
        if (_target.transform.position.x < this.transform.position.x)
        {
            //current speed - accel*time
            var xSpeed = _rigidBody.velocity.x - (_xAccel * Time.deltaTime);

            //constrain to max speed
            if (xSpeed < -_xMaxSpeed)
            {
                xSpeed = -_xMaxSpeed;
            }

            //set velocity
            _rigidBody.velocity = new Vector2(xSpeed, _rigidBody.velocity.y);
        }
        //if player is right of enemy
        else if (_target.transform.position.x > this.transform.position.x)
        {
            var xSpeed = _rigidBody.velocity.x + (_xAccel * Time.deltaTime);

            if (xSpeed > _xMaxSpeed)
            {
                xSpeed = _xMaxSpeed;
            }
            _rigidBody.velocity = new Vector2(xSpeed, _rigidBody.velocity.y);
        }
    }

    private void SetYSpeed()
    {
        //Set horizontal speed
        //if player is left of enemy
        if (_target.transform.position.y < this.transform.position.y)
        {
            //current speed - accel*time
            var ySpeed = _rigidBody.velocity.y - (_yAccel * Time.deltaTime);

            //constrain to max speed
            if (ySpeed < -_yMaxSpeed)
            {
                ySpeed = -_yMaxSpeed;
            }

            //set velocity
            _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, ySpeed);
        }
        //if player is right of enemy
        else if (_target.transform.position.y > this.transform.position.y)
        {
            var ySpeed = _rigidBody.velocity.y + (_yAccel * Time.deltaTime);

            if (ySpeed > _yMaxSpeed)
            {
                ySpeed = _yMaxSpeed;
            }
            _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, ySpeed);
        }
    }

    private void SetState()
    {
        _state = _rigidBody.velocity == Vector2.zero ? State.Idle : State.Running;
    }

    //private void Jump()
    //{
    //    _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, _jumpForce);
    //    _state = State.Jumping;
    //}

    //private bool IsGrounded()
    //{
    //    var raycastHit = Physics2D.Raycast(_boxCollider.bounds.center, Vector2.down, _boxCollider.bounds.extents.y + 0.1f, _layerMask);
    //    return raycastHit.collider != null; ;
    //}

    private bool IsBlockedByWall()
    {
        var direction = this.transform.position.x > 0 ? Vector2.left : Vector2.right;
        var rayCast = Physics2D.Raycast(_boxCollider.bounds.center, direction, _boxCollider.bounds.extents.y + 0.1f, _layerMask);
        return rayCast.collider != null;
    }
}
