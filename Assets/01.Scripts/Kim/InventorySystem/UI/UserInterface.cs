using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Linq;
using System;
using AYellowpaper.SerializedCollections;
using DG.Tweening;
using Unity.VisualScripting;

public enum InterfaceType
{
    Weapon,
    SkillAndBuff,
    Equipment
}

public abstract class UserInterface : MonoBehaviour
{
    public InterfaceType interfaceType; // UI 타입을 저장하는 변수

    // 인벤토리 데이터 객체, 슬롯 정보를 포함
    public InventoryObject inventory;

    // 슬롯 UI(GameObject)와 인벤토리 데이터(InventorySlot)를 매핑하는 딕셔너리
    public SerializedDictionary<GameObject, InventorySlot> slotsOnInterface =
        new SerializedDictionary<GameObject, InventorySlot>();

    private bool isMovedUp = false; // 슬롯이 위로 올라가 있는 상태인지 체크
    private HorizontalLayoutGroup layoutGroup; // HorizontalLayoutGroup 참조
    private float originalSpacing;
    private static bool isInitialized = false;

    public void Start()
    {
        // 인벤토리 슬롯 초기화
        for (int i = 0; i < inventory.GetSlots.Length; i++)
        {
            inventory.GetSlots[i].parent = this; // 슬롯의 부모를 현재 UserInterface로 설정
            // inventory.GetSlots[i].onAfterUpdated += OnSlotUpdate; // 슬롯 데이터 변경 시 호출될 이벤트 등록
            Debug.Log($"Registered OnSlotUpdate for slot: {inventory.GetSlots[i].card.Name}");
        }

        // 레이아웃그룹 
        layoutGroup = GetComponent<HorizontalLayoutGroup>(); // layoutGroup 참조 가져오기
        if (layoutGroup != null)
        {
            originalSpacing = layoutGroup.spacing; // 초기 spacing 값 저장
        }
        else
        {
            Debug.LogError("HorizontalLayoutGroup 컴포넌트가 Content Panel에 없습니다!");
        }
        
        CreateSlots(); // 슬롯 생성

        Debug.Log("UserInterface_Start실행 : " + this.interfaceType);

        // 현재 UI에서 마우스가 들어오거나 나갈 때의 이벤트를 등록
        // AddEvent(gameObject, EventTriggerType.PointerEnter, delegate { OnEnterInterface(gameObject); });
        // AddEvent(gameObject, EventTriggerType.PointerExit, delegate { OnExitInterface(gameObject); });
    }

