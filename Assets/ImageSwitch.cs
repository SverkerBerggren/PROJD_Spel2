using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageSwitch : MonoBehaviour
{
    public GameObject nextButton;
    public GameObject prevButton;
    public GameObject panel;
    public GameObject[] background;
    public int pages;

    private int index;
    private bool isActive = false;

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
            prevButton.SetActive(false);
        }
        else
        {
            prevButton.SetActive(true);
        }
        if (index == pages)
        {
            //background[pages].gameObject.SetActive(true);
            nextButton.SetActive(false);
        } 
        else
        {
            nextButton.SetActive(true);
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

    public void Close()
    {
        if(panel != null)
        {
            isActive = panel.activeSelf;
            background[index].gameObject.SetActive(false);
            panel.SetActive(!isActive);
            ResetOrder();
        }
    }

    public void ResetOrder()
    {
        index = 0;
        background[0].gameObject.SetActive(true);
    }
}
