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
     * List<Vector2> : the coordinates of every room in the layout (no duplicates)
     */
    public static List<Vector2> GenRandomWalk(int number_of_rooms)
    {
        // layout is represented using the coordinates of the rooms composing it.
        HashSet<Vector2> rooms = new HashSet<Vector2>();

        Vector2 current_space = new Vector2(0, 0);       // initial room, to expand from

        while(rooms.Count() < number_of_rooms)
        {
            rooms.Add(current_space);    // only added if different from other elements

            // Determine probability of each direction


            // randomly walk to a neighboring space
            int dir = Random.Range(0, 2);               // axis selection
            int sign = Random.Range(0, 2) * 2 - 1;      // direction along the axis (-1 or 1)
            if(dir == 0 )
            {
                current_space += new Vector2(sign, 0);
            }
            else
            {
                current_space += new Vector2(0, sign);
            }
        }

        return rooms.ToList();      // returns an ordered list
    }
}
