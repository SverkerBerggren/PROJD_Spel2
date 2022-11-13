using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Http.Headers;
using System.Net.Http;
using System;
using System.Threading.Tasks;
using UnityEditor.PackageManager;
using System.Linq;

//using HttpClient clienten = new();

//await ProcessRepositoriesAsync(clienten);




public class spreasheetTest : MonoBehaviour
{
    HttpClient clienten = new HttpClient();
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.J))
        {
            print("hej");
            Task hej = ProcessRepositoriesAsync(clienten);
        }
    }

    static async Task ProcessRepositoriesAsync(HttpClient client)
    {
        print("hej fast metoden innan wait");
        var json = await client.GetStringAsync(
            "https://sheets.googleapis.com/v4/spreadsheets/1qpkI_uNGf4TVIzgs8FyVeXSVlQOkfZR4z9SArJuQJww?key=AIzaSyCeFExPhC-xWNxyQT7KCBMisahAYTg1I0k ");

        print(json);
        print( json.Count());

        print("hej fast metoden efter wait");

    }
}

