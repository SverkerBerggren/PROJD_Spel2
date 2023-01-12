
using System.Collections.Generic;


public class RequestDamage : ClientRequest
{
    public List<TargetAndAmount> targetsToDamage = new List<TargetAndAmount>();

    public RequestDamage()
    {   
        
        Type = 6; 
    }
    public RequestDamage(List<TargetAndAmount> targetsToDamage)
    {
        this.targetsToDamage = targetsToDamage;
        Type = 6; 
    }

}
