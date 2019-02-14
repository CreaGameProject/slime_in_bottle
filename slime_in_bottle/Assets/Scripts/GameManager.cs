using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject flont_UI, back_UI;
    Color flont_c, back_c;
    int UI_flag = 0;
    [SerializeField] Sprite[] items;
    [SerializeField] GameObject[] list;
    const int item_num = 15;

    void Start()
    {

    }

    void Update()
    {

    }

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
