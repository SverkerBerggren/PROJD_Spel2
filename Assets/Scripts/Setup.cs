using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setup : MonoBehaviour
{

    private static Setup instance;

    public List<string> hostChamps = new List<string>();
    public List<string> clientChamps = new List<string>();

    public static Setup Instance { get { return instance; } set { instance = value; } }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

}
