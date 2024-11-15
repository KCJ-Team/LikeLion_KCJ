using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 로비메뉴, 로비에서 관리하는 정보들 매니저
/// </summary>
public class LobbyMenuManager : MonoBehaviour
{
    [Header("Player Data..")]
    public PlayerData PlayerData;

    [Header("로비에서 관리할 플레이어의 정보들")] 
    private float hp;
    private int stress;
    private float attack;
    private float defense;

    [Header("Down 패널 버튼들")] 
    public Button btnProfile;
    public Button btnDeck;
    public Button btnStore;

    
    
}
