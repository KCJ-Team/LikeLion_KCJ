using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//UI 요소 관리 스크립트
public class GameResourceUIView : MonoBehaviour
{
    // 각 자원의 UI 텍스트
    [Header("각 자원의 UI 텍스트")]
    public TextMeshProUGUI energyText;
    public TextMeshProUGUI foodText;
    public TextMeshProUGUI workforceText;
    public TextMeshProUGUI fuelText;
    public TextMeshProUGUI researchText;
    public TextMeshProUGUI currencyText;
    
    //[Header("자원이 0일경우 Warning Text, 자원이 0일때 1day가 지나면 GameOver")]
    
    // UpdateResourceUI 메서드를 통해 자원 정보를 받아 UI를 업데이트
    public void UpdateResourceUI(int energy, int food, int workforce, int fuel, int research, int currency)
    {
        UpdateTextWithTypingEffect(energyText, energy);
        UpdateTextWithTypingEffect(foodText, food);
        UpdateTextWithTypingEffect(workforceText, workforce);
        UpdateTextWithTypingEffect(fuelText, fuel);
        UpdateTextWithTypingEffect(researchText, research);
        UpdateTextWithTypingEffect(currencyText, currency);
    }

    private void UpdateTextWithTypingEffect(TextMeshProUGUI textElement, int newValue)
    {
        // 현재 텍스트 값 읽기 (숫자로 변환)
        int currentValue = int.TryParse(textElement.text, out int parsedValue) ? parsedValue : 0;

        // 타이핑 효과 (숫자가 서서히 변경됨)
        DOTween.To(() => currentValue, value =>
        {
            textElement.text = $"{value}";
        }, newValue, 0.5f); // 0.5초 동안 애니메이션
    }
} // end class