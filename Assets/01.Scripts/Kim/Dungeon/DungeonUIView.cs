using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DungeonUIView : MonoBehaviour
{
    [Header("HP와 Stress UI")]
    public Slider hpSlider;
    public Text hpText;
    public Text attackText;
    public Text defenseText;

    public void UpdateStats()
    {
        hpSlider.DOValue(DungeonManager.Instance.hp, 0.5f).OnUpdate(() =>
        {
            hpText.text = $"{Mathf.Round(hpSlider.value)}/{GameManager.Instance.playerData.BaseHP}";
        });
        
        attackText.text = $"{DungeonManager.Instance.attack}";
        defenseText.text = $"{DungeonManager.Instance.defense}";
    }
}
