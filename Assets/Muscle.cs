using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Muscle 
{
    public float minLength;
    public float maxLength;
    public float length;

    public Muscle(float minLength, float maxLength)
    {
        this.minLength = minLength;
        this.maxLength = maxLength;
    }

    public void Contract()
    {
        length = length * 0.95f + minLength * 0.05f;
    }

    public void Relax()
    {
        length = length * 0.98f + maxLength * 0.02f;
    }
}
