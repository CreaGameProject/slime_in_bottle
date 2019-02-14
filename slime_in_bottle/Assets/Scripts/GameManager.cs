using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject flont_UI, back_UI;
    Color flont_c, back_c; //アイテム画面と棚画面UIの背景色
    int UI_flag = 0; //アイテム画面と棚画面切り替えのフラグ
    [SerializeField] Sprite[] items; //アイテムと瓶のスプライト
    [SerializeField] GameObject[] list; //アイテムと瓶のリスト
    const int item_num = 15; //アイテム数

    void Start()
    {

    }

    void Update()
    {

    }

    /// <summary>
    /// UIの切り替えを行う関数
    /// </summary>
    public void Change_UI()
    {
        flont_c = flont_UI.GetComponent<Image>().color;
        back_c = back_UI.GetComponent<Image>().color;

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
