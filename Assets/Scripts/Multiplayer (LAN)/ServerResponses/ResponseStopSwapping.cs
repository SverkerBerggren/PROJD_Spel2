using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponseStopSwapping : ServerResponse
{
    public bool canSwap;

    public ResponseStopSwapping()
    {
        Type = 15;
    }
    public ResponseStopSwapping(bool canSwap)
    {
        Type = 15;

        this.canSwap = canSwap;
    }
}
