using System.Collections.Generic;

public class RequestShield : ClientRequest
{


    public List<TargetAndAmount> targetsToShield = new List<TargetAndAmount>(); 
    public RequestShield()
    {
        Type = 7; 
    }

    public RequestShield(List<TargetAndAmount> targetsToShield)
    {


        Type = 7;

        this.targetsToShield = targetsToShield; 


    }
    
    
}
