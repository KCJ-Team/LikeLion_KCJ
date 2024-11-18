using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollHandler : MonoBehaviour, IDragHandler, IBeginDragHandler
{
    private ScrollRect parentScrollRect;

    private void Start()
    {
        // 상위 ScrollRect 찾기
        parentScrollRect = GetComponentInParent<ScrollRect>();
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        // 드래그 시작 시 ScrollRect에 드래그 전달
        parentScrollRect.OnBeginDrag(eventData);
    }


    public void OnDrag(PointerEventData eventData)
    {
        if (parentScrollRect != null)
        {
            // 상위 ScrollRect에 드래그 이벤트 전달
            parentScrollRect.OnDrag(eventData);
        }
    }
}