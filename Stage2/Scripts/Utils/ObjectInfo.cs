using System;
using UnityEngine;

public class ObjectInfo : IEquatable<ObjectInfo>, IComparable<ObjectInfo>
{
    public float distance {get;}
    public float angle { get; }
    public Vector2 pos { get; }


    public ObjectInfo(float distance, float angle, Vector2 pos)
    {
        this.distance = distance;
        this.angle = angle;
        this.pos = pos;

    }

    public bool Equals(ObjectInfo other)
    {
        throw new NotImplementedException();
    }

    public int CompareTo(ObjectInfo other)
    {
        if (this.distance < other.distance)
        {
            return 1;
        }
        else if (this.distance == other.distance)
        {
            return 0;
        }
        return -1;
    }
}
