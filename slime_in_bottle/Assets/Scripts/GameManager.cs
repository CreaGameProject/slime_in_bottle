﻿using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.U2D;

enum Emotion
{
    NOMAL, HAPPY, SAD, ANGRY, HAPPY_B, SURPRISE, TIRED, HOPE, FEAR
}

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject flont_UI, back_UI;
    [SerializeField] GameObject parent;
    [SerializeField] GameObject bottle;
    [SerializeField] GameObject item;
    [SerializeField] GameObject child;
    [SerializeField] Image slime_image;

    int UI_flag = 0; //アイテム画面(0)と棚画面(1)切り替えのフラグ
    const int item_num = 15; //セーブデータ数
    List<string[]> itemData = new List<string[]>(); //CSVファイルのデータを格納するリスト
    SlimeStatus slime = new SlimeStatus();

    void Start()
    {
        ReadFile();

        slime.Status = 0;
        slime.LikePoint = 0;

        SlimeStatus();
    }

    void Update()
    {

    }

    void SlimeStatus()
    {
        for (int i = 0; i < slime.attribute_num; i++)
        {
            slime.attribute[i] = Random.Range(-10f, 10f);
            Debug.Log(itemData[1][i + 13] + ":" + slime.attribute[i]);
        }
    }

    /// <summary>
    /// CSVファイルからアイテム画像を読み込む関数
    /// </summary>
    void ReadFile()
    {
        TextAsset itemList = Resources.Load("itemList") as TextAsset;
        StringReader reader = new StringReader(itemList.text);
        //List<GameObject> items = new List<GameObject>();

        while (reader.Peek() != -1)
        {
            string line = reader.ReadLine();
            itemData.Add(line.Split(','));
        }

        for (int i = 0; i < 271; i++)
        {
            if (itemData[i][3] == "1" && File.Exists("Assets/Resources/Sprites/" + itemData[i][1]))
            {
                GameObject clone = Instantiate(item, parent.transform);
                GameObject e = Instantiate(child, clone.transform);
                e.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/" + itemData[i][1].Replace(".png", ""));
                //items.Add(e);
                EventTrigger eventTrigger = e.AddComponent<EventTrigger>();
                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.EndDrag;
                int j = i + 0;
                entry.callback.AddListener((x) => Give_item(itemData[j][1].Replace(".png", "")));
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

    public void Give_item(string itemName)
    {
        for (int i = 0; i < 271; i++)
        {
            if (itemName == itemData[i][1].Replace(".png", ""))
            {
                for (int j = 12; j < 36; j++)
                {
                    if (itemData[i][j] == "〇")
                    {
                        Debug.Log(itemData[1][j]);
                        if (slime.attribute[j - 12] > 0)
                        {
                            if (slime.attribute[j - 12] == 10)
                            {
                                slime.Status = 100;
                                slime.LikePoint = 10;
                            }
                            else if (slime.attribute[j - 12] > 8)
                            {
                                slime.Status += Random.Range(80, 90);
                                slime.LikePoint += Random.Range(8, 9);
                            }
                            else if (slime.attribute[j - 12] > 6)
                            {
                                slime.Status += Random.Range(60, 80);
                                slime.LikePoint += Random.Range(6, 8);

                            }
                            else if (slime.attribute[j - 12] > 4)
                            {
                                slime.Status += Random.Range(40, 60);
                                slime.LikePoint += Random.Range(4, 6);
                            }
                            else if (slime.attribute[j - 12] > 2)
                            {
                                slime.Status += Random.Range(20, 40);
                                slime.LikePoint += Random.Range(2, 4);
                            }
                            else
                            {
                                slime.Status += Random.Range(0, 20);
                                slime.LikePoint += Random.Range(0, 2);
                            }
                        }
                        else if (slime.attribute[j - 12] < 0)
                        {
                            if (slime.attribute[j - 12] == -10)
                            {
                                slime.Status = -100;
                                slime.LikePoint = -10;
                            }
                            else if (slime.attribute[j - 12] < -8)
                            {
                                slime.Status -= Random.Range(80, 90);
                                slime.LikePoint -= Random.Range(8, 9);
                            }
                            else if (slime.attribute[j - 12] < -6)
                            {
                                slime.Status -= Random.Range(60, 80);
                                slime.LikePoint -= Random.Range(6, 8);
                            }
                            else if (slime.attribute[j - 12] < -4)
                            {
                                slime.Status -= Random.Range(40, 60);
                                slime.LikePoint -= Random.Range(4, 6);
                            }
                            else if (slime.attribute[j - 12] < -2)
                            {
                                slime.Status -= Random.Range(20, 40);
                                slime.LikePoint -= Random.Range(2, 4);
                            }
                            else
                            {
                                slime.Status -= Random.Range(0, 20);
                                slime.LikePoint -= 1;
                            }
                        }
                        else
                        {
                            slime.Status += 0;
                        }
                    }
                }
            }
        }
        Debug.Log(slime.Status + ":" + slime.LikePoint);

        StartCoroutine(Change_face(slime.Status, slime.LikePoint));
    }

    IEnumerator Change_face(float status, float likepoint)
    {
        if (likepoint > 7 && status > 50)
        {
            slime_image.sprite = Resources.Load<Sprite>("Sprites/Slime/" + Emotion.HAPPY_B.ToString());
        }
        else if (likepoint > 5)
        {
            slime_image.sprite = Resources.Load<Sprite>("Sprites/Slime/" + Emotion.HAPPY.ToString());
        }
        else if (likepoint < -7 && status < -50)
        {
            slime_image.sprite = Resources.Load<Sprite>("Sprites/Slime/" + Emotion.ANGRY.ToString());
        }
        else if (likepoint < -5)
        {
            slime_image.sprite = Resources.Load<Sprite>("Sprites/Slime/" + Emotion.SAD.ToString());
        }

        yield return new WaitForSeconds(2.5f);
        slime_image.sprite = Resources.Load<Sprite>("Sprites/Slime/" + Emotion.NOMAL.ToString());
    }
}

/// <summary>
/// スライムの個体値管理クラス
/// </summary>
//[System.Serializable]
public class SlimeStatus
{
    [System.NonSerialized] private float likePoint;  //信頼↔嫌悪の基本となる感情 スライムの好みによって変動する変数
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

    public float LikePoint
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
    //public float red, blue, yellow, green, purple, white, black, bright, foods, fruits,
    //           sweets, toys, hot, cool, hard, soft, curiosity, fancy, sober, round, anglar;

}
