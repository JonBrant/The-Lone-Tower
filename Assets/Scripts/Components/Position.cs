using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Position
{
    public float x;
    public float y;
    
    public static implicit operator Vector2(Position position)
    {
        return new Vector2(position.x, position.y);
    }

    public static implicit operator Position(Vector2 position)
    {
        Position returnValue = new Position();
        returnValue.x = position.x;
        returnValue.y = position.y;
        return returnValue;
    }
}
