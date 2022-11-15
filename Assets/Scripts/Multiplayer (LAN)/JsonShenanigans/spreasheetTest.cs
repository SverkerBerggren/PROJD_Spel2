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
using Unity.VisualScripting;



//using HttpClient clienten = new();

//await ProcessRepositoriesAsync(clienten);




public class spreasheetTest : MonoBehaviour
{
    public AttackSpell[] attackSpellsObjectArray;
    public List<AttackSpell> attackSpellsObjectList;
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
           
           print( json);
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

            attackSpellsObjectArray = Resources.LoadAll<AttackSpell>("ScriptableObjects/Cards/Spells/Attacks");
        //    attackSpellsObject = AssetBundle.LoadFromFile("Assets/ScriptableObjects/Cards/Spells/Attacks").GetComponents<AttackSpell>();
            print(attackSpellsObjectArray.Length);
          //  print("attackspells object " + attackSpellsObject[0].cardName);


        }
        catch (Exception ex)
        {
            print(ex.Message.ToString());
        }
        try
        {
            attackSpellsObjectList.AddRange(attackSpellsObjectArray);
            int amountOfCardsChanged = 0;
            for (int i = 0; i < spreadsheetGrej.valueRanges.Count; i++)
            {   
                List<string> currentCard = spreadsheetGrej.valueRanges[0].values[i];
                AttackSpell scriptableObject;
                print("kommer den hit");    
                if ((scriptableObject = findAttackCardFromName(attackSpellsObjectList, currentCard[0])) != null)
                {
                    if (scriptableObject.description.Equals(currentCard[3]))
                    {
                        scriptableObject.description = currentCard[3];
                        amountOfCardsChanged += 1;
                    }

                }

            }
            if (amountOfCardsChanged != 0)
            {
                print(amountOfCardsChanged + " cards was changed");
            }
        }
        catch(Exception ex)
        {
            print(ex.Message.ToString());
        }

    }

    
    private AttackSpell findAttackCardFromName(List<AttackSpell> listToSearch, string name)
    {

        foreach(AttackSpell spell in listToSearch)
        {
            if(spell.cardName == name)
            {
                return spell;
            }
        }


        return null;
    }
}

