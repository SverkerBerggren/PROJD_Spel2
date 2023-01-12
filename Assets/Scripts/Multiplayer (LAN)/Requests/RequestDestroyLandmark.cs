
using System.Collections.Generic;


public class RequestDestroyLandmark : ClientRequest
{
    public List<TargetInfo> landmarksToDestroy = new List<TargetInfo>();
	public bool sacrificeLandmark = false;
	public RequestDestroyLandmark()
    {
        Type = 9;
    }    
    public RequestDestroyLandmark(List<TargetInfo> landmarksToDestroy)
    {
        Type = 9;

        this.landmarksToDestroy = landmarksToDestroy;
    }
}
