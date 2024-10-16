using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Date", menuName = "SO/PlayerData")]
public class PlayerData : ScriptableObject
{
    public GameObject Chracter; //선택한 캐릭터 모델
    //스탯 수정 가능
    public float HP;
    public float MP;
    
    //현재 장착한 장비 및 인벤토리, equipment 데이터를 바인딩
    public GameObject currentWeapon;
    public InventoryObject inventory;
    
    //재화
}
