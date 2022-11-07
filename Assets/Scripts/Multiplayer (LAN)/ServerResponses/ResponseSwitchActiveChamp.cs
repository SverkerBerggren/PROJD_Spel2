using System.Collections;
using System.Collections.Generic;
using System;

public class ResponseSwitchActiveChamp : ServerResponse
{

    public TargetInfo targetToSwitch = new TargetInfo();
	public bool championDied = false;
	public ResponseSwitchActiveChamp()
    {
        Type = 7;
    }
    public ResponseSwitchActiveChamp(TargetInfo targetToSwitch)
    {
        Type = 7;

        this.targetToSwitch = targetToSwitch;
    }
}
