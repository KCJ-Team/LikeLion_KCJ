using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


public enum EndingType
{
    NONE, RED, BLACK, GREEN, YELLOW, RIOT, STRESS, ILLNESS
}

[CreateAssetMenu(fileName = "EndingData", menuName = "Ending/EndingData")]
public class EndingData : ScriptableObject
{
    public int endingId;
    public string endingTitle; // 엔딩 이름
    public string endingTitleKr;
    public EndingType endingType; // 엔딩타입
    public string endingText; // 엔딩 텍스트
    public Sprite endingImage; // 엔딩 백그라운드 이미지
}
