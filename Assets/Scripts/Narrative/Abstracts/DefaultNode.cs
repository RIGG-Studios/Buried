using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultNode : MonoBehaviour
{
    FlagObjects flag;

    public void OnEnter()
    {
        Debug.Log("Entered");
    }

    public void OnExit()
    {
        Debug.Log("Left");
    }

    public void OnUpdate()
    {
        return;
    }

    public FlagObjects GetFlag => flag;
    
}
