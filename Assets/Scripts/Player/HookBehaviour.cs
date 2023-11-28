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

	public void ThrowHook(Vector3 playerpos, Vector3 mouseworldpos)
	{
		linerenderer_instance.enabled = true;
		spriterenderer_instance.enabled = true;

		hook_throw_direction = mouseworldpos - playerpos;

		hook_throw_direction.Normalize();

		RaycastHit2D hit = Physics2D.Raycast(playerpos, hook_throw_direction, hook_range);

		Debug.Log(transform.position);
		Debug.Log(hook_throw_direction);

		Vector3 endpoint;
		if (hit.collider != null)
		{
			endpoint = hit.point;
		}
		else
		{
			endpoint = playerpos + new Vector3(hook_throw_direction.x, hook_throw_direction.y) * hook_range;
		}

		//linerenderer_instance.SetPosition(0, playerpos);
		linerenderer_instance.SetPosition(0, endpoint);
		spriterenderer_instance.transform.position = endpoint;

		StartCoroutine(HookTravel(playerpos, endpoint, (hit.collider != null)));
	}

	IEnumerator HookTravel(Vector3 from, Vector3 to, bool anchor_at_the_end)
	{
		is_moving = true;

		Vector3 p;
		float next_move = 0;
		float move_time = Vector3.Distance(from, to) / hook_projectile_speed;

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

		if(!anchor_at_the_end)
		{
			Disable();
		}
	}

	public void Disable()
	{
		linerenderer_instance.enabled = false;
		spriterenderer_instance.enabled = false;
		anchored = false;
	}
}
