using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using PlayerInfo;

[Serializable]
public class Player
{
    public string playerId;
    public float x;
    public float y;
    public float z;
    
    public float rx;
    public float ry;
    public float rz;
    
    public float hp;
    public float speed;
    public float rotationSpeed;
    
    public PlayerModelType prefabModelType;
    public PlayerWeaponType prefabWeaponType;
    public AnimationParameters animParams;
}
