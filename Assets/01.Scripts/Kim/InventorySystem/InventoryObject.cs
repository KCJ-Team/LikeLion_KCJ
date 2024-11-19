using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public CardDatabase database;  // 카드 데이터베이스 (아이템에 대한 정보 포함)
    public InventoryType invenType;  // 인벤토리 타입 (예: 일반, 퀘스트 등)
    public CardType type;  // 카드의 타입 (예: 방어, 공격 등)
    
    public InventorySlot[] GetSlots => Container.Slots;  // 슬롯 배열을 반환 (인벤토리 내 아이템들)

    [SerializeField] private Inventory Container = new Inventory();  // 인벤토리 슬롯들을 관리하는 Inventory 객체
    
    // 인벤토리에 아이템을 추가하는 메서드
    public bool AddItem(Card card, int amount)
    {
        // 비어 있는 슬롯이 없으면 아이템을 추가할 수 없음
        // if (EmptySlotCount <= 0) return false;

        // 현재 아이템이 슬롯에 존재하는지 찾기
        InventorySlot slot = FindItemOnInventory(card);
        
        // 카드가 스택 불가능하거나, 슬롯에 아이템이 없다면 새로운 슬롯에 아이템 추가
        if (!database.CardObjects[card.Id].stackable || slot == null)
        {
            // 비어 있는 슬롯이 없는 경우, 새로운 슬롯을 생성
            if (EmptySlotCount <= 0)
            {
                InventorySlot newSlot = new InventorySlot();
                newSlot.UpdateSlot(card, amount);
                return true;
            }
            
            // 빈 슬롯에 아이템과 수량을 설정
            GetEmptySlot().UpdateSlot(card, amount);
            return true;
        }
        
        // 슬롯에 아이템이 존재하고 스택 가능하면 수량만 증가
        slot.AddAmount(amount);
        
        Debug.Log($"{card.Name} 추가됨 인벤토리 SO에 ");
        return true;
    }

    // 비어있는 슬롯의 개수를 반환하는 프로퍼티
    public int EmptySlotCount
    {
        get
        {
            int counter = 0;
            // 슬롯을 순회하면서 비어 있는 슬롯(아이템 ID가 -1인 슬롯)을 찾음
            for (int i = 0; i < GetSlots.Length; i++)
            {
                if (GetSlots[i].card.Id <= -1)
                {
                    counter++;
                }
            }
            return counter;  // 비어 있는 슬롯 개수 반환
        }
    }

    // 인벤토리에서 특정 아이템을 찾아 반환하는 메서드
    public InventorySlot FindItemOnInventory(Card item)
    {
        // 슬롯을 순회하면서 해당 아이템을 찾음
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].card.Id == item.Id)
            {
                return GetSlots[i];  // 아이템이 발견되면 해당 슬롯 반환
            }
        }
        return null;  // 아이템이 없으면 null 반환
    }

    // 특정 아이템이 인벤토리에 존재하는지 확인하는 메서드
    public bool IsItemInInventory(CardObject item)
    {
        // 슬롯을 순회하면서 해당 아이템이 있는지 확인
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].card.Id == item.cardData.Id)
            {
                return true;  // 아이템이 존재하면 true 반환
            }
        }
        return false;  // 아이템이 없으면 false 반환
    }

    // 비어있는 슬롯을 반환하는 메서드
    public InventorySlot GetEmptySlot()
    {
        // 슬롯을 순회하면서 비어있는 슬롯을 찾음
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].card.Id <= -1)
            {
                return GetSlots[i];  // 비어있는 슬롯을 반환
            }
        }
        return null;  // 빈 슬롯이 없으면 null 반환
    }

    // 두 슬롯의 아이템을 스왑하는 메서드
    public void SwapItems(InventorySlot sourceItem, InventorySlot targetItem)
    {
        // 같은 슬롯을 스왑하려고 하면 아무것도 하지 않음
        if (sourceItem == targetItem) return;

        // 슬롯 1과 슬롯 2의 아이템 상태를 로그로 출력
        Debug.Log($"스왑 시작: 슬롯 1 ({sourceItem.card.Name}, 수량: {sourceItem.amount}) <-> 슬롯 2 ({targetItem.card.Name}, 수량: {targetItem.amount})");

        // hyuna
        // Available Item을 검사 , 두 슬롯의 아이템 교환이 가능한지 확인
        bool canSwapSource = targetItem.CanPlaceInSlot(sourceItem.GetItemObject());
        bool canSwapTarget = sourceItem.CanPlaceInSlot(targetItem.GetItemObject());
        
        // 교환이 가능한 경우
        if (canSwapSource && canSwapTarget)
        {
            // 타겟 아이템이 Empty Slot일 경우 교환 불가능, 엠티를 대체. 
            if (targetItem.card.Id <= -1)
            {
                // 타겟 슬롯에 소스 아이템을 그대로 넣고, 소스 슬롯은 비워준다
                targetItem.UpdateSlot(sourceItem.card, sourceItem.amount);
                sourceItem.RemoveItem();  // 소스 슬롯을 비워준다
                Debug.Log($"슬롯 2가 비어 있었으므로, 아이템을 슬롯 2에 배치하고 슬롯 1은 비워두었습니다.");
                return;
            }
            
            // tmffhtdmf 업데이트 
            InventorySlot temp = new InventorySlot(targetItem.card, targetItem.amount);

            // 슬롯 1과 슬롯 2의 데이터를 바로 교환
            Card tempCard = targetItem.card;  // 임시로 targetItem의 카드 데이터를 저장
            int tempAmount = targetItem.amount;   // 임시로 targetItem의 수량을 저장
            
            // 인벤 -> 이큅 전달, 이큅먼트 슬롯 아래에 생성
            targetItem.UpdateSlot(sourceItem.card, sourceItem.amount);
            
            sourceItem.RemoveItem();
            // 이큅 -> 인벤으로 새로운 아이템 생성
            sourceItem.UpdateSlot(temp.card, temp.amount);
            
            // 전달한건 삭제하기 
        
            Debug.Log("아이템 스왑 완료");
        }
        else
        {
            Debug.Log("아이템 교환이 불가능합니다. 맞는 Available 슬롯 타입이 아님");
        }
        
        // // 두 슬롯이 서로 교환 가능한지 확인
        // if (targetItem.CanPlaceInSlot(sourceItem.GetItemObject()) && sourceItem.CanPlaceInSlot(targetItem.GetItemObject()))
        // {
        //     // 스왑을 위해 임시 슬롯을 생성하고, 아이템을 교환
        //     InventorySlot temp = new InventorySlot(targetItem.card, targetItem.amount);
        //     targetItem.UpdateSlot(sourceItem.card, sourceItem.amount);
        //     sourceItem.UpdateSlot(temp.card, temp.amount);
        // }
        // else
        // {
        // }
    }

    // 인벤토리의 모든 슬롯을 초기화하는 메서드
    public void Clear()
    {
        Container.Clear();  // 인벤토리의 모든 슬롯을 초기화
    }
}
