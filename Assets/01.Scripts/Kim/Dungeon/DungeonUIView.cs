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
    public GameObject panelBossSpawned;

    [FormerlySerializedAs("textCooltime")] [Header("Skills")] 
    public Text[] textCooltimes;
    
    // TODO : 던전매니저의 hp, attack, defense 실시간 관리할건지? 
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
