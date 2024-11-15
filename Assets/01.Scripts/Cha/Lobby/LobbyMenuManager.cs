using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 로비메뉴, 로비에서 관리하는 정보들 매니저
/// </summary>
public class LobbyMenuManager : SceneSingleton<LobbyMenuManager>
{
    [Header("Player Data..")] public PlayerData playerData;

    [Header("로비에서 관리할 플레이어의 정보들")] public float hp;
    public float stress;
    public float attack;
    public float defense;

    [Header("Down 패널 버튼들")] public Button btnProfile;
    public Button btnDeck;
    public Button btnStore;

    [Header("HP와 Stress UI")] public Slider hpSlider;
    public Text hpText;
    public Slider stressSlider;
    public Text stressText;
    public Text attackText;
    public Text defenseText;

    private void Start()
    {
        // UI 초기화
        // UpdateHpUI(hp);
        // UpdateStressUI(stress);
        // UpdateAttackUI(attack);
        // UpdateDefenseUI(defense);

        // 슬라이더 값이 변경되었을 때 이벤트 연결
        hpSlider.onValueChanged.AddListener(OnHpSliderChanged);
        stressSlider.onValueChanged.AddListener(OnStressSliderChanged);
    }

    private void OnDestroy()
    {
        // 슬라이더 이벤트 제거 (메모리 누수 방지)
        hpSlider.onValueChanged.RemoveListener(OnHpSliderChanged);
        stressSlider.onValueChanged.RemoveListener(OnStressSliderChanged);
    }

    public void SetHpAndStress(float newHp, float newStress)
    {
        hp = Mathf.Clamp(newHp, 0.0f, playerData.BaseHP);
        stress = Mathf.Clamp(newStress, 0.0f, 100.0f);

        Debug.Log($"SetHpAndStress: HP = {hp}, Stress = {stress}");
        
        UpdateHpAndStressUI();
    }

    public void UpdateHpAndStressUI()
    {
        hpSlider.DOValue(hp, 0.5f).OnUpdate(() =>
        {
            hpText.text = $"{Mathf.Round(hpSlider.value)}/{playerData.BaseHP}";
        });
        
        stressSlider.DOValue(stress, 0.5f).OnUpdate(() =>
        {
            stressText.text = $"{Mathf.Round(stressSlider.value)}/100";
        });
    }

    public void ChangeHp(float amount)
    {
        hp = Mathf.Clamp(hp + amount, 0.0f, playerData.BaseHP);
        
        Debug.Log($"ChangeHp: HP changed to {hp}");
        UpdateHpAndStressUI();
    }

    public void ChangeStress(float amount)
    {
        stress = Mathf.Clamp(stress - amount, 0.0f, 100.0f);
        
        Debug.Log($"ChangeStress: Stress changed to {stress}");
        UpdateHpAndStressUI();
    }

    public void SetAttackAndDefense(float newAttack, float newDefense)
    {
        attack = newAttack;
        defense = newDefense;

        UpdateAttackAndDefenseUI();
    }

    public void UpdateAttackAndDefenseUI()
    {
        attackText.text = $"{attack}";
        defenseText.text = $"{defense}";
    }

    private void OnHpSliderChanged(float value)
    {
        // SetHpAndStress(value);
    }

    private void OnStressSliderChanged(float value)
    {
        // UpdateStressUI(value);
    }
} // end class