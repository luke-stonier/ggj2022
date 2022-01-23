using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InventoryManager))]
public class Player : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _runSpeed = 5f;
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private float _iframeLengthSeconds = 0.5f;
    [SerializeField] private int _health = 100;
    [SerializeField] private float _xAccel = 5f;
    [SerializeField] private float _yAccel = 5f;
    [SerializeField] private float _decelerationRate = 10f;

    private Rigidbody2D _rigidBody;
    private InventoryManager _inventoryManager;
    private Collider2D _boxCollider;
    private State _state;
    private bool _isHurt;

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
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        SetYVelocityExact();
        SetXVelocityExact();
        //SetYVelocityAccel();
        //SetXVelocityAccel();

    }

    private void SetYVelocityAccel()
    {
        var vDirection = Input.GetAxisRaw("Vertical");

        if (vDirection > 0)
        {
            //current speed - accel*time
            var ySpeed = _rigidBody.velocity.y + (_yAccel * Time.deltaTime);

            //constrain to max speed
            if (ySpeed > _runSpeed)
            {
                ySpeed = _runSpeed;
            }

            //set velocity
            _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, ySpeed);
        }
        //if player is right of enemy
        else if (vDirection < 0)
        {
            //current speed - accel*time
            var ySpeed = _rigidBody.velocity.y - (_yAccel * Time.deltaTime);

            //constrain to max speed
            if (ySpeed < -_runSpeed)
            {
                ySpeed = -_runSpeed;
            }

            //set velocity
            _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, ySpeed);
        }
        else if (vDirection == 0)
        {
            if (Mathf.Abs(_rigidBody.velocity.y) < 0.05)
            {
                //Do Nothing
            }
            if (_rigidBody.velocity.y > 0)
            {
                var ySpeed = _rigidBody.velocity.y - _decelerationRate * Time.deltaTime;
                _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, ySpeed);
            }
            else if (_rigidBody.velocity.y < 0)
            {
                var ySpeed = _rigidBody.velocity.y + _decelerationRate * Time.deltaTime;
                _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, ySpeed);
            }
        }
    }

    private void SetXVelocityAccel()
    {
        var xDirection = Input.GetAxisRaw("Horizontal");

        if (xDirection > 0)
        {
            //current speed - accel*time
            var xSpeed = _rigidBody.velocity.x + (_xAccel * Time.deltaTime);

            //constrain to max speed
            if (xSpeed > _runSpeed)
            {
                xSpeed = _runSpeed;
            }

            //set velocity
            _rigidBody.velocity = new Vector2(xSpeed, _rigidBody.velocity.y);
        }
        //if player is right of enemy
        else if (xDirection < 0)
        {
            //current speed - accel*time
            var xSpeed = _rigidBody.velocity.x - (_xAccel * Time.deltaTime);

            //constrain to max speed
            if (xSpeed < -_runSpeed)
            {
                xSpeed = -_runSpeed;
            }

            //set velocity
            _rigidBody.velocity = new Vector2(xSpeed, _rigidBody.velocity.y);
        }
        else if (xDirection == 0)
        {
            if (Mathf.Abs(_rigidBody.velocity.x) < 0.05)
            {
                //Do Nothing
            }
            if (_rigidBody.velocity.x > 0)
            {
                var xSpeed = _rigidBody.velocity.x - _decelerationRate * Time.deltaTime;
                _rigidBody.velocity = new Vector2(xSpeed, _rigidBody.velocity.y);
            }
            else if (_rigidBody.velocity.x < 0)
            {
                var xSpeed = _rigidBody.velocity.x + _decelerationRate * Time.deltaTime;
                _rigidBody.velocity = new Vector2(xSpeed, _rigidBody.velocity.y);
            }
        }
    }

    private void SetYVelocityExact()
    {
        var vDirection = Input.GetAxisRaw("Vertical");
        if (vDirection < 0)
        {
            _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, -_runSpeed);
            //transform.localScale = new Vector2(-1, 1); //sets sprite direction
        }
        else if (vDirection > 0)
        {
            _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, _runSpeed);
            //transform.localScale = new Vector2(1, 1); //sets sprite direction
        }
        else if (vDirection == 0)
        {
            _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, 0);
        }
    }

    private void SetXVelocityExact()
    {
        var hDirection = Input.GetAxisRaw("Horizontal");
        

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
        else if (hDirection == 0)
        {
            _rigidBody.velocity = new Vector2(0, _rigidBody.velocity.y);
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
    //    var raycastHit = Physics2D.Raycast(_boxCollider.bounds.center, Vector2.down, _boxCollider.bound//s.extents.y// + 0.1f, _layerMask);//
    //    return raycastHit.collider != null; ;
    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleCollision(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        HandleCollision(collision);
    }

    private void HandleCollision(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            HandleEnemyCollision(collision);
        }
    }

    private void HandleEnemyCollision(Collision2D collision)
    {
        TakeDamage(collision.gameObject.GetComponent<Enemy>().DamageDealt);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        HandleCollider(collider);
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        HandleCollider(collider);
    }

    private void HandleCollider(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(collider.gameObject.GetComponent<Enemy>().DamageDealt);
        }
        else if (collider.gameObject.CompareTag("EnemyPlayerCollider"))
        {
            TakeDamage(collider.gameObject.GetComponentInParent<Enemy>().DamageDealt);
        }
    }

    private void TakeDamage(int damage)
    {
        if (_isHurt)
        {
            //currently in iframe
            Debug.Log($"No damage taken - currently in iframes");
            return;
        }

        _isHurt = true;
        _health -= damage;
        StartCoroutine(ResetIFrame());
    }

    private IEnumerator ResetIFrame()
    {
        yield return new WaitForSeconds(_iframeLengthSeconds);
        _isHurt = false;
    }
}
