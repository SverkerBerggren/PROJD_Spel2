
public class RequestEndTurn : ClientRequest
{   
    public RequestEndTurn() 
    {

        Type = 2; 
    } // maste vara kvar

    public RequestEndTurn(int whichPlayer)
    {
        this.whichPlayer = whichPlayer;

        Type = 2; 
    }
}
