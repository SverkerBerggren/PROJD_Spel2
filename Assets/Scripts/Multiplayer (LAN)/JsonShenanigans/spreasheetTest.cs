using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Http.Headers;
using System.Net.Http;
using System;
using System.Threading.Tasks;
using UnityEditor.PackageManager;
using System.Linq;
using Newtonsoft.Json;
using MBJson;
using static googlespreasheetObject;



//using HttpClient clienten = new();

//await ProcessRepositoriesAsync(clienten);




public class spreasheetTest : MonoBehaviour
{
    public AttackSpell[] attackSpellsObject;
    public List<List<string>> attackSpellsData;
    HttpClient clienten = new HttpClient();
    public googlespreasheetObject spreadsheetGrej;
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.J))
        {
            print("hej");
            Task hej = ProcessRepositoriesAsync(clienten);
        }
    }


    public async Task ProcessRepositoriesAsync(HttpClient client)
    {
        print("hej fast metoden innan wait");
        var json = await client.GetStringAsync(
            "https://sheets.googleapis.com/v4/spreadsheets/1qpkI_uNGf4TVIzgs8FyVeXSVlQOkfZR4z9SArJuQJww/values:batchGet?majorDimension=ROWS&ranges=A1:H33&key=AIzaSyCeFExPhC-xWNxyQT7KCBMisahAYTg1I0k");
        //https://sheets.googleapis.com/v4/spreadsheets/1qpkI_uNGf4TVIzgs8FyVeXSVlQOkfZR4z9SArJuQJww/values:batchGet?majorDimension=COLUMNS&ranges=A1&key=AIzaSyCeFExPhC-xWNxyQT7KCBMisahAYTg1I0k
           
        //   print( json);
        spreadsheetGrej = new googlespreasheetObject();
        try
        {

            spreadsheetGrej = JsonConvert.DeserializeObject<googlespreasheetObject>(json);
           

        }
        catch(Exception ex)
        {
            print(ex.Message.ToString());
        }        
       // print("hej fast metoden efter wait " + spreadsheetGrej.spreadsheetId ); 
        

        try
        {

            attackSpellsObject = Resources.LoadAll<AttackSpell>("ScriptableObjects/Cards/Spells/Attacks");  
            print("attackspells object " + attackSpellsObject[0].cardName);


        }
        catch (Exception ex)
        {
            print(ex.Message.ToString());
        }

        for(int i = 0; i < spreadsheetGrej.valueRanges.Count; i++)
        {
            List<string> currentCard = spreadsheetGrej.valueRanges[0].values[i];

            for(int z = 0; z < currentCard.Count; z++)
            {
             //   if()
            }
            
        }
    }

}

