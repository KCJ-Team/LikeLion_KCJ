using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.EventSystems;

public class StaticInterface : UserInterface
{
    public GameObject[] slots;
    public GameObject contentPanel;
    //private List<GameObject> instantiatedSlots = new List<GameObject>();

    public GameObject emptySlotPrefab;
    
    public override void CreateSlots()
    {
        slotsOnInterface = new SerializedDictionary<GameObject, InventorySlot>();
        
        // // 기존에 생성된 슬롯들을 모두 제거
        // foreach (var slot in instantiatedSlots)
        // {
        //     Destroy(slot);
        // }
        // instantiatedSlots.Clear();
        
        for (int i = 0; i < inventory.GetSlots.Length; i++)
        {
            // 여기에서 초기화 작업?? 
            inventory.GetSlots[i].onAfterUpdated += OnSlotUpdate; // 슬롯 데이터 변경 시 호출될 이벤트

            // var obj = slots[i];
            var obj = CreateSlotObj(i);

            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragStart(obj); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });
            AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });
            
            inventory.GetSlots[i].targetObject = obj;
            slotsOnInterface.Add(obj, inventory.GetSlots[i]);
            
            // 생성된 슬롯을 리스트에 추가
         //   instantiatedSlots.Add(obj);
        }
    }
    
    public GameObject CreateSlotObj(int slotIndex)
    {
        // 슬롯이 비어있는지 체크
        InventorySlot currentSlot = inventory.GetSlots[slotIndex];
        
        // 먼저 빈 슬롯을 생성
        GameObject slotObj = Instantiate(emptySlotPrefab, contentPanel.transform);
        slotObj.SetActive(true); // 빈 슬롯을 활성화

        if (currentSlot.GetItemObject() == null || currentSlot.GetItemObject().characterPrefab == null)
        {
            // 슬롯이 비어있으면 빈 슬롯 프리팹을 사용
            return slotObj;
        }
        else // 착용 아이템을 넣어야한다면.. 
        {
            // 엠티 슬롯의 자식 오브젝트들을 먼저 삭제
            foreach (Transform child in slotObj.transform)
            {
                Destroy(child.gameObject); // 기존의 자식 오브젝트 삭제
            }
            
            // 빈 슬롯 아이템을 먼저 생성하고 그 하위에 넣어주어야함.
            GameObject obj = currentSlot.GetItemObject().characterPrefab;

            // 인스턴스화하여 Content Panel의 자식으로 설정
            GameObject instantiatedObj = Instantiate(obj, slotObj.transform);
            instantiatedObj.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f); // 앵커를 중앙으로 설정
            instantiatedObj.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f); // 앵커를 중앙으로 설정
            instantiatedObj.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            instantiatedObj.SetActive(true);
        
            return slotObj;
        }
    }
} // end class