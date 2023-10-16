using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    private List<Room> rooms;

    // Start is called before the first frame update
    void Start()
    {
        rooms = DungeonGenerator.GenRandomWalk(12);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
