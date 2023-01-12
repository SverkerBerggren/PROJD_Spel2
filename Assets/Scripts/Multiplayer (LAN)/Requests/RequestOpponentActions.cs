

public class RequestOpponentActions : ClientRequest
{
    public RequestOpponentActions() { }
    public bool requestOpponentActions = false;

    public RequestOpponentActions(int whichPlayer, bool requestGameActions)
    {
        this.whichPlayer = whichPlayer;
        requestOpponentActions = true;

        Type = 1;
    }
}
