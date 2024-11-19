using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DynamicInterface_Lobby : UserInterface
{
    public GameObject inventoryPrefab;
    public GameObject contentPanel;

    [SerializeField]
    //private List<GameObject> instantiatedSlots = new List<GameObject>();
    
    public override void CreateSlots()
    {
        // 슬롯과 InventorySlot을 매핑하기 위한 딕셔너리 초기화
        slotsOnInterface = new SerializedDictionary<GameObject, InventorySlot>();
        
        // // 기존에 생성된 슬롯들을 모두 제거
        // foreach (var slot in instantiatedSlots)
        // {
        //     Destroy(slot);
        // }
        // instantiatedSlots.Clear();
    
        int itemCount = inventory.GetSlots.Count();
    
        // 슬롯 개수만큼이 아니고.. 음.. itemObject가 -1이면 건너띄워야하지 않을까? 
        for (int i = 0; i < itemCount; i++)
        {
            CardObject itemObject = inventory.GetSlots[i].GetItemObject();
            
            // 만약 itemObject가 null이면.. 슬롯을 생성 XXX임 
            if (itemObject == null) continue;
            if (itemObject.cardData.Id == -1) continue;
            
            
            // 인터페이스 타입에 맞는 아이템만 생성
            if (this.interfaceType == InterfaceType.Weapon && itemObject.type != CardType.Weapon)
            {
                continue;  // Weapon 타입이 아니면 이 슬롯은 건너뜀
            }
            else if (this.interfaceType == InterfaceType.SkillAndBuff && (itemObject.type != CardType.Skill && itemObject.type != CardType.Buff))
            {
                continue;  // SkillAndBuff 타입이 아니면 이 슬롯은 건너뜀
            }
            
            // 여기에서 초기화 작업?? 
            inventory.GetSlots[i].onAfterUpdated += OnSlotUpdate; // 슬롯 데이터 변경 시 호출될 이벤트
            //
            // 슬롯 프리팹을 인스턴스화하여 슬롯 오브젝트 생성
            GameObject obj = CreateSlotObj(i);
            
            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragStart(obj); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });
            AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });
            // 클릭 이벤트 추가
            AddEvent(obj, EventTriggerType.PointerClick, delegate { OnClick(obj); });

            // 슬롯 데이터를 UI 오브젝트와 매핑
            // inventory.GetSlots[i].slotDisplay = obj; // 슬롯 데이터를 UI 슬롯에 연결
            inventory.GetSlots[i].targetObject = obj;
            slotsOnInterface.Add(obj, inventory.GetSlots[i]);// 딕셔너리에 추가
            
            // 생성된 슬롯을 리스트에 추가
           // instantiatedSlots.Add(obj);
        }
    }
    
    public GameObject CreateSlotObj(int slotIndex)
    {
        // inventory.GetSlots[slotIndex].GetItemObject().characterDisplay가 Prefab이라면 인스턴스화해야 함
        GameObject obj = inventory.GetSlots[slotIndex].GetItemObject().characterPrefab;

        // 객체가 null이 아니면, Prefab을 인스턴스화하여 Content Panel의 자식으로 설정
        if (obj != null)
        {
            // 인스턴스화
            GameObject instantiatedObj = Instantiate(obj, contentPanel.transform);
        
            // 인스턴스화된 객체 활성화
            instantiatedObj.SetActive(true);
        
            return instantiatedObj;  // 인스턴스화된 객체 반환
        }

        return null; // null이면 반환
    }
    
    // public GameObject CreateSlotObj()
    // {
    //     // 슬롯 프리팹을 Content Panel의 자식으로 생성
    //     GameObject obj = Instantiate(inventoryPrefab, contentPanel.transform);
    //    //  obj.AddComponent<SlotHandler>();
    //     
    //     return obj;
    // }
}