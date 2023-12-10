using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClueScript : MonoBehaviour
{
	private Vector3 starting_position;
	[SerializeField] float floating_loop_rate;		// loop/sec
	[SerializeField] float floating_amplitude;

	// Start is called before the first frame update
	void Start()
	{
		starting_position = transform.position;
	}

	// Update is called once per frame
	void Update()
	{
		transform.position = starting_position + new Vector3(0, floating_amplitude * Mathf.Cos(floating_loop_rate * Time.time), 0);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		// gets destroyed on contact
		if(collision.CompareTag("Player"))
		{
			Destroy(gameObject);
		}
	}
}
