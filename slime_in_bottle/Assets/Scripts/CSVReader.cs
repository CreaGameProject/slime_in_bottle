using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CSVReader : MonoBehaviour
{
    TextAsset itemList;
    string line;
    [SerializeField] List<string[]> itemData = new List<string[]>();

    void Start()
    {
        itemList = Resources.Load("itemList") as TextAsset;
        StringReader reader = new StringReader(itemList.text);

        while(reader.Peek() != -1)
        {
            line = reader.ReadLine();
            itemData.Add(line.Split(','));

            //Debug.Log(itemData[0][3]);
        }
    }

    void Update()
    {

    }
}
