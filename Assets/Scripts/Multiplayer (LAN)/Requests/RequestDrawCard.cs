
public class RequestDrawCard : ClientRequest
{
    public int amountToDraw = 0; 

    public RequestDrawCard() { }//maste existera

    public RequestDrawCard(int amountToDraw)
    {
        this.amountToDraw = amountToDraw;

        Type = 3; 
    }


}
