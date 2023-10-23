using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;

    [SerializeField] private LayerMask jumpable_ground;

    [SerializeField] private float speed;
    [SerializeField] private float jump_speed;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        speed = 7;
        jump_speed = 17;
    }

    // Update is called once per frame
    private void Update()
    {
        // left - right
        float dir_x = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dir_x * speed, rb.velocity.y);

        // jump
        if (Input.GetButtonDown("Jump"))
        {
            if(isGrounded())
            {
                rb.velocity = new Vector2(rb.velocity.x, jump_speed);
                //Debug.Log("Grounded !");
            }
            else if(isSlidingLeft())
            {
                rb.velocity = new Vector2(jump_speed, jump_speed);
                //Debug.Log("Sliding Left !");
            }
            else if (isSlidingRight())
            {
                rb.velocity = new Vector2(-jump_speed, jump_speed);
                //Debug.Log("Sliding Right !");
            }
        }
    }

    private bool isGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0, Vector2.down, 0.01f, jumpable_ground);
    }

    private bool isSlidingRight()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0, Vector2.right, 0.01f, jumpable_ground);
    }

    private bool isSlidingLeft()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0, Vector2.left, 0.01f, jumpable_ground);
    }
}