    // 매개변수의 slot으로 ui가 업데이트가 되어야하는 것.. 
    public void OnSlotUpdate(InventorySlot slot)
    {
        Debug.Log($"OnSlotUpdate 실행됨. 슬롯 : {slot.card.Name}");

        var slotDisplay = slot.targetObject; // 슬롯의 UI GameObject를 가져옵니다.

        if (slot.card.Id <= -1) // 슬롯에 카드 데이터가 없을 경우, 즉 삭제되어야할 카드라면
        {
            // 프리팹을 제거
            var tempObject = slotDisplay.gameObject;
            if (tempObject != null && tempObject.activeSelf)
            {
                Debug.Log($"프리팹제거, : {tempObject.name}");
                Destroy(tempObject); // 카드가 없는 경우 해당 UI 객체를 제거
                slotsOnInterface.Remove(tempObject);
            }
        }
        else // 슬롯에 유효한 카드 데이터가 있는 경우
        {
            GameObject tempPrefab = slot.GetItemObject().characterPrefab;
            if (tempPrefab != null)
            {
                if (slotDisplay == null)
                {
                    slotDisplay = tempPrefab;
                }
                
                if (slotDisplay.transform.childCount > 0)
                {
                    // Empty Slot이라면.. 하위오브젝트를 생성, 그게 아니라면 삭제하고 바꿔치기를 해야함. 
                    if (slotDisplay.transform.childCount == 1)
                    {
                        Transform existingItem = slotDisplay.transform.GetChild(0);
                        if (existingItem != null)
                        {
                            Destroy(existingItem.gameObject); // 기존의 객체 제거

                            GameObject newItem = Instantiate(tempPrefab, slotDisplay.transform);
                            newItem.transform.localPosition = Vector3.zero; // 위치 초기화
                            Debug.Log($"엠티슬롯 : {tempPrefab.name}");
                            newItem.SetActive(true); // 활성화
                        }
                    }
                    else
                    {
                        slot.onAfterUpdated -= OnSlotUpdate;  // 이미 구독되어 있으면 제거
                        slot.onAfterUpdated += OnSlotUpdate;  // 새로 구독
                        
                        // // // 부모 슬롯에 새로운 아이템 추가.. 
                        GameObject newItem = Instantiate(tempPrefab, slotDisplay.transform.parent);
                        newItem.SetActive(true); // 활성화
                        // 슬롯 을 추가하면서 인벤토리.getslot을 하거나 해야할듯

                        // 이큅먼트에서 슬롯을 찾아 인벤토리에 넣기?
                        // 현재 인벤이 이큅먼트인지..를 봐야함.. 
                        InventorySlot findSlot = inventory.FindItemOnInventory(slot.card);
                        //
                        AddEvent(newItem, EventTriggerType.PointerEnter, delegate { OnEnter(newItem); });
                        AddEvent(newItem, EventTriggerType.PointerExit, delegate { OnExit(newItem); });
                        AddEvent(newItem, EventTriggerType.BeginDrag, delegate { OnDragStart(newItem); });
                        AddEvent(newItem, EventTriggerType.EndDrag, delegate { OnDragEnd(newItem); });
                        AddEvent(newItem, EventTriggerType.Drag, delegate { OnDrag(newItem); });
                        // 클릭 이벤트 추가
                        AddEvent(newItem, EventTriggerType.PointerClick, delegate { OnClick(newItem); });
                        
                        // findSlot.targetObject = newItem;
                        // 타겟 오브젝트 설정..?
                        findSlot.targetObject = newItem;
                        slotsOnInterface.Add(newItem, findSlot);

                        Debug.Log($"엠티가 아닌 인벤슬롯 : {tempPrefab.name}");
                    }
                }
            }
            else
            {
                // 프리팹이 없다면 여기에서 에러 처리를 할 수 있습니다. 예: 로그 출력
                Debug.LogWarning("슬롯에 배치할 카드 프리팹이 없습니다.");
            }
        }

        // if (slot.card.Id <= -1) // 슬롯에 카드 데이터가 없을 경우
        // {
        //     var image = slot.slotDisplay.GetComponent<Image>(); // .transform.GetChild(0).GetComponent<Image>(); // 이미지 컴포넌트 가져오기
        //     image.sprite = null; // 슬롯의 이미지를 제거
        //     image.color = new Color(1, 1, 1, 0); // 이미지 색상을 투명으로 설정
        //     
        //     Debug.Log("슬롯에 카드 데이터가 없어서 이미지를 제거 null");
        // }
        // else // 슬롯에 유효한 카드 데이터가 있는 경우
        // {
        //     // 바뀌어야할 카드 이미지.. 
        //     var image = slot.slotDisplay.GetComponent<Image>(); // slot.slotDisplay.transform.GetChild(0).GetComponent<Image>(); // 이미지 컴포넌트 가져오기
        //     image.sprite = slot.GetItemObject().uiDisplay; // 슬롯에 표시할 아이템의 이미지를 설정
        //     image.color = new Color(1, 1, 1, 1); // 이미지 색상을 불투명으로 설정
        //     
        //     Debug.Log($"슬롯에 데이터가 있음!! {slot.card.Name}");
        //
        // }
    }

    // 슬롯을 생성하는 추상 메서드, 상속받은 클래스에서 구현해야 함
    public abstract void CreateSlots();

