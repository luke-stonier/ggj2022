using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Pathfinding;

public class Enemy : MonoBehaviour
{
    public int Health = 10;
    public float XMaxSpeed = 3f;
    public float YMaxSpeed = 3f;
    public float xAccel = 0.1f;
    public float YAccel = 0.1f;
    public float AggroRange = 15f;
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] public LayerMask _layerMask;
    public bool IsRecentlyAggro = false;
    public float AggroTimeRemaining = 0f;

    public int DamageDealt;
    public bool SeenPlayer = false;
    public GameObject Target;
    public Rigidbody2D RigidBody;

    public Collider2D BoxCollider;

    private State _state;
    private enum State
    {
        Idle = 0,
        ChasingTarget = 1
    }

    // Start is called before the first frame update
    void Start()
    {
        RigidBody = GetComponent<Rigidbody2D>();
        BoxCollider = GetComponent<Collider2D>();
        Target = GameObject.FindWithTag("Player");
        var destinationSetter = GetComponent<AIDestinationSetter>();
        destinationSetter.target = Target.transform;
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfDead();
        MoveToTarget();
    }

    private void CheckIfDead()
    {
        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void MoveToTarget()
    {
        EnemyMovementController.MoveV2(this);
        //SetXSpeed();
        //SetYSpeed();
        //JumpIfNeeded();
    }

    //private void JumpIfNeeded()
    //{
    //    bool targetIsAbove = _target.transform.position.y > this.transform.position.y + _boxCollider.bounds.size.y / 2; //only jump if player is above the top of the box
    //    //if (IsGrounded() && (targetIsAbove || IsBlockedByWall()))
    //    //{
    //    //    Jump();
    //    //}
    //}

    private void SetXSpeed()
    {
        //Set horizontal speed
        //if player is left of enemy
        if (Target.transform.position.x < this.transform.position.x)
        {
            //current speed - accel*time
            var xSpeed = RigidBody.velocity.x - (xAccel * Time.deltaTime);

            //constrain to max speed
            if (xSpeed < -XMaxSpeed)
            {
                xSpeed = -XMaxSpeed;
            }

            //set velocity
            RigidBody.velocity = new Vector2(xSpeed, RigidBody.velocity.y);
        }
        //if player is right of enemy
        else if (Target.transform.position.x > this.transform.position.x)
        {
            var xSpeed = RigidBody.velocity.x + (xAccel * Time.deltaTime);

            if (xSpeed > XMaxSpeed)
            {
                xSpeed = XMaxSpeed;
            }
            RigidBody.velocity = new Vector2(xSpeed, RigidBody.velocity.y);
        }
    }

    private void SetYSpeed()
    {
        //Set horizontal speed
        //if player is left of enemy
        if (Target.transform.position.y < this.transform.position.y)
        {
            //current speed - accel*time
            var ySpeed = RigidBody.velocity.y - (YAccel * Time.deltaTime);

            //constrain to max speed
            if (ySpeed < -YMaxSpeed)
            {
                ySpeed = -YMaxSpeed;
            }

            //set velocity
            RigidBody.velocity = new Vector2(RigidBody.velocity.x, ySpeed);
        }
        //if player is right of enemy
        else if (Target.transform.position.y > this.transform.position.y)
        {
            var ySpeed = RigidBody.velocity.y + (YAccel * Time.deltaTime);

            if (ySpeed > YMaxSpeed)
            {
                ySpeed = YMaxSpeed;
            }
            RigidBody.velocity = new Vector2(RigidBody.velocity.x, ySpeed);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Bullet"))
        {
            Health -= collision.gameObject.GetComponent<Bullet>().Damage;
        }
    }

    //private void SetState()
    //{
    //    _state = _rigidBody.velocity == Vector2.zero ? State.Idle : State.Running;
    //}

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

    //private bool IsBlockedByWall()
    //{
    //    var direction = this.transform.position.x > 0 ? Vector2.left : Vector2.right;
    //    var rayCast = Physics2D.Raycast(_boxCollider.bounds.center, direction, _boxCollider.bounds.extents.y + 0.1f, _layerMask);
    //    return rayCast.collider != null;
    //}
}
