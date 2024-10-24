using System;
using UnityEngine;
using System.Collections.Generic;

// 기본 카드 오브젝트 클래스
[CreateAssetMenu(fileName = "New CardObject", menuName = "Inventory System/Cards/CardObject")]
public class CardObject : ScriptableObject
{
    public Sprite uiDisplay;              // UI에 표시될 이미지
    public GameObject characterDisplay;   // 3D 모델
    public bool stackable;                // 중첩 가능 여부
    public CardType type;                 // 카드 타입
    [NonSerialized] public Card cardData = new Card();    // 기본 카드 데이터
    
    // 카드 인스턴스 생성 메서드
    public virtual Card CreateCard()
    {
        return new Card(this);
    }

    // Unity 에디터에서 값이 변경될 때 호출되는 메서드
    protected virtual void OnValidate()
    {
        if (characterDisplay == null) return;
        
        if(!characterDisplay.GetComponent<SkinnedMeshRenderer>()) return;

        var renderer = characterDisplay.GetComponent<SkinnedMeshRenderer>();
    }
}