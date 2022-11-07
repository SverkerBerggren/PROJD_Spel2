using System.Collections;
using System.Collections.Generic;
using System;

public class RequestSwitchActiveChamps : ClientRequest
{
    public TargetInfo targetToSwitch = new TargetInfo();
	public bool championDied = false;
	public RequestSwitchActiveChamps()
    {
        Type = 8; 
    }
    public RequestSwitchActiveChamps(TargetInfo targetToSwitch)
    {
        Type = 8;

        this.targetToSwitch = targetToSwitch;
    }
}
