using System.Collections;
using System.Collections.Generic;


public class GameActionDestroyLandmark : GameAction
{
    public List<TargetInfo> landmarksToDestroy = new List<TargetInfo>();

    public bool sacrificeLandmark = false;
    public GameActionDestroyLandmark()
    {
        Type = 8; 
    }
    
    public GameActionDestroyLandmark(List<TargetInfo> landmarksToDestroy)
    {
        Type = 8;

        this.landmarksToDestroy = landmarksToDestroy; 
    }
}
