using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TouchController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Vector2 prevPos, dragPos, endPos;
    [SerializeField] Transform parent;
    [SerializeField] GameObject image;
    [SerializeField] GameObject slimePos;
    [System.NonSerialized] public int eat_flag;

    void Start()
    {
        eat_flag = 0;
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

        dragPos = eventData.position;
        transform.position = dragPos;
        //transform.SetParent(parent);
        //transform.SetAsLastSibling();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        endPos = transform.position;
        transform.position = prevPos;
        if (Vector2.Distance(endPos, slimePos.transform.position) > 1200)
        {
            //Debug.Log("ate!");
        }
    }
}
