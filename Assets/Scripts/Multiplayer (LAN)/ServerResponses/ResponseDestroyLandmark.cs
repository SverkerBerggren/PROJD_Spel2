using System.Collections;
using System.Collections.Generic;
using System;

public class ResponseDestroyLandmark : ServerResponse
{
    public List<TargetInfo> landmarksToDestroy = new List<TargetInfo>();
	public bool sacrificeLandmark = false;
	public ResponseDestroyLandmark()
    {
        Type = 8;
    }
    
    public ResponseDestroyLandmark(List<TargetInfo> landmarksToDestroy)
    {
        Type = 8;
        this.landmarksToDestroy = landmarksToDestroy; 
    }


}
