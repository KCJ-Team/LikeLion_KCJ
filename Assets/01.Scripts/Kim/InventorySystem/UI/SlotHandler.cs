using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotHandler : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler
{
    // 인벤토리 데이터 객체, 슬롯 정보를 포함
    public InventoryObject inventory;

    // 슬롯 UI(GameObject)와 인벤토리 데이터(InventorySlot)를 매핑하는 딕셔너리
    public SerializedDictionary<GameObject, InventorySlot> slotsOnInterface = new SerializedDictionary<GameObject, InventorySlot>();
    
    private ScrollRect parentScrollRect;
    private RectTransform slotTransform;
    private HorizontalLayoutGroup layoutGroup;
    private Image slotImage;

    private static SlotHandler currentFocusedSlot; // 현재 강조된 슬롯을 추적 (static)
    private bool isMovedUp = false; // 슬롯이 위로 올라가 있는 상태인지 체크

    private float originalSpacing;
    
    private void Start()
    {
        parentScrollRect = GetComponentInParent<ScrollRect>(); // ScrollRect 참조
        slotTransform = GetComponent<RectTransform>();        // 슬롯의 RectTransform
        layoutGroup = parentScrollRect?.content.GetComponent<HorizontalLayoutGroup>(); // Horizontal Layout Group 참조
        originalSpacing = (layoutGroup != null) ? layoutGroup.spacing : 0.0f;                // Layout Group의 원래 spacing 값 저장

        slotImage = GetComponent<Image>();  
    }
    
    // public void OnPointerEnter(PointerEventData eventData)
    // {
    //     MouseData.slotHoveredOver = gameObject;
    //     Debug.Log($"Mouse entered {gameObject.name}");
    // }
    //
    // public void OnPointerExit(PointerEventData eventData)
    // {
    //     if (MouseData.slotHoveredOver == gameObject)
    //     {
    //         MouseData.slotHoveredOver = null;
    //         Debug.Log($"Mouse exited {gameObject.name}");
    //     }
    // }

    // 드래깅을 시작했을때. 
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log($"OnBeginDrag called on {gameObject.name}");
        
        // 다른 슬롯을 클릭하거나 드래그 시작 시 강조된 슬롯을 초기화
        ResetCurrentFocusedSlot();
        
        // // 드래그 중 표시할 임시 아이템 생성
        CreateTempItem(); // 드래그 시작 시 임시 아이템 생성
        
        // 드래그 시작 시 ScrollRect에 드래그 전달
        parentScrollRect.OnBeginDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log($"OnDrag called on {gameObject.name}");
        
        if (MouseData.tempItemBeingDragged != null)
        {
            RectTransform tempTransform = MouseData.tempItemBeingDragged.GetComponent<RectTransform>();
        
            // 화면 좌표(Screen Space)를 월드 좌표(World Space)로 변환
            Vector3 worldPosition;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(
                tempTransform, // RectTransform 기준
                eventData.position, // Screen Space의 현재 마우스 위치
                eventData.pressEventCamera, // 사용 중인 카메라
                out worldPosition // 변환된 월드 좌표
            );
        
            // 월드 좌표를 RectTransform의 위치로 설정
            tempTransform.position = worldPosition;
        }
        
        // 드래그 중에는 ScrollRect의 스크롤 처리
       parentScrollRect.OnDrag(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
       
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"OnPointerClick called on {gameObject.name}");
        
        if (currentFocusedSlot != null && currentFocusedSlot != this)
        {
            // 현재 강조된 슬롯이 있고, 클릭된 슬롯이 다른 슬롯인 경우 강조 해제
            currentFocusedSlot.ResetSlot();
        }

        if (isMovedUp)
        {
            // 슬롯이 이미 올라가 있는 경우: 원위치로 복구
            ResetSlot();
        }
        else
        {
            // 슬롯이 원래 위치에 있는 경우: 위로 이동
            FocusSlot();
        }
    }
    
    private void FocusSlot()
    {
        Debug.Log($"Focusing slot {gameObject.name}");
        
        isMovedUp = true;
        currentFocusedSlot = this; // 현재 강조된 슬롯으로 설정

        // Horizontal Layout Group의 spacing을 부드럽게 60으로 증가
        DOTween.To(() => layoutGroup.spacing, x => layoutGroup.spacing = x, 60f, 0.3f).SetEase(Ease.OutQuad);
        //
        // // 슬롯 위치를 살짝 위로 이동
        // slotTransform.DOAnchorPos(slotTransform.anchoredPosition + new Vector2(0, 50f), 0.3f).SetEase(Ease.OutQuad);
        //
        // ScrollRect 스크롤 멈춤 처리 (옵션)
        parentScrollRect.velocity = Vector2.zero;
    }

    private void ResetSlot()
    {
        isMovedUp = false;

        // Horizontal Layout Group의 spacing을 부드럽게 원래 값(-10)으로 복구
        DOTween.To(() => layoutGroup.spacing, x => layoutGroup.spacing = x, originalSpacing, 0.3f).SetEase(Ease.OutQuad);

        // // 슬롯 위치를 원래 위치로 복구
        // slotTransform.DOAnchorPos(slotTransform.anchoredPosition - new Vector2(0, 50f), 0.3f).SetEase(Ease.OutQuad);
        //
        if (currentFocusedSlot == this)
        {
            currentFocusedSlot = null; // 강조된 슬롯 해제
        }
    }
    
    private static void ResetCurrentFocusedSlot()
    {
        if (currentFocusedSlot != null)
        {
            currentFocusedSlot.ResetSlot(); // 현재 강조된 슬롯 초기화
        }
    }
    
    private void CreateTempItem()
    {
        if (MouseData.tempItemBeingDragged == null)
        {
            Debug.Log("Creating temp item by cloning the original slot.");

            // 클릭된 슬롯(GameObject)을 복사
            GameObject tempItem = Instantiate(gameObject);

            // 부모를 최상위 Canvas로 설정
            Canvas rootCanvas = GetComponentInParent<Canvas>();
            if (rootCanvas != null)
            {
                tempItem.transform.SetParent(rootCanvas.transform, false);
            }
            else
            {
                tempItem.transform.SetParent(transform.parent.parent, false);
            }

            // RectTransform 설정
            RectTransform originalTransform = GetComponent<RectTransform>();
            RectTransform tempTransform = tempItem.GetComponent<RectTransform>();

            // 원본 슬롯의 World Position 복사
            tempTransform.position = originalTransform.position;

            // 크기를 고정 (133x178)
            tempTransform.sizeDelta = new Vector2(133, 178);

            // `Horizontal Layout Group`의 영향을 받지 않도록 LayoutElement 추가
            LayoutElement layoutElement = tempItem.AddComponent<LayoutElement>();
            layoutElement.ignoreLayout = true;

            // 복사본의 이미지 설정
            Image tempImage = tempItem.GetComponent<Image>();
            if (tempImage != null)
            {
                tempImage.raycastTarget = false; // 임시 아이템은 마우스 이벤트를 받지 않음
            }

            // 복사본의 SlotHandler를 제거하여 이벤트 처리 방지
            Destroy(tempItem.GetComponent<SlotHandler>());

            // MouseData에 저장
            MouseData.tempItemBeingDragged = tempItem;

            Debug.Log($"Temp item created at position: {tempTransform.position}, size: {tempTransform.sizeDelta}");
        }
    }

    
} // end class