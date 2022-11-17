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
using static SpreadsheetData;
using Unity.VisualScripting;
using UnityEditor;
using System.Transactions;
using System.IO;
using System.Runtime.CompilerServices;



//using HttpClient clienten = new();

//await ProcessRepositoriesAsync(clienten);




public class SpreadsheetUpdater : EditorWindow
{
    private List<AttackSpell> attackSpellsObjects;
    private List<Landmarks> landmarkObjects;
    private List<DefendSpell> defendSpellsObjects;
    private List<HealChampion> healSpellsObjects;
    private List<List<string>> cardData;
    private HttpClient client = new HttpClient();
    private SpreadsheetData spreadsheetData;

    //Values from spreadsheet
    private int cardNameIndex = 0;
    private int cardTypeIndex = 1;
    private int manaIndex = 2;
    private int descriptionIndex = 3;
    private int healthIndex = 4;
    private int attackIndex = 5;

    private int amountOfCardsChanged = 0;
    private bool updatedFiles = false;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.J))
        {
            
            Task hej = ProcessRepositoriesAsync(client);
        }
    }

    [MenuItem("Tools/spreadsheet updater")]
    public static void ShowWindow()
    {
        GetWindow(typeof(SpreadsheetUpdater));
    }

    private void OnGUI()
    {
        GUILayout.Label("Update from spreadsheet",EditorStyles.boldLabel);

        if(GUILayout.Button("Update cards"))
        {
            Task theTask =  ProcessRepositoriesAsync(client);
        }
    }
    

    public async Task ProcessRepositoriesAsync(HttpClient client)
    {   
        Debug.ClearDeveloperConsole();
        var jsonHowBigSpreadsheet = await client.GetStringAsync(
            "https://sheets.googleapis.com/v4/spreadsheets/1qpkI_uNGf4TVIzgs8FyVeXSVlQOkfZR4z9SArJuQJww/values:batchGet?majorDimension=ROWS&ranges=K2&key=AIzaSyCeFExPhC-xWNxyQT7KCBMisahAYTg1I0k");

        string stringToAppend = ""; 
        try
        {
            SpreadsheetData parseJsonHowBigSpreadsheet = JsonConvert.DeserializeObject<SpreadsheetData>(jsonHowBigSpreadsheet);

            stringToAppend = parseJsonHowBigSpreadsheet.valueRanges[0].values[0][0].ToString();
            string key = "&key=AIzaSyCeFExPhC-xWNxyQT7KCBMisahAYTg1I0k";

            var json = await client.GetStringAsync(
                "https://sheets.googleapis.com/v4/spreadsheets/1qpkI_uNGf4TVIzgs8FyVeXSVlQOkfZR4z9SArJuQJww/values:batchGet?majorDimension=ROWS&ranges=A2:H" + stringToAppend + key);
            //https://sheets.googleapis.com/v4/spreadsheets/1qpkI_uNGf4TVIzgs8FyVeXSVlQOkfZR4z9SArJuQJww/values:batchGet?majorDimension=COLUMNS&ranges=A1&key=AIzaSyCeFExPhC-xWNxyQT7KCBMisahAYTg1I0k


            spreadsheetData = new SpreadsheetData();
            spreadsheetData = JsonConvert.DeserializeObject<SpreadsheetData>(json);
            attackSpellsObjects = new List<AttackSpell>(Resources.LoadAll<AttackSpell>("ScriptableObjects/Cards/Spells/Attacks"));
            landmarkObjects = new List<Landmarks>(Resources.LoadAll<Landmarks>("ScriptableObjects/Cards/Landmarks"));
        //    defendSpellsObjects = new List<DefendSpell>(Resources.LoadAll<DefendSpell>("ScriptableObjects/Cards/Spells/Defence"));
        //    attackSpellsObjects = new List<AttackSpell>(Resources.LoadAll<AttackSpell>("ScriptableObjects/Cards/Spells/Attacks"));


            CheckCardList();


        }
        catch(Exception ex)
        {
            Debug.LogError(ex.Message.ToString());   
        }
    }

    
    private Card FindCardFromName(List<Card> listToSearch, string name)
    {

        foreach(Card spell in listToSearch)
        {
            if(spell.cardName.Equals(name))
            {
                return spell;
            }
        }

        return null;
    }

    private void CheckCardList()
    {
        for (int i = 0; i < spreadsheetData.valueRanges.Count; i++)
        {
            for (int z = 0; z < spreadsheetData.valueRanges[0].values.Count; z++)
            {
                List<string> currentCard = spreadsheetData.valueRanges[0].values[z];
                switch (currentCard[cardTypeIndex])
                {
                    case "Attack":
                        ChangeAttackCards(currentCard);
                        break;

                    case "Support":
                        break;

                    case "Landmark":
                        ChangeLandmarkCards(currentCard);
                        break;
                }
            }
        }

        if (amountOfCardsChanged != 0)
            Debug.Log(amountOfCardsChanged + " cards was changed");

        amountOfCardsChanged = 0; 

        if (updatedFiles)
            Debug.Log("Tab in and out to see new changes in unimplemented and changed cards");
    }

    private void ChangeAttackCards(List<string> currentCard)
    {
        List<string> oldCard;
        
        AttackSpell scriptableObject = (AttackSpell)FindCardFromName(attackSpellsObjects.Cast<Card>().ToList(), currentCard[0]);
        string textName = currentCard[ cardNameIndex] + ".txt";
        if (scriptableObject != null)
        {
            bool isDamageChanged = false;
            if (!currentCard[attackIndex].Equals("-"))
            {
                isDamageChanged = System.Convert.ToInt32(currentCard[attackIndex]) != scriptableObject.damage;
            }
            if (!scriptableObject.description.Equals(currentCard[descriptionIndex]) || isDamageChanged || !scriptableObject.maxManaCost.Equals(Convert.ToInt32(currentCard[manaIndex])))
            {
                StreamWriter temp = File.CreateText(Application.dataPath + "/Resources/ChangedCards/" + textName);
                temp.WriteLine("Old Card");
                
                temp.WriteLine( scriptableObject.ToString());


                updatedFiles = true;
                scriptableObject.description = currentCard[descriptionIndex];
                scriptableObject.maxManaCost = Convert.ToInt32(currentCard[manaIndex]);           
                scriptableObject.damage = Convert.ToInt32(currentCard[attackIndex]);
        

                EditorUtility.SetDirty(scriptableObject);
                amountOfCardsChanged += 1;
                temp.WriteLine("\nNew Card");
				
                temp.Write(scriptableObject.ToString());

				temp.Close();
            }
        }
        else
        {
            updatedFiles = true;
            StreamWriter temp = File.CreateText(Application.dataPath + "/Resources/UnimplementedCards/" + textName);
            temp.Close();
        }
    }
    
    private void ChangeLandmarkCards(List<string> currentCard)
    {
        Landmarks scriptableObject = (Landmarks)FindCardFromName(landmarkObjects.Cast<Card>().ToList(), currentCard[0]);
        string textName = currentCard[cardNameIndex] + ".txt";
        if (scriptableObject != null)
        {
            if (!scriptableObject.description.Equals(currentCard[descriptionIndex]) || !scriptableObject.minionHealth.Equals(Convert.ToInt32(currentCard[healthIndex])) || !scriptableObject.maxManaCost.Equals(Convert.ToInt32(currentCard[manaIndex])))
            {
                updatedFiles = true;
                scriptableObject.description = currentCard[descriptionIndex];
                scriptableObject.minionHealth = System.Convert.ToInt32(currentCard[healthIndex]);
                scriptableObject.maxManaCost = System.Convert.ToInt32(currentCard[manaIndex]);    
                EditorUtility.SetDirty(scriptableObject);
                amountOfCardsChanged += 1;

                StreamWriter temp = File.CreateText(Application.dataPath + "/Resources/ChangedCards/" + textName);
                temp.Close();
            }
        }
        else
        {
            updatedFiles = true;
            StreamWriter temp = File.CreateText(Application.dataPath + "/Resources/UnimplementedCards/" + textName);
            temp.Close();
        }
    }
}

