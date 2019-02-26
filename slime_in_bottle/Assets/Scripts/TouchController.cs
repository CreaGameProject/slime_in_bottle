﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TouchController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    Vector2 prevPos, dragPos, endPos;
    [SerializeField] GameObject image;

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
;       dragPos = eventData.position;
        image.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        endPos = transform.position;
        image.transform.position = prevPos;
    }
}