using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientRequestGameSetup : ClientRequest
{
	public bool reciprocate = false;
	public ClientRequestGameSetup()
	{
		Type = 15;
	}
}
