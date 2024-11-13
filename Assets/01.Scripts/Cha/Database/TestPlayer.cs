using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 테스트용 클라이언트 플레이어데이터
[Serializable]
public class TestPlayer
{
    public string playerId;
    public float x;
    public float y;
    public float z;
    public float speed;
    public int health;
    public GameObject playerObject;

    public TestPlayer()
    {
        
    }

    public TestPlayer(string playerId, float x, float y, float z, float speed, int health)
    {
        this.playerId = playerId;
        this.x = x;
        this.y = y;
        this.z = z;
        this.speed = speed;
        this.health = health;
    }
}
