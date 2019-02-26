using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject flont_UI, back_UI;
    [SerializeField] GameObject item;
    [SerializeField] GameObject parent;
    [SerializeField] GameObject child;
    int UI_flag = 0; //アイテム画面(0)と棚画面(1)切り替えのフラグ
    const int item_num = 15; //アイテム数
    List<string[]> itemData = new List<string[]>(); //CSVファイルのデータを格納するリスト

    int status; //信頼↔嫌悪の基本となる感情
    int likePoint; //スライムの好みによって変動する変数

    public int Status
    {
        get
        {
            return status;
        }

        set
        {
            if (value > 100)
            {
                status = 100;
            }
            else if (value < -100)
            {
                status = -100;
            }
            else
            {
                status = value;
            }
        }
    }

    public int LikePoint
    {
        get
        {
            return likePoint;
        }

        set
        {
            if (value > 10)
            {
                likePoint = 10;
            }
            else if (value < -10)
            {
                likePoint = -10;
            }
            else
            {
                likePoint = value;
            }
        }
    }

    void Start()
    {
        ReadFile();
        Status = 0;
        LikePoint = 0;
    }

    void Update()
    {

    }

    /// <summary>
    /// CSVファイルからアイテム画像を読み込む関数
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

        int j = 0;

        for (int i = 0; i <= 359; i++)
        {
            if (itemData[i][3] == "1" && File.Exists("Assets/Resources/Sprites/" + itemData[i][1]) && j < 15)
            {
                Transform clone = Instantiate(item, parent.transform).transform;
                Instantiate(child, clone).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/" + itemData[i][1].Replace(".png", ""));
                j++;
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
            int j = 0;

            for (int i = 0; i <= 359; i++)
            {
                string fileName = itemData[i][1].Replace(".png", "");

                if (itemData[i][3] == "1" && File.Exists("Assets/Resources/Sprites/" + itemData[i][1]) && j < 15)
                {
                    //list.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/" + itemData[i][1].Replace(".png", ""));
                    j++;
                }
            }
        }
        else
        {
            for (int i = 0; i <= item_num - 1; i++)
            {
               // list.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/bottle");
            }
        }
    }
}

/// <summary>
/// スライムの個体値管理クラス
/// </summary>
public class SlimeStatus
{
    public int red;
    public int blue;
    public int yellow;
    public int green;
    public int purple;
    public int white;
    public int black;
    public int bright;
    public int foods;
    public int fruits;
    public int sweets;
    public int toys;
    public int hot;
    public int cool;
    public int hard;
    public int soft;
    public int curiosity;
    public int fancy;
    public int sober;
    public int round;
    public int anglar;
}
