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
    DISLIKE, NORMAL, TRUST, HAPPY, SAD, ANGRY, HAPPY_B, SURPRISE, TIRED, HOPE, FEAR
}

enum Form
{
    NORMAL, CUBE, SHARPLY, LIQUID, LIMP, ANGLAR
}

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject parent, frame, item, makeName, back, done; // UI関連のオブジェクト
    [SerializeField] Text name, new_name; // スライムの今の名前と新しい名前
    [SerializeField] InputField input_name; // 入力文字
    [SerializeField] Image slime_image; // スライムの画像
    List<string[]> itemData = new List<string[]>(); //CSVファイルのデータを格納するリスト
    SlimeStatus slime = new SlimeStatus(); //インスタンス作成
    int flag = 0; // UIの表示フラグ
    const int offset = 13; // CSVの属性データの位置

    [SerializeField] int const_num = 5; // statusの変動を決める変数 Inspectorから調整できる

    void Start()
    {
        ReadFile();
        name.text = slime.name;
        slime.slime_color = slime_image.color;
        SlimeAttribute();
    }

    void Update()
    {

    }

    /// <summary>
    /// slime.attribute[]を決定する関数
    /// </summary>
    void SlimeAttribute()
    {
        for (int i = 0; i < slime.attribute_num; i++)
        {
            slime.attribute[i] = Random.Range(-10, 10);
            Debug.Log(itemData[1][i + offset] + ":" + slime.attribute[i]);
        }
    }

    /// <summary>
    /// CSVファイルからアイテム画像を読み込み、アイテムリストを生成する関数
    /// </summary>
    void ReadFile()
    {
        #region
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

                // アイテムひとつひとつにイベントをつける
                EventTrigger eventTrigger = item_image.AddComponent<EventTrigger>();
                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.EndDrag;
                int j = i;
                entry.callback.AddListener((x) => Give_item(itemData[j][1].Replace(".png", "")));
                eventTrigger.triggers.Add(entry);
            }
        }
        #endregion
    }

    /// <summary>
    /// アイテムを与えたときのスライムの値の変動を行う関数
    /// </summary>
    /// <param name="itemName">アイテムの名前</param>
    public void Give_item(string itemName)
    {
        #region
        // フラグが1なら（スライムの上でアイテムが離されたらなら）ステータス変動を行う
        if (TouchController.flag == 1)
        {
            int attribute_sum = 0; // 好き嫌いの合計値

            // 与えられたアイテムとCSVデータを照らし合わせるためのfor分
            // 271=CSVデータの最終行
            for (int i = 0; i < 271; i++)
            {
                if (itemName == itemData[i][1].Replace(".png", ""))
                {
                    // アイテムの属性を見るためのfor文
                    // 35=CSVデータの属性の最終列
                    for (int j = offset; j <= 35; j++)
                    {
                        if (itemData[i][j] == "〇")
                        {
                            // 好き嫌いの合計値を足していく
                            attribute_sum += slime.attribute[j - offset];

                            // 1/5の確率でスライムの色を変更
                            int rand = Random.Range(0, 5);

                            if (rand == 0)
                            {
                                if (itemData[i][4] != "")
                                {
                                    int r = int.Parse(itemData[i][4]);
                                    int g = int.Parse(itemData[i][5]);
                                    int b = int.Parse(itemData[i][6]);

                                    Change_color(r, g, b);
                                }
                                else if (itemData[i][7] != "")
                                {
                                    int r = int.Parse(itemData[i][7]);
                                    int g = int.Parse(itemData[i][8]);
                                    int b = int.Parse(itemData[i][9]);

                                    Change_color(r, g, b);
                                }
                                else
                                {
                                    Change_color(255, 255, 255);
                                }
                            }
                        }
                    }
                }
            }

            // Debug.Log("好み:" + attribute_sum);
            slime.Status += attribute_sum * const_num;
            StartCoroutine(Change_face(slime.Status, attribute_sum));
            TouchController.flag = 0;
        }
        #endregion
    }

    /// <summary>
    /// スライムの表情を変更する関数
    /// </summary>
    /// <param name="status">スライムのステータス</param>
    /// <param name="attribute_sum">好き嫌い値の合計（＝アイテムの好き嫌い）</param>
    /// <returns></returns>
    IEnumerator Change_face(int status, int attribute_sum)
    {
        #region
        Emotion emotion = Emotion.NORMAL;

        if (status > 50 && attribute_sum > 5)
        {
            emotion = Emotion.HAPPY_B;
        }
        else if (status < -50 && attribute_sum < -5)
        {
            emotion = Emotion.ANGRY;
        }
        else if (attribute_sum > 3)
        {
            emotion = Emotion.HAPPY;
        }
        else if (attribute_sum < -3)
        {
            emotion = Emotion.SAD;
        }

        slime_image.sprite = Resources.Load<Sprite>("Sprites/Slime/" + emotion.ToString());

        yield return new WaitForSeconds(2);

        if (status >= 75)
        {
            emotion = Emotion.TRUST;
        }
        else if (status <= -75)
        {
            emotion = Emotion.DISLIKE;
        }
        else
        {
            emotion = Emotion.NORMAL;
        }

        slime_image.sprite = Resources.Load<Sprite>("Sprites/Slime/" + emotion.ToString());
        #endregion
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
    }

    /// <summary>
    /// 名前変更のUI表示切り替え
    /// </summary>
    public void Slime_name_UI()
    {
        flag = 1 - flag;

        if (flag == 1)
        {
            makeName.SetActive(true);
        }
        else
        {
            input_name.text = "";
            makeName.SetActive(false);
        }
    }

    /// <summary>
    /// スライムの名前を変更する関数
    /// </summary>
    public void Change_slime_name()
    {
        if (new_name.text.Trim() != "")
        {
            slime.name = new_name.text.Trim();
            name.text = slime.name;
            input_name.text = "";
        }
    }
}

/// <summary>
/// スライムの個体値管理クラス
/// </summary>
public class SlimeStatus
{
    public string name; // スライムの名前
    public int status; // 信頼↔嫌悪の基本となる感情の変数
    public Color slime_color; // スライムの色情報
    public int attribute_num { get; private set; }　// アイテム属性の種類数
    public int[] attribute { get; private set; }　// 属性ごとの好き嫌い値

    public SlimeStatus()
    {
        status = 0;
        name = "すらいむくん";
        attribute_num = 23;
        attribute = new int[attribute_num];
    }

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
            Debug.Log("status:" + status);
        }
    }
}
