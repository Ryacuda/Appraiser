using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class HookBehaviour : MonoBehaviour
{
    [SerializeField] private float hook_projectile_speed;
    [SerializeField] private float hook_range;

    [SerializeField] private Sprite rope_sprite;
    [SerializeField] private Sprite hook_sprite;

	private LineRenderer linerenderer_instance;
	private SpriteRenderer spriterenderer_instance;
	private Vector2 hook_throw_direction;

	public bool anchored;
	private float rope_length;
	private bool is_moving;

    // Start is called before the first frame update
    void Start()
    {
		linerenderer_instance = GetComponent<LineRenderer>();
		spriterenderer_instance = GetComponent<SpriteRenderer>();

		anchored = false;
		is_moving = false;

		Disable();
    }

    // Update is called once per frame
    void Update()
    {
		// update rope position
		linerenderer_instance.SetPosition(1, GameObject.Find("Player").transform.position);

		// update hook part
        Vector2 p1 = linerenderer_instance.GetPosition(0);
        Vector2 p2 = linerenderer_instance.GetPosition(1);

        float angle = Mathf.Atan2(p2.y - p1.y, p2.x - p1.x) + Mathf.PI;

        spriterenderer_instance.transform.rotation = Quaternion.Euler(0,0,180*angle / Mathf.PI);
    }

	// Method called to throw the hook toward the desired direction
	public void ThrowHook(Vector3 playerpos, Vector3 mouseworldpos)
	{
		// display the hook
		linerenderer_instance.enabled = true;
		spriterenderer_instance.enabled = true;

		// compute the direction
		hook_throw_direction = mouseworldpos - playerpos;

		hook_throw_direction.Normalize();

		// assess if a hit is registered
		RaycastHit2D hit = Physics2D.Raycast(playerpos, hook_throw_direction, hook_range);

		Vector3 endpoint;
		if (hit.collider != null)
		{
			endpoint = hit.point;
		}
		else
		{
			endpoint = playerpos + new Vector3(hook_throw_direction.x, hook_throw_direction.y) * hook_range;
		}

		// start the hook projectile travel
		StartCoroutine(HookTravel(playerpos, endpoint, (hit.collider != null)));
	}

	// moves the hook from a point to another, and whether the hook anchors at the end
	IEnumerator HookTravel(Vector3 from, Vector3 to, bool anchor_at_the_end)
	{
		is_moving = true;

		Vector3 p;
		float next_move = 0;
		float move_time = Vector3.Distance(from, to) / hook_projectile_speed;

		// linear interpolation
		while (next_move < move_time)
		{
			p = Vector3.Lerp(from, to, next_move / move_time);
			next_move += Time.deltaTime;

			linerenderer_instance.SetPosition(0, p);
			spriterenderer_instance.transform.position = p;

			yield return null;
		}

		linerenderer_instance.SetPosition(0, to);
		spriterenderer_instance.transform.position = to;

		is_moving = false;
		anchored = anchor_at_the_end;

		// if the hook doesn't anchor, it disappears
		if(!anchor_at_the_end)
		{
			Disable();
		}
	}

	// makes the hook disappear
	public void Disable()
	{
		linerenderer_instance.enabled = false;
		spriterenderer_instance.enabled = false;
		anchored = false;
	}


	// the movement that the hook gives to the player
	public Vector2 GetNewSpeed(Vector2 speed, Vector2 playerpos)
	{
		if( Vector2.Distance(playerpos + speed, linerenderer_instance.GetPosition(0)) < Vector2.Distance(linerenderer_instance.GetPosition(0), linerenderer_instance.GetPosition(1)) )
		{
			return speed;
		}

		Vector2 dir = Vector2.Perpendicular(new Vector2(linerenderer_instance.GetPosition(0).x, linerenderer_instance.GetPosition(0).y) - playerpos).normalized;

		if(Vector2.Dot(dir, speed)  < 0)
		{
			return - speed.magnitude * dir;
		}
		else
		{
			return speed.magnitude * dir;
		}
	}
}
