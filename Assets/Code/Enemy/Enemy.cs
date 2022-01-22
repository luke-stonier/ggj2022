using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int _health;
    [SerializeField] private int _moveSpeed;


    private Rigidbody2D _rigidBody;
    private Collider2D _boxCollider;

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveTowardsPlayer();
    }

    private static void MoveTowardsPlayer()
    {
        
    }
}
