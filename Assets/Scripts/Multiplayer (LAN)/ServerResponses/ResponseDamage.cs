using System.Collections.Generic;
public class ResponseDamage : ServerResponse
{
    public List<TargetAndAmount> targetsToDamage = new List<TargetAndAmount>();


    public ResponseDamage()
    {
        Type = 5; 
    }
    public ResponseDamage(List<TargetAndAmount> targetsToDamage)
    {
        Type = 5;

        this.targetsToDamage = targetsToDamage; 
    }
}
