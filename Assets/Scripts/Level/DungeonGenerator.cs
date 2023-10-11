using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

/*
 * Dungeon generator class.
 * Used to procedurally generate dungeons (levels) layouts.
 */
public class DungeonGenerator
{
    // Attributes

    // Constructors
    public DungeonGenerator()
    {
    }

    // Accessors

    // Methods

    /*
     * Procedural generation of a layout using a random walk algorithm.
     * Each unique visited space is added to the layout.
     * Allowed movements are up, down, left and right, ensuring that every room in the layout is connected to the whole.
     * 
     * Parameters:
     * int number_of_rooms : the number of rooms desired in the layout
     * 
     * Returns:
     * List<Room> : a list of Room
     */
    public static List<Room> GenRandomWalk(int number_of_rooms)
    {
        // room list, openings updated later
        HashSet<Room> rooms = new HashSet<Room>();

        // starting point
        Vector2Int p = new Vector2Int(0,0);

        // add the desired number of rooms to the layout
        while (rooms.Count < number_of_rooms)
        {
            Room r = new Room();
            r.position = p;
            rooms.Add(r);

            // Determine probability of each direction
            p = RandomMovement(p);
        }

        // update openings
        foreach(Room r1 in rooms) 
        {
            foreach (Room r2 in rooms)
            {
                if(r1.position != r2.position)
                {
                    r1.openings = r1.openings | r1.isNeighbour(r2);
                }
            }
        }

        return rooms.ToList();      // returns an ordered list
    }


    static private Vector2Int RandomMovement(in Vector2Int p)
    {
        List<Vector2Int> p_candidates = new List<Vector2Int>();

        for(int dx = -1; dx <= 1; dx++)            // {-1, 1}
        {
            for (int dy = -1; dy <= 1; dy++)       // {-1, 1}
            {
                 p_candidates.Add(p + new Vector2Int(dx, dy));
            }
        }

        return p_candidates[Random.Range(0,p_candidates.Count)];
    }

}
