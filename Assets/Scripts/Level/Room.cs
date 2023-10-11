using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Room
{
    [Flags]
    public enum Openings : byte
    {
        None    = 0b_0000_0000,
        Left    = 0b_0000_0001,
        Right   = 0b_0000_0010,
        Top     = 0b_0000_0100,
        Bottom  = 0b_0000_1000
    }

    // Attributes
    public Vector2Int position;
    public Openings openings;

    // Methods
    public Openings isNeighbour(in Room other)
    {
        Vector2Int dp = position - other.position;

        return computeOpenings(dp);
    }

    private Openings computeOpenings(Vector2Int relative_position)
    {
        Openings op = Openings.None;

        if(Mathf.Abs(relative_position.x) + Mathf.Abs(relative_position.y) != 1)
        {
            return op;
        }

        if(relative_position.x == -1)
        {
            op = op | Openings.Left;
        }
        else if(relative_position.x == 1)
        {
            op = op | Openings.Right;
        }
        else if (relative_position.y == -1)
        {
            op = op | Openings.Bottom;
        }
        else if (relative_position.y == 1)
        {
            op = op | Openings.Top;
        }

        return op;
    }
}
