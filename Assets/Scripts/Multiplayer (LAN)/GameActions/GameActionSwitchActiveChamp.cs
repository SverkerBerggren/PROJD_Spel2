using System.Collections;
using System.Collections.Generic;
using System;

public class GameActionSwitchActiveChamp : GameAction
{

    public TargetInfo targetToSwitch = new TargetInfo();
    public bool championDied = false;

    public GameActionSwitchActiveChamp()
    {
        Type = 7;
    }
    public GameActionSwitchActiveChamp(TargetInfo targetToSwitch)
    {
        Type = 7;

        this.targetToSwitch = targetToSwitch;
    }
}
