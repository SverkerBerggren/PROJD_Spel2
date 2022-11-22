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
            spellObjects = new List<Spells>(Resources.LoadAll<Spells>("ScriptableObjects/Cards/Spells/Support"));
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
                        ChangeSupportSpells(currentCard);
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

    private bool CheckIfDefaultCardInfoChanged(Card scriptableObject, List<string> currentCard)
    {
        if (!scriptableObject.description.Equals(currentCard[descriptionIndex]) || !scriptableObject.maxManaCost.Equals(Convert.ToInt32(currentCard[manaIndex])))
            return true;
        else
            return false;
    }

    private void ChangeSupportSpells(List<string> currentCard)
    {
        Spells scriptableObject = (Spells)FindCardFromName(spellObjects.Cast<Card>().ToList(), currentCard[0]);       

        string textName = currentCard[cardNameIndex] + ".txt";
        if (scriptableObject != null)
        {
            if (scriptableObject is HealAndShieldChampion)
            {
                ChangeHealingAndShieldingSpells(currentCard);
                return;
            }
			bool drawChange = false;
			bool discardChange = false;

			if (!currentCard[healIndex].Equals("-"))
			{
				drawChange = Convert.ToInt32(currentCard[healIndex]) != scriptableObject.amountOfCardsToDraw;
			}

			if (!currentCard[healIndex].Equals("-"))
			{
				discardChange = Convert.ToInt32(currentCard[healIndex]) != scriptableObject.amountOfCardsToDiscard;
			}

			if (CheckIfDefaultCardInfoChanged(scriptableObject, currentCard))
            {
                StreamWriter temp = File.CreateText(Application.dataPath + "/Resources/ChangedCards/" + textName);
                temp.WriteLine("Old Card");

                temp.WriteLine(scriptableObject.WriteOutCardInfo());
                string oldString = scriptableObject.WriteOutCardInfo();

                updatedFiles = true;
                scriptableObject.description = currentCard[descriptionIndex];
                scriptableObject.maxManaCost = Convert.ToInt32(currentCard[manaIndex]);
                string newString = scriptableObject.WriteOutCardInfo();

                MakeDirty(scriptableObject);
                amountOfCardsChanged += 1;
                temp.WriteLine("\nNew Card");

                foreach (string s in CompareStringChanges(oldString, newString))
                {
                    temp.WriteLine(s);
                }

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

    private void ChangeHealingAndShieldingSpells(List<string> currentCard)
    {
        HealAndShieldChampion scriptableObject = (HealAndShieldChampion)FindCardFromName(spellObjects.Cast<Card>().ToList(), currentCard[0]);
        string textName = currentCard[cardNameIndex] + ".txt";
        if (scriptableObject != null)
        {
            bool shieldChange = false;
            bool healChange = false;

            if (!currentCard[shieldIndex].Equals("-"))
            {
                shieldChange = Convert.ToInt32(currentCard[shieldIndex]) != scriptableObject.amountToDefence;
            }
            if (!currentCard[healIndex].Equals("-"))
            {
                healChange = Convert.ToInt32(currentCard[healIndex]) != scriptableObject.amountToHeal;
            }
            if (CheckIfDefaultCardInfoChanged(scriptableObject, currentCard) || shieldChange || healChange)
            {
                StreamWriter temp = File.CreateText(Application.dataPath + "/Resources/ChangedCards/" + textName);
                temp.WriteLine("Old Card");

                temp.WriteLine(scriptableObject.WriteOutCardInfo());
                string oldString = scriptableObject.WriteOutCardInfo();

                updatedFiles = true;
                scriptableObject.description = currentCard[descriptionIndex];
                scriptableObject.maxManaCost = Convert.ToInt32(currentCard[manaIndex]);
                if (shieldChange)
                    scriptableObject.amountToDefence = Convert.ToInt32(currentCard[shieldIndex]);
                if (healChange)
                    scriptableObject.amountToHeal = Convert.ToInt32(currentCard[healIndex]);
                string newString = scriptableObject.WriteOutCardInfo();

                MakeDirty(scriptableObject);
                amountOfCardsChanged += 1;
                temp.WriteLine("\nNew Card");

                foreach (string s in CompareStringChanges(oldString, newString))
                {
                    temp.WriteLine(s);
                }
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

    private void ChangeAttackCards(List<string> currentCard)
    {
        AttackSpell scriptableObject = (AttackSpell)FindCardFromName(attackSpellsObjects.Cast<Card>().ToList(), currentCard[0]);
        string textName = currentCard[ cardNameIndex] + ".txt";
        if (scriptableObject != null)
        {
            bool isDamageChanged = false;
            if (!currentCard[damageIndex].Equals("-"))
            {
                isDamageChanged = Convert.ToInt32(currentCard[damageIndex]) != scriptableObject.damage;
            }
            if (CheckIfDefaultCardInfoChanged(scriptableObject, currentCard) || isDamageChanged)
            {
                StreamWriter temp = File.CreateText(Application.dataPath + "/Resources/ChangedCards/" + textName);
                temp.WriteLine("Old Card");
                
                temp.WriteLine( scriptableObject.WriteOutCardInfo());
				string oldString = scriptableObject.WriteOutCardInfo();

				updatedFiles = true;
                scriptableObject.description = currentCard[descriptionIndex];
                scriptableObject.maxManaCost = Convert.ToInt32(currentCard[manaIndex]);           
                scriptableObject.damage = Convert.ToInt32(currentCard[damageIndex]);
				string newString = scriptableObject.WriteOutCardInfo();

                MakeDirty(scriptableObject);
                amountOfCardsChanged += 1;
                temp.WriteLine("\nNew Card");

				foreach (string s in CompareStringChanges(oldString, newString))
				{
					temp.WriteLine(s);
				}

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
            if (CheckIfDefaultCardInfoChanged(scriptableObject, currentCard) || !scriptableObject.minionHealth.Equals(Convert.ToInt32(currentCard[healthIndex])))
            {
                StreamWriter temp = File.CreateText(Application.dataPath + "/Resources/ChangedCards/" + textName);
				temp.WriteLine("Old Card");
                temp.WriteLine(scriptableObject.WriteOutCardInfo());

                string oldString = scriptableObject.WriteOutCardInfo();

				updatedFiles = true;
                scriptableObject.description = currentCard[descriptionIndex];
                scriptableObject.minionHealth = System.Convert.ToInt32(currentCard[healthIndex]);
                scriptableObject.maxManaCost = System.Convert.ToInt32(currentCard[manaIndex]);

				string newString = scriptableObject.WriteOutCardInfo();

                MakeDirty(scriptableObject);
                amountOfCardsChanged += 1;
				temp.WriteLine("\nNew Card");

                foreach (string s in CompareStringChanges(oldString, newString))
                {
				    temp.WriteLine(s);
                }
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

    private bool ChangedVariable(Card cardObject, List<string> currentCard, int index)
    {
        for (int i = 0; i < currentCard.Count; i++)
        {
            if (currentCard[i].Equals("-")) continue;

            switch (index)
            {
                case healthIndex:

                break;

                case healIndex:

                break;

			    case damageIndex:

		        break;

			    case shieldIndex:

			    break;

			    case drawIndex:

			    break;

			    case discardIndex:

			    break;
		    }
            
        }

        return false;
    }

    private void MakeDirty(Card scriptableObject)
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