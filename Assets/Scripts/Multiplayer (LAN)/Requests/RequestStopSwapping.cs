using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestStopSwapping : ClientRequest
{
    public bool canSwap;

    public RequestStopSwapping()
    {

        Type = 17;
    }
    public RequestStopSwapping(bool canSwap)
    {
        this.canSwap = canSwap;
        Type = 17;
    }

}
