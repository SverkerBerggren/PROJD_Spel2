using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 
public class RequestHealing : ClientRequest
{
    public List<TargetAndAmount> targetsToHeal = new List<TargetAndAmount>(); 
    
    // Start is called before the first frame update
    public RequestHealing() 
    {
        Type = 4;
    }// ta ej bortr
    public RequestHealing(List<TargetAndAmount> targetsToHeal)
    {
        this.targetsToHeal = targetsToHeal; 
        Type = 4; 
    }
}