    // 특정 GameObject에 이벤트를 추가하는 메서드
    protected void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        // hyuna
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = obj.AddComponent<EventTrigger>();
        }

        var eventTrigger = new EventTrigger.Entry
        {
            eventID = type
        };

        eventTrigger.callback.AddListener(data =>
        {
            action.Invoke(data); // 사용자 정의 동작 호출

            // 이벤트 전파 허용 (이벤트 소모 방지)
            if (data is PointerEventData pointerEventData)
            {
                pointerEventData.useDragThreshold = false;
            }
        });

        trigger.triggers.Add(eventTrigger);
    }

    // 마우스가 특정 슬롯에 들어갔을 때 호출
    public void OnEnter(GameObject obj)
    {
        MouseData.slotHoveredOver = obj; // 현재 마우스가 위치한 슬롯을 기록
        Debug.Log($"OnEnter: {obj.name}");
    }

    // 마우스가 특정 슬롯에 클릭
    public void OnClick(GameObject obj)
    {
        Debug.Log($"OnClick called on {obj.name}");

        // if (obj != null)
        // {
        //     // 현재 강조된 슬롯이 있고, 클릭된 슬롯이 다른 슬롯인 경우 강조 해제
        //     currentFocusedSlot.ResetSlot();
        // }

        if (isMovedUp)
        {
            // 슬롯이 이미 올라가 있는 경우: 원위치로 복구
            ResetSlot(obj);
        }
        else
        {
            // 슬롯이 원래 위치에 있는 경우: 위로 이동
            FocusSlot(obj);
        }
    }

    private void FocusSlot(GameObject obj)
    {
        Debug.Log($"Focusing slot {obj.name}");

        isMovedUp = true;

        // Horizontal Layout Group의 spacing을 부드럽게 60으로 증가
        DOTween.To(() => layoutGroup.spacing, x => layoutGroup.spacing = x, 60f, 0.3f).SetEase(Ease.OutQuad);
    }

    private void ResetSlot(GameObject obj)
    {
        isMovedUp = false;

        // Horizontal Layout Group의 spacing을 부드럽게 원래 값(-10)으로 복구
        DOTween.To(() => layoutGroup.spacing, x => layoutGroup.spacing = x, originalSpacing, 0.3f)
            .SetEase(Ease.OutQuad);
    }


    // 마우스가 UI 전체에 들어갔을 때 호출
    public void OnEnterInterface(GameObject obj)
    {
        MouseData.interfaceMouseIsOver = obj.GetComponent<UserInterface>(); // 마우스가 위치한 인터페이스를 기록
    }

    // 마우스가 UI에서 나갔을 때 호출
    public void OnExitInterface(GameObject obj)
    {
        MouseData.interfaceMouseIsOver = null; // 마우스가 더 이상 인터페이스 위에 있지 않음을 기록
    }

    // 마우스가 특정 슬롯에서 나갔을 때 호출
    public void OnExit(GameObject obj)
    {
        MouseData.slotHoveredOver = null; // 마우스가 슬롯 위에 있지 않음을 기록
        Debug.Log($"OnExit: {obj.name}");
    }

    // 드래그가 시작되었을 때 호출
    public void OnDragStart(GameObject obj)
    {
        Debug.Log($"OnBeginDrag called on {obj.name}");
        
        if (this.interfaceType == InterfaceType.Equipment) return;

        CreateTempItem(obj);
        
        // 241120 hyuna
        SoundManager.Instance.PlayUISound(UISoundType.Pop);
        // MouseData.tempItemBeingDragged = CreateTempItem(obj); // 드래그 중 표시할 임시 아이템 생성
    }

    // 드래그 중 호출
    public void OnDrag(GameObject obj)
    {
        Debug.Log($"OnDrag called on {obj.name}");

        if (MouseData.tempItemBeingDragged != null)
        {
            // RectTransform을 가져옴
            RectTransform tempTransform = MouseData.tempItemBeingDragged.GetComponent<RectTransform>();

            // 현재 마우스 위치를 월드 좌표로 변환
            Vector3 worldPosition;
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(
                    tempTransform, // RectTransform 기준
                    Input.mousePosition, // 현재 마우스 위치
                    Camera.main, // 카메라
                    out worldPosition)) // 변환된 월드 좌표를 출력
            {
                tempTransform.position = worldPosition; // 변환된 월드 좌표를 설정
            }
            else
            {
                Debug.LogError("Failed to convert mouse position to world position.");
            }
        }
    }

    // 드래그가 끝났을 때 호출
    public void OnDragEnd(GameObject obj)
    {
        // 임시 아이템 삭제
        Destroy(MouseData.tempItemBeingDragged);

        // 마우스 위치를 UI의 월드 좌표로 변환
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // PointerEventData 설정
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition // 마우스 화면 좌표
        };

        // 레이캐스트가 유효한 UI 요소와 충돌하는지 확인
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);

        // 결과가 없으면, 레이캐스트 실패로 간주
        if (raycastResults.Count == 0)
        {
            Debug.Log("유효한 슬롯을 찾을 수 없음. 드래그 취소");
            Destroy(MouseData.tempItemBeingDragged); // 임시 아이템 제거
            return;
        }

        bool isValidSlotFound = false;

        // 모든 레이캐스트 결과를 확인
        foreach (RaycastResult result in raycastResults)
        {
            GameObject hitObject = result.gameObject;
            Debug.Log("레이캐스트 되는 거 : " + hitObject.name);

            // 슬롯에 해당하는 UI 요소인지 확인
            if (hitObject.CompareTag("Slot"))
            {
                isValidSlotFound = true;

                // 슬롯의 부모를 거슬러 올라가면서 UserInterface를 찾음
                Transform parentTransform = hitObject.transform;
                UserInterface targetUI = null;

                // 부모를 계속 따라 올라가며 UserInterface를 찾음
                while (parentTransform != null)
                {
                    targetUI = parentTransform.GetComponent<UserInterface>();
                    if (targetUI != null)
                    {
                        break; // UserInterface를 찾았으면 루프 종료
                    }

                    parentTransform = parentTransform.parent; // 부모로 이동
                }

                // 동일한 인터페이스끼리는 X
                if (targetUI.interfaceType != this.interfaceType)
                {
                    // 슬롯 간 아이템 이동 처리 (여기서 실제 이동하는 코드를 추가해야 합니다)
                    InventorySlot sourceSlot = slotsOnInterface[obj];

                    Transform targetUserInterface = targetUI.transform;
                    var targetSlotsOnInterface = targetUserInterface.GetComponent<UserInterface>().slotsOnInterface;
                    
                    InventorySlot targetSlot = targetSlotsOnInterface[hitObject.transform.parent.gameObject];

                    // 웨폰, 스킬앤버프 => 타겟의 인터페이스 타입이 equip이면
                    if (targetUI.interfaceType == InterfaceType.Equipment)
                    {
                        // 장착(스왑)
                        inventory.SwapItems(sourceSlot, targetSlot);
                        Debug.Log($"아이템을 {obj.name}에서 {hitObject.name} 슬롯으로 이동");
                        
                        SoundManager.Instance.PlayUISound(UISoundType.OK);
                    }
                }
                
                break;
            }
        }

        // 유효한 슬롯을 찾지 못했다면 취소
        if (!isValidSlotFound)
        {
            Debug.Log("유효한 슬롯에 드롭되지 않음. 드래그 취소");
            Destroy(MouseData.tempItemBeingDragged); // 임시 아이템 제거
            
            SoundManager.Instance.PlayUISound(UISoundType.Cancel);
        }

        // if (MouseData.slotHoveredOver != null)
        // {
        //     Debug.Log($"Transferring item from {obj.name} to {MouseData.slotHoveredOver.name}");
        //     // 슬롯 간 아이템을 이동시키는 코드 실행
        //     // // 슬롯 간 아이템을 스왑
        //     // InventorySlot mouseHoverSlotData = MouseData.interfaceMouseIsOver.slotsOnInterface[MouseData.slotHoveredOver];
        //     // inventory.SwapItems(slotsOnInterface[obj], mouseHoverSlotData);
        //
        // }
        // else
        // {
        //     Debug.Log("No valid slot to transfer the item.");
        // }


        // Destroy(MouseData.tempItemBeingDragged); // 임시 아이템 제거

        // if (MouseData.interfaceMouseIsOver == null) // 마우스가 UI 외부에 있는 경우
        // {
        //     Debug.Log("Dropped outside valid slots. Cancelling drag.");
        //
        //     slotsOnInterface[obj].RemoveItem(); // 드래그 중인 아이템 제거
        //     return;
        // }
    }

    // // 임시 아이템을 생성하는 메서드 (현재 사용되지 않음)
    // private GameObject CreateTempItem(GameObject obj)
    // {
    //     GameObject tempItem = null;
    //
    //     if (slotsOnInterface[obj].card.Id >= 0) // 유효한 아이템이 있는 경우
    //     {
    //         tempItem = new GameObject(); // 새 게임 오브젝트 생성
    //         var rt = tempItem.AddComponent<RectTransform>(); // RectTransform 추가
    //         rt.sizeDelta = new Vector2(90, 140); // 임시 아이템의 크기 설정
    //         tempItem.transform.SetParent(transform.parent.parent); // 부모 오브젝트 설정
    //         var img = tempItem.AddComponent<Image>(); // 이미지 컴포넌트 추가
    //         img.sprite = slotsOnInterface[obj].GetItemObject().uiDisplay; // 슬롯의 아이템 이미지를 설정
    //         img.raycastTarget = false; // 임시 아이템은 마우스 이벤트를 받지 않도록 설정
    //     }
    //     return tempItem; // 생성된 임시 아이템 반환
    // }

    private void CreateTempItem(GameObject obj)
    {
        if (MouseData.tempItemBeingDragged == null)
        {
            Debug.Log("Creating temp item by cloning the original slot.");

            // 클릭된 슬롯(GameObject)을 복사
            GameObject tempItem = Instantiate(obj);

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
            //tempTransform.sizeDelta = new Vector2(210, 280);
            tempTransform.localScale = new Vector3(1f, 1f, 1f);

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
    
    private void OnEnable()
    {
        if (this.interfaceType == InterfaceType.Equipment) return;
        CreateSlots();
    }

    private void OnDisable()
    {
        if (this.interfaceType == InterfaceType.Equipment) return;
        
        // slotsOnInterface의 모든 키(GameObject)를 순회
        foreach (var kvp in slotsOnInterface.ToList()) // ToList로 복사본을 순회
        {
            var slotGameObject = kvp.Key;
            var inventorySlot = kvp.Value;

            if (slotGameObject != null) // GameObject가 null이 아닌 경우만 처리
            {
                Destroy(slotGameObject); // GameObject 파괴
            }

            if (inventorySlot != null) // InventorySlot이 null이 아닌 경우
            {
                inventorySlot.onAfterUpdated -= OnSlotUpdate; // 델리게이트 해제
            }
        }
        
        slotsOnInterface.Clear(); // 매핑된 슬롯 데이터도 초기화
    }
} // end class