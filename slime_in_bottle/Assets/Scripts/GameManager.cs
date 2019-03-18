using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.U2D;

enum Emotion
{
    DISLIKE, NOMAL, TRUST, HAPPY, SAD, ANGRY, HAPPY_B, SURPRISE, TIRED, HOPE, FEAR
}

//enum Form
//{
//    NOMAL, CUBE, SHARPLY, LIQUID, LIMP, ANGLAR
//}

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject parent, frame, item;
    [SerializeField] Image slime_image;
    List<string[]> itemData = new List<string[]>(); //CSVファイルのデータを格納するリスト
    SlimeStatus slime = new SlimeStatus(); //インスタンス作成

    void Start()
    {
        ReadFile();

        slime.Status = 0;
        slime.LikePoint = 0;
        slime.slime_color = slime_image.color;
        SlimeAttribute();
        Debug.Log(slime.slime_color);
    }

    void Update()
    {

    }

    /// <summary>
    /// スライムのステータスを決定する関数
    /// </summary>
    void SlimeAttribute()
    {
        for (int i = 0; i < slime.attribute_num; i++)
        {
            slime.attribute[i] = Random.Range(-10, 10);
            // Debug.Log(itemData[1][i + 13] + ":" + slime.attribute[i]);
        }
    }

    /// <summary>
    /// CSVファイルからアイテム画像を読み込み、アイテムリストを生成する関数
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

        for (int i = 0; i < 271; i++)
        {
            if (itemData[i][3] == "1" && File.Exists("Assets/Resources/Sprites/Items/" + itemData[i][1]))
            {
                GameObject clone = Instantiate(frame, parent.transform);
                GameObject item_image = Instantiate(item, clone.transform);
                item_image.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Items/" + itemData[i][1].Replace(".png", ""));
                EventTrigger eventTrigger = item_image.AddComponent<EventTrigger>();
                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.EndDrag;
                int j = i;
                entry.callback.AddListener((x) => Give_item(itemData[j][1].Replace(".png", "")));
                eventTrigger.triggers.Add(entry);
            }
        }
    }
    
    /// <summary>
    /// アイテムを与えたときのスライムの値の変動を行う関数
    /// </summary>
    /// <param name="itemName">アイテムの名前</param>
    public void Give_item(string itemName)
    {
        for (int i = 0; i < 271; i++)
        {
            if (itemName == itemData[i][1].Replace(".png", ""))
            {
                for (int j = 13; j <= 35; j++)
                {
                    if (itemData[i][j] == "〇")
                    {
                        // Debug.Log(itemData[1][j]);
                        if (slime.attribute[j - 13] > 0)
                        {
                            if (slime.attribute[j - 13] == 10)
                            {
                                slime.Status += 100;
                                slime.LikePoint += 10;
                            }
                            else if (slime.attribute[j - 13] > 8)
                            {
                                slime.Status += Random.Range(80, 100);
                                slime.LikePoint += Random.Range(8, 10);
                            }
                            else if (slime.attribute[j - 13] > 6)
                            {
                                slime.Status += Random.Range(60, 80);
                                slime.LikePoint += Random.Range(6, 8);

                            }
                            else if (slime.attribute[j - 13] > 4)
                            {
                                slime.Status += Random.Range(40, 60);
                                slime.LikePoint += Random.Range(4, 6);
                            }
                            else if (slime.attribute[j - 13] > 2)
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
                        else if (slime.attribute[j - 13] < 0)
                        {
                            if (slime.attribute[j - 13] == -10)
                            {
                                slime.Status -= Random.Range(80, 100);
                                slime.LikePoint -= -10;
                            }
                            else if (slime.attribute[j - 13] < -8)
                            {
                                slime.Status -= Random.Range(60, 80);
                                slime.LikePoint -= Random.Range(6, 8);
                            }
                            else if (slime.attribute[j - 13] < -6)
                            {
                                slime.Status -= Random.Range(40, 60);
                                slime.LikePoint -= Random.Range(4, 6);
                            }
                            else if (slime.attribute[j - 13] < -4)
                            {
                                slime.Status -= Random.Range(20, 40);
                                slime.LikePoint -= Random.Range(2, 4);
                            }
                            else if (slime.attribute[j - 13] < -2)
                            {
                                slime.Status -= Random.Range(0, 20);
                                slime.LikePoint -= Random.Range(0, 2);
                            }
                            else
                            {
                                slime.Status -= Random.Range(0, 10);
                                slime.LikePoint -= Random.Range(0, 2);
                            }
                        }
                        else
                        {
                            slime.Status += 0;
                        }

                        int rand = Random.Range(0, 10);

                        //if (rand > 8)
                        //{
                        for (int n = 13; n <= 19; n++)
                        {
                            if (j == n)
                            {
                                if (itemData[i][4] != "")
                                {
                                    int r = int.Parse(itemData[i][4]);
                                    int g = int.Parse(itemData[i][5]);
                                    int b = int.Parse(itemData[i][6]);

                                    Change_color(r, g, b);
                                    //Debug.Log(r + ":" + g + ":" + b);
                                }
                                else if (itemData[i][7] != "")
                                {
                                    int r = int.Parse(itemData[i][7]);
                                    int g = int.Parse(itemData[i][8]);
                                    int b = int.Parse(itemData[i][9]);

                                    Change_color(r, g, b);
                                    //Debug.Log(r + ":" + g + ":" + b);
                                }
                                else
                                {
                                    Change_color(255, 255, 255);
                                }
                            }
                        }
                        //}
                    }
                }
            }
        }
        // Debug.Log(slime.Status + ":" + slime.LikePoint);

        StartCoroutine(Change_face(slime.Status, slime.LikePoint));
    }

    /// <summary>
    /// スライムの表情を変える関数
    /// </summary>
    /// <param name="status">信頼↔嫌悪の基本となる変数</param>
    /// <param name="likepoint">スライムの好みによって変動する変数</param>
    /// <returns></returns>
    IEnumerator Change_face(float status, float likepoint)
    {
        if (slime.Status >= 75)
        {
            slime_image.sprite = Resources.Load<Sprite>("Sprites/Slime/" + Emotion.TRUST.ToString());
        }
        else if (slime.Status <= -75)
        {
            slime_image.sprite = Resources.Load<Sprite>("Sprites/Slime/" + Emotion.DISLIKE.ToString());
        }
        else
        {
            slime_image.sprite = Resources.Load<Sprite>("Sprites/Slime/" + Emotion.NOMAL.ToString());
        }

        if (likepoint == 10 && status == 100)
        {
            slime_image.sprite = Resources.Load<Sprite>("Sprites/Slime/" + Emotion.HOPE.ToString());
        }
        else if (likepoint > 7 && status > 50)
        {
            slime_image.sprite = Resources.Load<Sprite>("Sprites/Slime/" + Emotion.HAPPY_B.ToString());
        }
        else if (likepoint > 5)
        {
            slime_image.sprite = Resources.Load<Sprite>("Sprites/Slime/" + Emotion.HAPPY.ToString());
        }
        else if (likepoint == -10 && status == -100)
        {
            slime_image.sprite = Resources.Load<Sprite>("Sprites/Slime/" + Emotion.FEAR.ToString());
        }
        else if (likepoint < -7 && status < -50)
        {
            slime_image.sprite = Resources.Load<Sprite>("Sprites/Slime/" + Emotion.ANGRY.ToString());
        }
        else if (likepoint < -5)
        {
            slime_image.sprite = Resources.Load<Sprite>("Sprites/Slime/" + Emotion.SAD.ToString());
        }

        yield return new WaitForSeconds(2);
        if (slime.Status >= 75)
        {
            slime_image.sprite = Resources.Load<Sprite>("Sprites/Slime/" + Emotion.TRUST.ToString());
        }
        else if (slime.Status <= -75)
        {
            slime_image.sprite = Resources.Load<Sprite>("Sprites/Slime/" + Emotion.DISLIKE.ToString());
        }
        else
        {
            slime_image.sprite = Resources.Load<Sprite>("Sprites/Slime/" + Emotion.NOMAL.ToString());
        }
    }

    /// <summary>
    ///  スライムの色を変える関数
    /// </summary>
    /// <param name="r">R値</param>
    /// <param name="g">G値</param>
    /// <param name="b">B値</param>
    void Change_color(int r, int g, int b)
    {
        slime_image.color = new Color(r, g, b);
        Debug.Log(slime_image.color);
    }
}

/// <summary>
/// スライムの個体値管理クラス
/// </summary>
// [System.Serializable]
public class SlimeStatus
{
    [System.NonSerialized] public int status; // 信頼↔嫌悪の基本となる感情の変数
    [System.NonSerialized] public int likePoint;  // スライムの好みによって変動する変数
    [System.NonSerialized] public int attribute_num = 23;
    [System.NonSerialized] public Color slime_color;
    [System.NonSerialized]
    public int[] attribute = new int[23];

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
}
