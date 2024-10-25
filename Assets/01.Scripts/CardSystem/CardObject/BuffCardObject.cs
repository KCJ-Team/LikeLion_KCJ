using UnityEngine;
using UnityEngine.Serialization;

// 버프 카드 오브젝트 클래스
[CreateAssetMenu(fileName = "New BuffCard", menuName = "Inventory System/Cards/BuffCard")]
public class BuffCardObject : CardObject
{
    public BuffSkill buffData;
}
