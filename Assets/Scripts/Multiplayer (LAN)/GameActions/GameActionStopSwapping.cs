using System.Collections;
using System.Collections.Generic;

public class GameActionStopSwapping : GameAction
{

    public bool canSwap = true;

    public GameActionStopSwapping()
    {
        Type = 16;
    }
    public GameActionStopSwapping(bool canSwap)
    {
        Type = 16;
        this.canSwap = canSwap;
    }
}
