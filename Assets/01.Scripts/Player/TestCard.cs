using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCard : MonoBehaviour
{
    public PlayerData playerData;

    public List<CardObject> cards = new List<CardObject>();

    private void Update()
    {
        InvenToCard();
    }

    private void InvenToCard()
    {
        // 1부터 9까지의 숫자 키 입력 처리
        for (int i = 1; i <= 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + i))
            {
                // cards 리스트의 인덱스는 0부터 시작하므로 i-1
                int cardIndex = i - 1;
            
                // cards 리스트의 범위를 벗어나지 않는지 확인
                if (cardIndex < cards.Count)
                {
                    playerData.inventory.AddItem(new Card(cards[cardIndex]), 1);
                }
            }
        }
    }
}
