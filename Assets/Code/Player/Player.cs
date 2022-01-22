using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _runSpeed = 5f;
    [SerializeField] private float _jumpForce = 5f;

    [SerializeField] private int _health = 100;

    private Rigidbody2D _rigidBody;
    private Collider2D _boxCollider;
    private State _state;

    private enum State
    {
        Idle = 0,
        Running = 1,
        Jumping = 2,
        Falling = 3,
        Hurt = 4
    }

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    private void Movement()
    {
        var hDirection = Input.GetAxis("Horizontal");
        var vDirection = Input.GetAxis("Vertical");

        if (hDirection < 0)
        {
            _rigidBody.velocity = new Vector2(-_runSpeed, _rigidBody.velocity.y);
            //transform.localScale = new Vector2(-1, 1); //sets sprite direction
        }
        else if (hDirection > 0)
        {
            _rigidBody.velocity = new Vector2(_runSpeed, _rigidBody.velocity.y);
            //transform.localScale = new Vector2(1, 1); //sets sprite direction
        }

        if (IsGrounded() && (vDirection > 0 || Input.GetButtonDown("Jump")))
        {
            Jump();
        }
    }

    private void SetState()
    {
        if (!IsGrounded())
        {
            _state = _rigidBody.velocity.y > 0 ? State.Jumping : State.Falling;
        }
        else
        {
            _state = _rigidBody.velocity == Vector2.zero ? State.Idle : State.Running;
        }
    }

    private void Jump()
    {
        _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, _jumpForce);
        _state = State.Jumping;
    }

    private bool IsGrounded()
    {
        var raycastHit = Physics2D.Raycast(_boxCollider.bounds.center, Vector2.down, _boxCollider.bounds.extents.y + 0.1f, _layerMask);
        return raycastHit.collider != null; ;
    }
}
