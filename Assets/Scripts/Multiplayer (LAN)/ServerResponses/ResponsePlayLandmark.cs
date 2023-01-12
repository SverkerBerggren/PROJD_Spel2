
public class ResponsePlayLandmark : ServerResponse
{	
	public CardAndPlacement landmarkToPlace = new CardAndPlacement();

	public ResponsePlayLandmark()
	{
		Type = 13; 
	}

	public ResponsePlayLandmark(CardAndPlacement landmarkToPlace)
	{
		Type = 13;

		this.landmarkToPlace = landmarkToPlace; 
	}
}
