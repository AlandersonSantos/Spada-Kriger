using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    
    private float movement;
    private Rigidbody2D rb;

    [SerializeField] [Range(1,10)] private float velocidade = 5.0f;

    //[SerializeField] private Transform peNoChao;

    //[SerializeField] private LayerMask ChaoLayer;

    //private bool estaNoChao;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        movement = Input.GetAxis("Horizontal");


        if (Input.GetKey(KeyCode.Space)) //&& estaNoChao)
        {
            rb.AddForce(Vector2.up * 5);
        }

        //estaNoChao = Physics2D.OverlapCircle(peNoChao.position, 0.2f, ChaoLayer);

    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(movement * velocidade, rb.linearVelocity.y);
    }

}
