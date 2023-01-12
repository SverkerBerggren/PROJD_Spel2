
public class RequestPlayLandmark : ClientRequest
{
	public CardAndPlacement landmarkToPlace = new CardAndPlacement();

	public RequestPlayLandmark()
	{
		Type = 14; 
	}
	public RequestPlayLandmark(CardAndPlacement landmarkToPlace)
	{
		Type = 14;
		this.landmarkToPlace = landmarkToPlace; 
	}
}
