using System;
using System.Collections;
using System.Collections.Generic;


public class TargetInfo 
{
    
    public ListEnum whichList = new ListEnum();
    public int index = 0;

    public TargetInfo()
    {
        index = 100;

        whichList = new ListEnum();
    }

    public TargetInfo(ListEnum list, int index)
    {
        whichList = list;
        this.index = index;
    }

}
