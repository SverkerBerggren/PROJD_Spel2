using System.Collections;
using System.Collections.Generic;


public class RequestLogGameAction : ClientRequest
{
    public int turnPerformed = 0;
    public string gameActionName = "";

    public RequestLogGameAction()
    {
        Type = 23;   
    }
    
}
