using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DynamicInterface : UserInterface
{
    public GameObject inventoryPrefab;
    public GameObject contentPanel;
    public int X_START;
    public int Y_START;
    public int X_SPACE_BETWEEN_ITEM;
    public int Y_SPACE_BETWEEN_ITEM;
    public int ITEMS_PER_PAGE = 10;
    
    private List<GameObject> instantiatedSlots = new List<GameObject>();

    public override void CreateSlots()
    {
        slotsOnInterface = new SerializedDictionary<GameObject, InventorySlot>();
        
        foreach (var slot in instantiatedSlots)
        {
            Destroy(slot);
        }
        instantiatedSlots.Clear();

        int itemCount = inventory.GetSlots.Count();
        
        for (int i = 0; i < itemCount; i++)
        {
            var obj = CreateSlotObj();
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);

            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragStart(obj); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });
            AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });
            
            inventory.GetSlots[i].targetObject = obj;
            slotsOnInterface.Add(obj, inventory.GetSlots[i]);
            instantiatedSlots.Add(obj);
        }
        
        int columns = (itemCount + ITEMS_PER_PAGE - 1) / ITEMS_PER_PAGE;
        float contentWidth = columns * (X_SPACE_BETWEEN_ITEM + inventoryPrefab.GetComponent<RectTransform>().rect.width);
        contentPanel.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, contentWidth);
    }

    public GameObject CreateSlotObj()
    {
        var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, contentPanel.transform);

        return obj;
    }

    public Vector3 GetPosition(int i)
    {
        int column = i / ITEMS_PER_PAGE;
        int row = i % ITEMS_PER_PAGE;
        return new Vector3(X_START + (X_SPACE_BETWEEN_ITEM * column), Y_START + (-Y_SPACE_BETWEEN_ITEM * row), 0f);
    }
}