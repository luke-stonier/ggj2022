using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _runSpeed = 5f;
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private float _iframeLengthSeconds = 0.5f;
    [SerializeField] private int _health = 100;

    private Rigidbody2D _rigidBody;
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
    void Update()
    {
        Movement();
    }

    private void Movement()
    {
        SetXVelocity();
        SetYVelocity();
    }

    private void SetYVelocity()
    {
        var vDirection = Input.GetAxis("Vertical");
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
    }

    private void SetXVelocity()
    {
        var hDirection = Input.GetAxis("Horizontal");
        

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
        if (collision.gameObject.CompareTag("Enemy"))
        {
            HandleEnemyCollision(collision);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
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

    private void HandleEnemyCollision(Collision2D collision)
    {
        TakeDamage(collision.gameObject.GetComponent<Enemy>().DamageDealt);
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
