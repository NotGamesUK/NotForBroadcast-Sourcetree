using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;



public class TestingResourcesLoading : MonoBehaviour {

	// Use this for initialization
	void Start () {
        WriteToFile();
        Invoke("ReadAndPrintFile", 1);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void ReadAndPrintFile()
    {
        var textFile = Resources.Load<TextAsset>("Text/textFile01");
        if (textFile)
        {
            Debug.Log("File found and Loaded: " + textFile);
        }
        else
        {
            Debug.Log("File not found.");
        }

    }

    public static void WriteToFile()
    {
        StreamWriter sw = new StreamWriter(Application.persistentDataPath + "/Video Reader Output/textFile01.txt");

        sw.WriteLine("Generated table of 1 to 10");
        sw.WriteLine("");

        for (int i = 1; i <= 10; i++)
        {
            for (int j = 1; j <= 10; j++)
            {
                sw.WriteLine("{0}x{1}= {2}", i, j, (i * j));
            }

            sw.WriteLine("====================================");
        }

        sw.WriteLine("Table successfully written to file!");

        sw.Close();
    }
}
