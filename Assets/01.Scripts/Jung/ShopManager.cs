using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public GameObject itemButtonPrefab; // 동적으로 생성할 버튼의 Prefab
    public Transform itemListContainer; // 버튼들이 위치할 상점 UI의 부모 객체
    public List<ShopItem> shopItems = new List<ShopItem>();

    void Start()
    {
        LoadItemsFromXML("items.xml");
        DisplayShopItems();
    }

    // XML 파일에서 아이템 데이터를 로드
    void LoadItemsFromXML(string filePath)
    {
        TextAsset xmlData = Resources.Load<TextAsset>("Items/items"); // 확장자 제외하고 불러옴
        if (xmlData != null)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlData.text);

            XmlNodeList itemList = doc.SelectNodes("ShopItems/Item");
            foreach (XmlNode itemNode in itemList)
            {
                ShopItem item = new ShopItem();
                item.name = itemNode["Name"].InnerText;
                item.iconPath = itemNode["IconPath"].InnerText;
                item.currency1 = int.Parse(itemNode["Currency1"].InnerText);
                item.description = itemNode["Description"].InnerText;

                shopItems.Add(item);
            }
        }
        else
        {
            Debug.LogError("XML file not found: " + filePath);
        }
    }

    // 동적으로 버튼을 생성하여 상점에 표시
    void DisplayShopItems()
    {
        foreach (ShopItem item in shopItems)
        {
            // 아이템 버튼 동적 생성
            GameObject itemButton = Instantiate(itemButtonPrefab, itemListContainer);

            // "ItemName" 오브젝트의 TextMeshProUGUI 컴포넌트 찾기
            var itemNameObject = itemButton.transform.Find("ItemName");
            if (itemNameObject != null)
            {
                var textComponent = itemNameObject.GetComponent<TextMeshProUGUI>();
                if (textComponent != null)
                {
                    textComponent.text = item.name; // 아이템 이름 설정
                }
                else
                {
                    Debug.LogError("TextMeshProUGUI component is missing on ItemName object!");
                }
            }
            else
            {
                Debug.LogError("ItemName object not found in itemButtonPrefab!");
            }

            // 나머지 텍스트 설정도 TextMeshProUGUI를 사용
            var descriptionComponent = itemButton.transform.Find("ItemDescription").GetComponent<TextMeshProUGUI>();
            if (descriptionComponent != null)
            {
                descriptionComponent.text = item.description; // 아이템 설명 설정
            }

            var priceComponent = itemButton.transform.Find("ItemPrice").GetComponent<TextMeshProUGUI>();
            if (priceComponent != null)
            {
                priceComponent.text = "Price: " + item.currency1.ToString(); // 가격 설정
            }

            // 아이콘 설정
            Image iconImage = itemButton.transform.Find("ItemIcon").GetComponent<Image>();
            Sprite itemIcon = Resources.Load<Sprite>(item.iconPath);
            if (itemIcon != null)
            {
                iconImage.sprite = itemIcon; // 아이콘 설정
            }
            else
            {
                Debug.LogError("Icon not found for item: " + item.name);
            }
        }
    }

    public class ShopItem
    {
        public string name;
        public string iconPath; // 아이콘 경로 추가
        public int currency1;
        public string description;
    }
}