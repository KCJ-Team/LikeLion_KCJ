using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//플레이어 기본 정보 스크립트
[CreateAssetMenu]
public class PlayerInfo : ScriptableObject
{
    public string Name;

    public PlayerInfo(string name)
    {
        Name = name;
    }
}
