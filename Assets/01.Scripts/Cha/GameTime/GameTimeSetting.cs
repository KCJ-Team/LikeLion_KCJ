using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameTimeSetting", menuName = "Configurations/GameTimeSettings")]
public class GameTimeSetting : ScriptableObject
{
    public int startDay = 14; // 총 게임 일수, 시작일수
    public float dayDuration = 180f; // 하루 길이 (3분)
    public float xSpeed = 2f; // 현재 2배속 가능 
}
