
using System.Collections.Generic;


public class ResponseHeal : ServerResponse
{
    public List<TargetAndAmount> targetsToHeal =  new List<TargetAndAmount>(); 
    public ResponseHeal()
    {
        Type = 4; 
    }

    public ResponseHeal(List<TargetAndAmount> targetsToHeal)
    {
        Type = 4;
        this.targetsToHeal = targetsToHeal;
    }
}
