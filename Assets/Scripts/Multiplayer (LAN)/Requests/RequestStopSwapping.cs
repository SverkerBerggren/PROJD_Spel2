using System.Collections;
using System.Collections.Generic;


public class RequestStopSwapping : ClientRequest
{
    public bool canSwap;

    public RequestStopSwapping()
    {

        Type = 21;
    }
    public RequestStopSwapping(bool canSwap)
    {
        this.canSwap = canSwap;
        Type = 21;
    }

}
