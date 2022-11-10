using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestPassPriority : ClientRequest
{
    public bool priority;

    public RequestPassPriority()
    {
        Type = 16;
    }

    public RequestPassPriority(bool priority)
    {
        Type = 16;
        this.priority = priority;   
    }
}
