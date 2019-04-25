using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TouchController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Vector2 prevPos, endPos;
    [SerializeField] GameObject image;
    public static int flag = 0; // スライムのステータスを変動させるかどうかのフラグ

    void Start()
    {

    }

    void Update()
    {

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        prevPos = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        endPos = transform.position;
        transform.position = prevPos;

        var raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);

        foreach (var hit in raycastResults)
        {
            // スライムの上でアイテムが離されたらフラグを1に
            if (hit.gameObject.CompareTag("Slime"))
            {
                flag = 1;
            }
        }
    }
}
