using UnityEngine;

// 모든 카드 오브젝트를 관리하는 데이터베이스
[CreateAssetMenu(fileName = "New Item Database", menuName = "Inventory System/Cards/Database")]
public class CardDatabase : ScriptableObject
{
    public CardObject[] CardObjects; // 모든 카드 오브젝트 배열

    // Unity 에디터에서 값이 변경될 때 호출되는 메서드
    public void OnValidate()
    {
        // 각 카드 오브젝트에 고유 ID 할당
        for (int i = 0; i < CardObjects.Length; i++)
        {
            CardObjects[i].cardData.Id = i;
        }
    }
    
    // 런타임 또는 에디터에서 호출 가능한 초기화 메서드
    public void InitializeIDs()
    {
        for (int i = 0; i < CardObjects.Length; i++)
        {
            CardObjects[i].cardData.Id = i;
        }
    }
}