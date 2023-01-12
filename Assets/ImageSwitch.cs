using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageSwitch : MonoBehaviour
{

    public GameObject[] background;
    public int pages;
    int index;

    void Start()
    {
        index = 0;
    }


    void Update()
    {
        if (index >= pages)
            index = pages;

        if (index < 0)
            index = 0;



        if (index == 0)
        {
            background[0].gameObject.SetActive(true);
        }

        if (Input.GetMouseButtonDown(0))
        {
            Next();
        }

        if (Input.GetMouseButtonDown(1))
        {
            Previous();
        }

        if (Input.GetMouseButtonDown(2))
        {
            gameObject.SetActive(false);
        }
    }

    public void Next()
    {
        index += 1;

        for (int i = 0; i < background.Length; i++)
        {
            background[i].gameObject.SetActive(false);
            background[index].gameObject.SetActive(true);
        }
        Debug.Log(index);
    }

    public void Previous()
    {
        index -= 1;

        for (int i = 0; i < background.Length; i++)
        {
            background[i].gameObject.SetActive(false);
            background[index].gameObject.SetActive(true);
        }
        Debug.Log(index);
    }


}
