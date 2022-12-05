using System.Collections;
using System.Collections.Generic;

public class TargetAndAmount
{
    public TargetInfo targetInfo;
    public int amount;
   // public Champion championTarget;
  //  public LandmarkDisplay landmarkTarget;
 //   public int damage;
    public TargetAndAmount(TargetInfo targetInfo,int amount)
    {
        this.targetInfo = targetInfo;
        this.amount = amount;
    }
    public TargetAndAmount()
    {
        this.targetInfo = new TargetInfo();
        this.amount = 0;
    }
}
