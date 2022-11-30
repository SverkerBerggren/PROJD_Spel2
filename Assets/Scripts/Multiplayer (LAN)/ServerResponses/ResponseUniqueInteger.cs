using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponseUniqueInteger : ServerResponse
{
    public int UniqueInteger = 0; 
    public ResponseUniqueInteger()
    {
        Type = 18;
    }
}
