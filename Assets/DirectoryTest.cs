using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dapper;
using System.Data.SQLite;
using System.IO;
using System.Data;
using System.Linq;

public class DirectoryTest : MonoBehaviour
{
    public void OnClick()
    {
        print(Directory.GetCurrentDirectory());

        StreamWriter writer = File.CreateText( "Assets/TestDatabas" + "/hej.txt");
        writer.Write("HEJ!");
        writer.Close();

        System.Data.SQLite.SQLiteCommand sQLiteCommand = new SQLiteCommand();
        print("hej 1");
        List<Test> tests = LoadTest();
        print(tests.Count);
    }
    public static List<Test> LoadTest()
    {
        using (IDbConnection cnn = new SQLiteConnection("Data Source=.\\Assets\\TestDatabas\\TestDataBas.db;Version=3;"))
        {
            print("hej 2");
            var output = cnn.Query<Test>("select * from Test", new DynamicParameters());
            print("hej 3");




            return output.ToList();

        }
    }

    public static void SaveTest(Test hej)
    {
        using (IDbConnection cnn = new SQLiteConnection("Data Source=.\\TestDatabas.db;Version=3;"))
        {
            cnn.Execute("insert into test (Namn) values (@Namn)", hej);
        }
    }

    
}
public class Test
{
    public string name { get; set; }
}

