using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EndingType
{
    NONE, RED, BLACK, GREEN, YELLOW, RIOT, STRESS, ILLNESS
}

[CreateAssetMenu(fileName = "EndingData", menuName = "Ending/EndingData")]
public class EndingData : ScriptableObject
{
    public int endingId;
    public string endingName; // 엔딩 이름 (예: Red Faction Ending, Bad Ending)
    public EndingType endingType; // 엔딩타입
    public string endingText; // 엔딩 텍스트
    public Sprite endingImage; // 엔딩 아이콘, 이미지
}
