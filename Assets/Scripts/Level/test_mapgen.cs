using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_mapgen : MonoBehaviour
{
    [SerializeField] private float scale;
    [SerializeField] private int size;

    // Start is called before the first frame update
    void Start()
    {
        List<Vector2> rooms = DungeonGenerator.GenRandomWalk(size);
        List<GameObject> squares = new List<GameObject>();

        foreach(Vector2 room_coords in rooms)
        {
            GameObject p = GameObject.CreatePrimitive(PrimitiveType.Cube);
           
            p.transform.localScale = scale * Vector3.one;
            p.transform.parent = gameObject.transform;
            p.transform.position = gameObject.transform.position + new Vector3(room_coords.x, room_coords.y, 0) * scale;

            squares.Add(p);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
