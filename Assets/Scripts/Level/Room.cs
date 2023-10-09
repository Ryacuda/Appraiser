using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum Openings : byte
{
    Left    = 0b_0000_0000,
    Right   = 0b_0000_0001,
    Top     = 0b_0000_0010,
    Bottom  = 0b_0000_0100
}

public class Room
{
    

    // Attributes
    public Vector2 position;
    public Openings openings;

}
