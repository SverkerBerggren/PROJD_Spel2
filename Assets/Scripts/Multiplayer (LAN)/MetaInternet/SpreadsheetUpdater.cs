using System.Collections.Generic;
using UnityEngine;
using System.Net.Http;
using System;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEditor;
using System.IO;


public class SpreadsheetUpdater 
{
    private List<AttackSpell> attackSpellsObjects;
    private List<Landmarks> landmarkObjects;
    private List<HealAndShieldChampion> healAndShieldSpellsObjects;
    private List<Spells> spellObjects;
    private List<List<string>> cardData;
    public HttpClient client = new HttpClient();
    private SpreadsheetData spreadsheetData;

    //Values from spreadsheet
    private const int cardNameIndex = 0;
    private const int cardTypeIndex = 1;
    private const int manaIndex = 2;
    private const int descriptionIndex = 3;
    private const int healthIndex = 4;
    private const int damageIndex = 5;
    private const int shieldIndex = 6;
    private const int healIndex = 7;
	private const int drawIndex = 8;
	private const int discardIndex = 9;

    private List<Card> cards;

	private int amountOfCardsChanged = 0;
    private bool updatedFiles = false;

    public void ClearFiles()
    {
        try
        {          
            object[] filesToClearUnimplemented = Resources.LoadAll("UnimplementedCards");
            object[] filesToClearChangd = Resources.LoadAll("ChangedCards");
            
            for (int i = 0; i < filesToClearUnimplemented.Length; i++)
            {                             
                string fileToDelete = filesToClearUnimplemented[i].ToSafeString();

                File.Delete(Application.dataPath + "/Resources/UnimplementedCards/" + fileToDelete + ".meta");
                File.Delete(Application.dataPath + "/Resources/UnimplementedCards/" + fileToDelete + ".txt");
            }
            for (int i = 0; i < filesToClearChangd.Length; i++)
            {               
                string fileToDelete = filesToClearChangd[i].ToSafeString();

                File.Delete(Application.dataPath + "/Resources/ChangedCards/" + fileToDelete + ".meta");
                File.Delete(Application.dataPath + "/Resources/ChangedCards/" + fileToDelete + ".txt");
            }

            Debug.ClearDeveloperConsole();
            Debug.Log("Cleared files, tab to see it ");
        }
        catch(Exception ex)
        {
            Debug.LogError(ex.Message.ToString());  
        }
    }

    public async Task ProcessRepositoriesAsync(HttpClient client)
    {   
        Debug.ClearDeveloperConsole();
        var jsonHowBigSpreadsheet = await client.GetStringAsync(
            "https://sheets.googleapis.com/v4/spreadsheets/1qpkI_uNGf4TVIzgs8FyVeXSVlQOkfZR4z9SArJuQJww/values:batchGet?majorDimension=ROWS&ranges=M2&key=AIzaSyCeFExPhC-xWNxyQT7KCBMisahAYTg1I0k");

        string stringToAppend = ""; 
        try
        {
            SpreadsheetData parseJsonHowBigSpreadsheet = JsonConvert.DeserializeObject<SpreadsheetData>(jsonHowBigSpreadsheet);

            stringToAppend = parseJsonHowBigSpreadsheet.valueRanges[0].values[0][0].ToString();
            string key = "&key=AIzaSyCeFExPhC-xWNxyQT7KCBMisahAYTg1I0k";

            var json = await client.GetStringAsync(
                "https://sheets.googleapis.com/v4/spreadsheets/1qpkI_uNGf4TVIzgs8FyVeXSVlQOkfZR4z9SArJuQJww/values:batchGet?majorDimension=ROWS&ranges=A2:J" + stringToAppend + key);
            //https://sheets.googleapis.com/v4/spreadsheets/1qpkI_uNGf4TVIzgs8FyVeXSVlQOkfZR4z9SArJuQJww/values:batchGet?majorDimension=COLUMNS&ranges=A1&key=AIzaSyCeFExPhC-xWNxyQT7KCBMisahAYTg1I0k

            spreadsheetData = new SpreadsheetData();
            spreadsheetData = JsonConvert.DeserializeObject<SpreadsheetData>(json);
            attackSpellsObjects = new List<AttackSpell>(Resources.LoadAll<AttackSpell>("ScriptableObjects/Cards/Spells/Attacks"));
            landmarkObjects = new List<Landmarks>(Resources.LoadAll<Landmarks>("ScriptableObjects/Cards/Landmarks"));
            spellObjects = new List<Spells>(Resources.LoadAll<Spells>("ScriptableObjects/Cards/Spells/Support"));
            

            cards = new List<Card>(attackSpellsObjects);
            cards.AddRange(landmarkObjects);
            cards.AddRange(spellObjects);

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
				
                ChangeCards(currentCard);
			}
		}

		if (amountOfCardsChanged != 0)
			Debug.Log(amountOfCardsChanged + " cards was changed");

