
using System.Collections.Generic;

public class GameActionHeal : GameAction
{

    public  List<TargetAndAmount> targetsToHeal = new List<TargetAndAmount>(); 

    public GameActionHeal() 
    {
        Type = 3; 
    } 

    public GameActionHeal(List<TargetAndAmount> targetsToHeal)
    {
        this.targetsToHeal = targetsToHeal;

        Type = 3; 
    }
}
