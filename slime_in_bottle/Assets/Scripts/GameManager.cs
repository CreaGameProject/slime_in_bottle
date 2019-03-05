using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject flont_UI, back_UI;
    [SerializeField] GameObject parent;
    [SerializeField] GameObject bottle;
    [SerializeField] GameObject item;
    [SerializeField] GameObject child;
    int UI_flag = 0; //アイテム画面(0)と棚画面(1)切り替えのフラグ
    const int item_num = 15; //セーブデータ数
    List<string[]> itemData = new List<string[]>(); //CSVファイルのデータを格納するリスト
    SlimeStatus slime = new SlimeStatus();

    void Start()
    {
        ReadFile();

        slime.Status = 0;
        slime.likePoint = 0;

        SlimeStatus();
    }

    void Update()
    {
        Debug.Log(slime.Status);
    }

    void SlimeStatus()
    {
        for (int i = 0; i < slime.attribute_num; i++)
        {
            slime.attribute[i] = Random.Range(-10f, 10f);
            Debug.Log(i + ":" + slime.attribute[i]);
        }
    }

    /// <summary>
    /// CSVファイルからアイテム画像を読み込む関数
    /// </summary>
    void ReadFile()
    {
        TextAsset itemList = Resources.Load("itemList") as TextAsset;
        StringReader reader = new StringReader(itemList.text);
        List<GameObject> items = new List<GameObject>();

        while (reader.Peek() != -1)
        {
            string line = reader.ReadLine();
            itemData.Add(line.Split(','));
        }

        for (int i = 0; i <= 359; i++)
        {
            if (itemData[i][3] == "1" && File.Exists("Assets/Resources/Sprites/" + itemData[i][1]))
            {
                GameObject clone = Instantiate(item, parent.transform);
                GameObject e = Instantiate(child, clone.transform);
                e.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/" + itemData[i][1].Replace(".png", ""));
                items.Add(e);
                EventTrigger eventTrigger = e.AddComponent<EventTrigger>();
                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.EndDrag;
                int j = i + 0;
                entry.callback.AddListener((x) => OnEndDrag(itemData[j][1].Replace(".png", "")));
                eventTrigger.triggers.Add(entry);
            }
        }



        for (int i = 0; i <= item_num - 1; i++)
        {
            Transform clone = Instantiate(item, bottle.transform).transform;
            Instantiate(child, clone).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/bottle");
        }

        bottle.SetActive(false);
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
            parent.SetActive(true);
            bottle.SetActive(false);
        }
        else
        {
            parent.SetActive(false);
            bottle.SetActive(true);
        }
    }

    public void OnEndDrag(string itemName)
    {
        switch (itemName)
        {
            case "balloon1":
                for (int i = 13; i <= 36; i++)
                {
                    if (itemData[3][i] == "〇")
                    {
                        if (slime.attribute[i - 13] > 0)
                        {
                            slime.Status += Random.Range(50, 80);
                        }
                        else if(slime.attribute[i - 13] < 0)
                        {
                            slime.Status -= Random.Range(50, 80);
                        }
                    }
                }
                break;
            case "balloon3":
                for (int i = 13; i <= 36; i++)
                {
                    if (itemData[5][i] == "〇")
                    {
                        if (slime.attribute[i - 13] > 0)
                        {
                            slime.Status += Random.Range(50, 80);
                        }
                        else if (slime.attribute[i - 13] < 0)
                        {
                            slime.Status -= Random.Range(50, 80);
                        }
                    }
                }
                break;
        }
    }
}

/// <summary>
/// スライムの個体値管理クラス
/// </summary>
//[System.Serializable]
public class SlimeStatus
{
    [System.NonSerialized] public float likePoint;  //信頼↔嫌悪の基本となる感情 スライムの好みによって変動する変数
    [System.NonSerialized] public int attribute_num = 23;
    [System.NonSerialized]
    public float[] attribute = new float[23];
    [System.NonSerialized]
    private float status;

    public float Status
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
            else if(value < -100)
            {
                status = -100;
            }
            else
            {
                status = value;
            }
        }
    }
    //public float red, blue, yellow, green, purple, white, black, bright, foods, fruits,
    //           sweets, toys, hot, cool, hard, soft, curiosity, fancy, sober, round, anglar;

}
