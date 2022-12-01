using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponseStopSwapping : ServerResponse
{
    public bool canSwap;

    public ResponseStopSwapping()
    {
        Type = 19;
    }
    public ResponseStopSwapping(bool canSwap)
    {
        Type = 19;

        this.canSwap = canSwap;
    }
}
