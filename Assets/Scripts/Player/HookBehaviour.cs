using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookBehaviour : MonoBehaviour
{
    [SerializeField] private float hook_projectile_speed;
    [SerializeField] private Sprite rope_sprite;
    [SerializeField] private Sprite hook_sprite;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 p1 = gameObject.GetComponent<LineRenderer>().GetPosition(0);
        Vector2 p2 = gameObject.GetComponent<LineRenderer>().GetPosition(1);

        float angle = Mathf.Atan2(p1.y - p2.y, p1.x - p2.x);

        Debug.Log(angle);

        gameObject.GetComponent<SpriteRenderer>().transform.rotation = Quaternion.Euler(0,0,180*angle / Mathf.PI);
    }
}
