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
			p = WeightedRandomWalk(rooms_positions ,p);
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

	// unused
	static private Vector2Int RandomMovementNoBacktrack(in HashSet<Vector2Int> rooms_pos, in Vector2Int p)
	{
        List<Vector2Int> p_candidates = new List<Vector2Int>
        {   p + new Vector2Int(-1, 0),
            p + new Vector2Int(1, 0),
            p + new Vector2Int(0, -1),
            p + new Vector2Int(0, 1) };

		List<Vector2Int> tmp = new List<Vector2Int>(rooms_pos.ToList());
        p_candidates = p_candidates.Where(r => !tmp.Contains(r)).ToList();

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
		return p_candidates[Random.Range(0, p_candidates.Count)];
	}

    static private Vector2Int WeightedRandomWalk(in HashSet<Vector2Int> rooms_pos, in Vector2Int p)
    {
        List<Vector2Int> p_candidates = new List<Vector2Int>
        {   p + new Vector2Int(-1, 0),
            p + new Vector2Int(1, 0),
            p + new Vector2Int(0, -1),
            p + new Vector2Int(0, 1) };

        List<Vector2Int> tmp = new List<Vector2Int>(rooms_pos.ToList());
        p_candidates = p_candidates.Where(r => !tmp.Contains(r)).ToList();
		
		List<int> weights = new List<int>();
		int weightsum = 0;

		for(int i = 0; i < p_candidates.Count; i++)
		{
			int w = 4 - CountNeigbours(rooms_pos, p_candidates[i]);		// the weight is the number of empty (non-neighbour) tiles

            weights.Add(w);
			weightsum += w;
		}


        if (p_candidates.Count == 0)
        {       // if there's no candidates (i.e adjacent rooms already added), chose a random direction
            int dir = Random.Range(0, 2);           // 0 for x, 1 for y
            int val = Random.Range(0, 2) * 2 - 1;   // -1 or 1

            if (dir == 0)
            {
                return p + new Vector2Int(val, 0);
            }
            else
            {
                return p + new Vector2Int(0, val);
            }
        }

		// return a random candidate
		int ind_w = Random.Range(0, weightsum);
		weightsum = 0;
        for(int i = 0; i < weights.Count; i++)
		{
            weightsum += weights[i];
			if(ind_w < weightsum)
			{
				return p_candidates[i];
			}
		}

		return p_candidates.Last();
    }

	static private int CountNeigbours(in HashSet<Vector2Int> rooms_pos, in Vector2Int p)
	{
        List<Vector2Int> neigboring_tiles = new List<Vector2Int>
        {   p + new Vector2Int(-1, 0),
            p + new Vector2Int(1, 0),
            p + new Vector2Int(0, -1),
            p + new Vector2Int(0, 1) };

		int count = 0;
		for(int i = 0; i < neigboring_tiles.Count; i++)
		{
			if (rooms_pos.Contains(neigboring_tiles[i]))
			{
				count++;
			}
		}

		return count;
    }
}
