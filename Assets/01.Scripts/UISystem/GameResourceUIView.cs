using System.Collections;
using System.Collections.Generic;
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
    public void UpdateResourceUI(int energy, int food, int workforce,  int fuel, int research, int currency)
    {
        energyText.text = $"{energy}";
        foodText.text = $"{food}";
        workforceText.text = $"{workforce}";
        fuelText.text = $"{fuel}";
        researchText.text = $"{research}";
        currencyText.text = $"{currency}";
    }
} // end class