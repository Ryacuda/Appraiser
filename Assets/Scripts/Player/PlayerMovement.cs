using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	private Rigidbody2D instance_rigidbody;
	private BoxCollider2D instance_collider;
	private float horizontal_direction;     // -1 or 1 along x
	private Vector2 hook_throw_direction;
	private bool hook = false;
	private float timestamp_jump;
	private bool grounded;
	private bool sliding_right;
	private bool sliding_left;
	private uint jump_charges;
	private Camera camera_instance;

	[SerializeField] private float jump_input_buffer;
	[SerializeField] private LayerMask jumpable_ground;
	[SerializeField] private float max_speed;
	[SerializeField] private float jump_speed;
	[SerializeField] private float hook_range;

	// Start is called before the first frame update
	private void Start()
	{
		instance_rigidbody = GetComponent<Rigidbody2D>();
		instance_collider = GetComponent<BoxCollider2D>();
		camera_instance = gameObject.GetComponentInChildren<Camera>();

		timestamp_jump = -100;

		horizontal_direction = 0;
	}

	// Update is called once per frame
	private void Update()
	{
		horizontal_direction = Input.GetAxisRaw("Horizontal");

		if(Input.GetButtonDown("Jump"))
		{
			timestamp_jump = Time.time;
		}

		if(Input.GetKeyDown(KeyCode.E))
		{
			hook_throw_direction = camera_instance.ScreenToWorldPoint(Input.mousePosition) - transform.position;

			hook_throw_direction.Normalize();

			hook = true;
		}
	}

	private void FixedUpdate()
	{
		UpdateGrounded();

		Movement();
		Jump();
		ThrowHook();
	}

	private void Movement()
	{
		if(grounded)
		{
			instance_rigidbody.velocity = new Vector2(max_speed * horizontal_direction, instance_rigidbody.velocity.y);
		}
		else
		{
			float new_x_speed = Mathf.Clamp(instance_rigidbody.velocity.x + horizontal_direction * max_speed / 5, -max_speed, max_speed);

			instance_rigidbody.velocity = new Vector2(new_x_speed, instance_rigidbody.velocity.y);
		}
	}

	private void Jump()
	{
		if(Time.time - timestamp_jump < jump_input_buffer)
		{
			timestamp_jump = -100;  // consume input in order to prevent jumping twice in some cases

			if (jump_charges > 0)
			{
				instance_rigidbody.velocity = new Vector2(instance_rigidbody.velocity.x, jump_speed);
				jump_charges--;
			}
			else if (sliding_left && sliding_right)
			{
				instance_rigidbody.velocity = new Vector2(instance_rigidbody.velocity.x, jump_speed);
			}
			else if(sliding_right)
			{
				instance_rigidbody.velocity = new Vector2(-max_speed, jump_speed);
			}
			else if(sliding_left)
			{
				instance_rigidbody.velocity = new Vector2(max_speed, jump_speed);
			}
		}
	}

	private void ThrowHook()
	{
		if(hook)
		{
			RaycastHit2D hit = Physics2D.Raycast(transform.position, hook_throw_direction, hook_range);

			if (hit.collider != null)
			{
				Debug.DrawLine(transform.position, hit.point);
			}
			else
			{
				Debug.DrawLine(transform.position, transform.position + new Vector3(hook_throw_direction.x, hook_throw_direction.y) * hook_range);
			}
		}

		// consume input
		hook = false;
	}

	private void UpdateGrounded()
	{
		grounded = Physics2D.BoxCast(instance_collider.bounds.center, instance_collider.bounds.size, 0, Vector2.down, 0.08f, jumpable_ground);
		if(grounded)
		{
			jump_charges = 2;
		}

		sliding_right = Physics2D.BoxCast(instance_collider.bounds.center, instance_collider.bounds.size, 0, Vector2.right, 0.08f, jumpable_ground);
		sliding_left = Physics2D.BoxCast(instance_collider.bounds.center, instance_collider.bounds.size, 0, Vector2.left, 0.08f, jumpable_ground);
	}
}
