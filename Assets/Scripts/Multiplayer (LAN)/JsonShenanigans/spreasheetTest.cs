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
using UnityEditor;
using System.Transactions;
using System.IO;



//using HttpClient clienten = new();

//await ProcessRepositoriesAsync(clienten);




public class spreasheetTest : EditorWindow
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
            
            Task hej = ProcessRepositoriesAsync(clienten);
        }
    }

    [MenuItem("Tools/spreadsheet updater")]
    public static void ShowWindow()
    {
        GetWindow(typeof(spreasheetTest));
    }

    private void OnGUI()
    {
        GUILayout.Label("Update from spreadsheet",EditorStyles.boldLabel);

        if(GUILayout.Button("Update cards"))
        {
            Task theTask =  ProcessRepositoriesAsync(clienten);
        }
    }
    

    public async Task ProcessRepositoriesAsync(HttpClient client)
    {
        var jsonHowBigSpreadsheet = await client.GetStringAsync(
            "https://sheets.googleapis.com/v4/spreadsheets/1qpkI_uNGf4TVIzgs8FyVeXSVlQOkfZR4z9SArJuQJww/values:batchGet?majorDimension=ROWS&ranges=K2&key=AIzaSyCeFExPhC-xWNxyQT7KCBMisahAYTg1I0k");

        string stringToAppend = ""; 
        try
        {
            googlespreasheetObject parseJsonHowBigSpreadsheet = JsonConvert.DeserializeObject<googlespreasheetObject>(jsonHowBigSpreadsheet);

            stringToAppend = parseJsonHowBigSpreadsheet.valueRanges[0].values[0][0].ToString();
            Debug.Log(stringToAppend);
            Debug.Log(jsonHowBigSpreadsheet);
        }
        catch(Exception ex)
        {
            Debug.Log(ex.Message.ToString());

            
        }
        string key = "&key=AIzaSyCeFExPhC-xWNxyQT7KCBMisahAYTg1I0k";

        var json = await client.GetStringAsync(
            "https://sheets.googleapis.com/v4/spreadsheets/1qpkI_uNGf4TVIzgs8FyVeXSVlQOkfZR4z9SArJuQJww/values:batchGet?majorDimension=ROWS&ranges=A2:H" + stringToAppend + key);
        //https://sheets.googleapis.com/v4/spreadsheets/1qpkI_uNGf4TVIzgs8FyVeXSVlQOkfZR4z9SArJuQJww/values:batchGet?majorDimension=COLUMNS&ranges=A1&key=AIzaSyCeFExPhC-xWNxyQT7KCBMisahAYTg1I0k

        
        spreadsheetGrej = new googlespreasheetObject();
        Debug.Log(json);
        try
        {

            spreadsheetGrej = JsonConvert.DeserializeObject<googlespreasheetObject>(json);

            Debug.Log(spreadsheetGrej.valueRanges[0].values.Count);
        }
        catch(Exception ex)
        {
            Debug.Log(ex.Message.ToString());
        }
        // print("hej fast metoden efter wait " + spreadsheetGrej.spreadsheetId ); 


        try
        {

            attackSpellsObjectArray = Resources.LoadAll<AttackSpell>("ScriptableObjects/Cards/Spells/Attacks");
        //    attackSpellsObject = AssetBundle.LoadFromFile("Assets/ScriptableObjects/Cards/Spells/Attacks").GetComponents<AttackSpell>();
        //    print(attackSpellsObjectArray.Length);
          //  print("attackspells object " + attackSpellsObject[0].cardName);


        }
        catch (Exception ex)
        {
                Debug.Log(ex.Message.ToString());
        }
        try
        {
            attackSpellsObjectList = new List<AttackSpell>();
            attackSpellsObjectList.AddRange(attackSpellsObjectArray);
            int amountOfCardsChanged = 0;
            for (int i = 0; i < spreadsheetGrej.valueRanges.Count; i++)
            {   
                for(int z = 0; z < spreadsheetGrej.valueRanges[0].values.Count; z++)
                {
                    List<string> currentCard = spreadsheetGrej.valueRanges[0].values[z];
                    AttackSpell scriptableObject;
                    if ((scriptableObject = findAttackCardFromName(attackSpellsObjectList, currentCard[0])) != null)
                    {   
                        bool isDamageChanged = false;
                        if (!currentCard[5].Equals("-"))
                        {
                            isDamageChanged = System.Convert.ToInt32(currentCard[5]) != scriptableObject.damage ;
                        }
                        if (!scriptableObject.description.Equals(currentCard[3]) || isDamageChanged)
                        {
                          //  print("kommer den hit");
                            scriptableObject.description = currentCard[3];
                            
                            scriptableObject.damage = System.Convert.ToInt32( currentCard[5]);

                            EditorUtility.SetDirty( scriptableObject);
                            amountOfCardsChanged += 1;
                        }



                    }
                    else
                    {
                        //StreamWriter writer = new StreamWriter("Assets/Resources/ScriptableObjects/UnimplementedCards");
                          StreamWriter temp =  File.CreateText("D:\\Bullshitmapp");
                            temp.WriteLine("hej hej");
                      //  System.IO.File.WriteAllText("Assets/All unimplemented cards", currentCard[0]);
                        
                    }
                }
              

            }
            if (amountOfCardsChanged != 0)
            {
                    
                    Debug.Log(amountOfCardsChanged + " cards was changed");
            }
        }
        catch(Exception ex)
        {
            Debug.Log(ex.Message.ToString());
        }

    }

    
    private AttackSpell findAttackCardFromName(List<AttackSpell> listToSearch, string name)
    {

        foreach(AttackSpell spell in listToSearch)
        {
            if(spell.cardName.Equals(name))
            {
                return spell;
            }
        }


        return null;
    }
}

