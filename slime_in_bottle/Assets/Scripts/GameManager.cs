using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject flont_UI, back_UI;
    [SerializeField] Sprite[] items; //アイテムと瓶のスプライト
    [SerializeField] GameObject[] list; //アイテムと瓶のリスト
    int UI_flag = 0; //アイテム画面(0)と棚画面(1)切り替えのフラグ
    const int item_num = 15; //アイテム数
    List<string[]> itemData = new List<string[]>();

    void Start()
    {
        ReadFile();
    }

    void Update()
    {

    }

    /// <summary>
    /// CSVファイルを読み込む関数
    /// </summary>
    void ReadFile()
    {
        TextAsset itemList = Resources.Load("itemList") as TextAsset;
        StringReader reader = new StringReader(itemList.text);

        while (reader.Peek() != -1)
        {
            string line = reader.ReadLine();
            itemData.Add(line.Split(','));
        }

        for(int i = 0; i <= 359; i++)
        {
            string imageName = itemData[i][1];
            MatchCollection match = Regex.Matches(imageName, "^[a-z].*");

            foreach(Match m in match)
            {
                Debug.Log(m.Value);
            }
        }
    }

    /// <summary>
    /// UIの切り替えを行う関数
    /// </summary>
    public void Change_UI()
    {
        Color flont_c = flont_UI.GetComponent<Image>().color;
        Color back_c = back_UI.GetComponent<Image>().color;

        flont_UI.GetComponent<Image>().color = back_c;
        back_UI.GetComponent<Image>().color = flont_c;

        UI_flag = 1 - UI_flag;

        if (UI_flag == 0)
        {
            for (int i = 0; i <= item_num - 1; i++)
            {
                list[i].GetComponent<Image>().sprite = items[i];
            }
        }
        else
        {
            for (int i = 0; i <= item_num - 1; i++)
            {
                list[i].GetComponent<Image>().sprite = items[item_num];
            }
        }
    }
}