		amountOfCardsChanged = 0;

		if (updatedFiles)
			Debug.Log("Tab in and out to see new changes in unimplemented and changed cards");
	}

    private void ChangeCards(List<string> currentCard)
    {       
		Card scriptableObject = (Card)FindCardFromName(cards.Cast<Card>().ToList(), currentCard[0]);
		string textName = currentCard[cardNameIndex] + ".txt";
		
		if (scriptableObject != null)
		{

            string oldString = scriptableObject.WriteOutCardInfo();
			
			List<int> indexChanges = ChangeVariables(scriptableObject, currentCard);
			
			string newString = scriptableObject.WriteOutCardInfo();

            if (indexChanges.Count <= 0) return;
            

			StreamWriter temp = File.CreateText(Application.dataPath + "/Resources/ChangedCards/" + textName);
			temp.WriteLine("Old Card");

			temp.WriteLine(oldString);
			

			
			MakeDirty(scriptableObject);
			amountOfCardsChanged += 1;
			temp.WriteLine("\nNew Card");

			foreach (string s in CompareStringChanges(oldString, newString))
			{
				temp.WriteLine(s);
			}

			temp.Close();
			
		}
		else
		{
			updatedFiles = true;
			StreamWriter temp = File.CreateText(Application.dataPath + "/Resources/UnimplementedCards/" + textName);
			temp.Close();
		}
	}

    private string[] CompareStringChanges(string old, string newS)
    {
		string[] oldStringSplit = old.Split("\n");
		string[] newStringSplit = newS.Split("\n");

		for (int i = 0; i < oldStringSplit.Length; i++)
		{
			if (!oldStringSplit[i].Equals(newStringSplit[i]))
			{
				newStringSplit[i] = "--->\t" + newStringSplit[i];
			}
		}
        return newStringSplit;
	}

    private List<int> ChangeVariables(Card cardObject, List<string> currentCard)
    {
        List<int> changed = new List<int>();
        for (int i = 0; i < currentCard.Count; i++)
        {
            if (currentCard[i].Equals("-")) continue;

            switch (i)
            {
                case healthIndex:
                    if (cardObject is Landmarks)
                    {
                        Landmarks l = (Landmarks)cardObject;
                        if (Convert.ToInt32(currentCard[i]) != l.minionHealth)
                        {
                            l.minionHealth = Convert.ToInt32(currentCard[i]);
				    	    changed.Add(i);
				    	}
                    }
                    break;

                case healIndex:
                    if (cardObject.amountToHeal != Convert.ToInt32(currentCard[i]))
                    {
                        cardObject.amountToHeal = Convert.ToInt32(currentCard[i]);
					    changed.Add(i);
                    }
					break;

			    case damageIndex:
					if (cardObject.damage != Convert.ToInt32(currentCard[i]))
					{
						cardObject.damage = Convert.ToInt32(currentCard[i]);
						changed.Add(i);
					}
					break;

			    case shieldIndex:
					if (cardObject.amountToShield != Convert.ToInt32(currentCard[i]))
					{
						cardObject.amountToShield = Convert.ToInt32(currentCard[i]);
						changed.Add(i);
					}
					break;

			    case drawIndex:
					if (cardObject.amountOfCardsToDraw != Convert.ToInt32(currentCard[i]))
					{
						cardObject.amountOfCardsToDraw = Convert.ToInt32(currentCard[i]);
						changed.Add(i);
					}
					break;

			    case discardIndex:
					if (cardObject.amountOfCardsToDiscard != Convert.ToInt32(currentCard[i]))
					{
						cardObject.amountOfCardsToDiscard = Convert.ToInt32(currentCard[i]);
						changed.Add(i);
					}
                    break;

			    case descriptionIndex:
					if (!cardObject.description.Equals(currentCard[descriptionIndex]))
					{
						cardObject.description = currentCard[i];
						changed.Add(i);
					}
					break;
		    }
            
        }
        return changed;
    }

    private static void MakeDirty(Card scriptableObject)
    {
        #if UNITY_EDITOR
        EditorUtility.SetDirty(scriptableObject);
        #endif
    }
}



#if UNITY_EDITOR
public class SpreadSheetUpdatetWindow : EditorWindow
{   private SpreadsheetUpdater spreadsheetUpdater = new SpreadsheetUpdater();
    [MenuItem("Tools/spreadsheet updater")]
    public static void ShowWindow()
    {
        GetWindow(typeof(SpreadSheetUpdatetWindow));
    }

    private void OnGUI()
    {
        GUILayout.Label("Update from spreadsheet", EditorStyles.boldLabel);

        if (GUILayout.Button("Update cards"))
        {
            Task theTask = spreadsheetUpdater.ProcessRepositoriesAsync(spreadsheetUpdater.client);
        }
        if (GUILayout.Button("Clear TextFiles"))
        {
            spreadsheetUpdater.ClearFiles();
        }
    }

}
#endif