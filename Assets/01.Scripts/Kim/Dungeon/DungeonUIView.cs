using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Serialization;

public class DungeonUIView : MonoBehaviour
{
    [Header("HP와 Stress UI")]
    public Slider hpSlider;
    public Text hpText;
    public Text attackText;
    public Text defenseText;

    [Header("Ammo")] 
    public Text textAmmo;

    [Header("Monster Wave")] 
    public Text textRemaingMonster;
    public Text textRemaingBossTime;
    public GameObject panelBossSpawned;

    public Button LobbyBtn;

    [FormerlySerializedAs("textCooltime")] [Header("Skills")] 
    public Text[] textCooltimes;
    
    private void Start()
    {
        LobbyBtn.onClick.AddListener(() => GameSceneDataManager.Instance.LoadScene("Lobby"));
    }
    
    public void UpdateStats()
    {
        hpSlider.DOValue(DungeonManager.Instance.hp, 0.5f).OnUpdate(() =>
        {
            hpText.text = $"{Mathf.Round(hpSlider.value)}/{GameManager.Instance.playerData.BaseHP}";
        });
        
        attackText.text = $"{DungeonManager.Instance.attack}";
        defenseText.text = $"{DungeonManager.Instance.defense}";
    }
    
    public void UpdateAmmo(int currentAmmo, int maxAmmo)
    {
        textAmmo.text = $"{currentAmmo}/{maxAmmo}";
    }

    public void UpdateRemainingMonsters(int remaining)
    {
        textRemaingMonster.text = $"{remaining}";
    }

    public void UpdateBossSpawned(bool isSpawned)
    {
        panelBossSpawned.SetActive(isSpawned);
    }

    public void UpdateRemainingBossTimer(int remaining)
    {
        textRemaingBossTime.text = $"{remaining}";

        if (remaining == 0)
        {
            textRemaingBossTime.text = "SPAWN";
        }
        else
        {
            textRemaingBossTime.text = $"{remaining}";
        }
    }

    public void UpdateSkillCooldowns(int slotNum, int cooldowns)
    {
        if (cooldowns == 0)
        {
            textCooltimes[slotNum].text = null;
        }
        else
        {
            textCooltimes[slotNum].text = $"{cooldowns}";
        }
    }

    
}
