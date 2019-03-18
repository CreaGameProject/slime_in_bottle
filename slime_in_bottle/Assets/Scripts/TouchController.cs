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
    [SerializeField] Vector2 slimePos;

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
        Debug.Log(Vector2.Distance(endPos, slimePos));
        if (Vector2.Distance(endPos, slimePos) < 1200 && Vector2.Distance(endPos, slimePos) > 850)
        {
            Debug.Log("ate!");
        }
    }
}
