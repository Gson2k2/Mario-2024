using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    
    [field: Range(0,20)]
    public float speed {  get; private set; }
    [field: Range(0,20)]

    public float jumpForce {  get; private set; }
    
    private bool isJumping;
    private Rigidbody2D rb;

    private Animator _animator;

    private bool OneTimeDestroy;
    private bool isFinish;

    private void Awake()
    {
        if (MenuController.Instance == null)
        {
            Instantiate(Resources.Load<GameObject>("Canvas - GamePlay"));
        }
    }

    private void Start()
    {
        speed = 5.0f;
        jumpForce = 135.0f;
        rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private Vector3 velocity;

    private void Update()
    {
        Camera.main.transform.position =
            Vector3.SmoothDamp(new Vector3(Camera.main.transform.position.x, 0f,-10f),
                new Vector3(transform.position.x, 0f,-10f), ref velocity, 0.1f);

        var moveHorizontal = Input.GetAxis("Horizontal");
        var movement = new Vector2(moveHorizontal, 0);

        _animator.SetFloat("Horizontal",moveHorizontal);

        if (transform.position.x >= MenuController.Instance.RightPos && !isFinish)
        {
            isFinish = true;
            MenuController.Instance.OnCompleteUI();
            this.enabled = false;
        }
        if (moveHorizontal > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        if (moveHorizontal < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }

        rb.velocity = new Vector2(movement.x * speed, rb.velocity.y);
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            rb.AddForce(new Vector2(moveHorizontal, jumpForce), ForceMode2D.Impulse);
            isJumping = true;
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
            OneTimeDestroy = false;
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Dead"))
        {
            gameObject.SetActive(false);
            MenuController.Instance.OnGameOverUI();
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Item")
            && transform.position.y >= collision.transform.position.y + 1)
        {
            isJumping = false;
            OneTimeDestroy = false;
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Item")
            && transform.localPosition.y <= collision.transform.localPosition.y - 1)
        {
            if(OneTimeDestroy)return;
            OneTimeDestroy = true;
            Debug.Log("test");
            collision.gameObject.GetComponent<Item>().OnObjectTrigger();
        }
    }
}
