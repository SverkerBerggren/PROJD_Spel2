using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponsePassPriority : ServerResponse
{
    public bool priority;
    public ResponsePassPriority()
    {
        Type = 14;
    }
    public ResponsePassPriority(bool priority)
    {
        Type = 14;

        this.priority = priority;
    }
}