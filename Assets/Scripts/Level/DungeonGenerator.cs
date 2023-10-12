using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
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
		HashSet<Vector2Int> rooms_positions = new HashSet<Vector2Int>();

		// starting point
		Vector2Int p = new Vector2Int(0,0);

		// add the desired number of rooms to the layout
		while (rooms_positions.Count < number_of_rooms)
		{
			rooms_positions.Add(p);

			// Determine probability of each direction
			p = RandomMovementNoBacktrack(rooms_positions ,p);
        }

		List<Room> rooms = new List<Room>();
		foreach (Vector2Int pos in rooms_positions)
		{
			Room r = new Room();
			r.position = pos;
			rooms.Add(r);
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
		List<Vector2Int> p_candidates = new List<Vector2Int> 
		{	new Vector2Int(-1, 0),
			new Vector2Int(1, 0),
			new Vector2Int(0, -1),
			new Vector2Int(0, 1) };

		// return a random candidate
		return p + p_candidates[Random.Range(0,p_candidates.Count)];
	}

	static private Vector2Int RandomMovementNoBacktrack(in HashSet<Vector2Int> rooms_pos, in Vector2Int p)
	{
        List<Vector2Int> p_candidates = new List<Vector2Int>
        {   new Vector2Int(-1, 0),
            new Vector2Int(1, 0),
            new Vector2Int(0, -1),
            new Vector2Int(0, 1) };

        if (p_candidates.Count == 0)
		{       // if there's no candidates (i.e adjacent rooms already added), chose a random direction
			int dir = Random.Range(0, 2);			// 0 for x, 1 for y
			int val = Random.Range(0, 2) * 2 - 1;	// -1 or 1
			
			if(dir == 0)
			{
				return p + new Vector2Int(val, 0);
			}
			else
			{
                return p + new Vector2Int(0, val);
            }
		}
		
		// return a random candidate
		return p + p_candidates[Random.Range(0, p_candidates.Count)];
	}

}
