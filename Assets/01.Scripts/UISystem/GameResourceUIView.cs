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

    public Image iconConsume;
    
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

    public void ShowIconConsumeAt9PM()
    {
        // 초기 투명도와 위치 설정
        var canvasGroup = iconConsume.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = iconConsume.gameObject.AddComponent<CanvasGroup>();
        }
        canvasGroup.alpha = 1f;

        var originalPosition = iconConsume.transform.localPosition;
        var originalScale = iconConsume.transform.localScale; // 원래 크기 저장

        // 효과음 재생
        SoundManager.Instance.PlaySFX(SFXSoundType.NegativePop);

        // DOTween 애니메이션 설정
        Sequence sequence = DOTween.Sequence();
        sequence.Append(iconConsume.transform.DOPunchScale(Vector3.one * 0.2f, 0.5f, 10, 1f)) // 크기 팽창 효과
            .AppendInterval(1.5f) // 1.5초 유지
            .Append(iconConsume.transform.DOLocalMoveY(originalPosition.y + 30f, 0.5f)) // 위로 이동
            .Join(canvasGroup.DOFade(0f, 0.5f)) // 서서히 투명해짐
            .OnComplete(() =>
            {
                iconConsume.gameObject.SetActive(false); // 완전히 사라진 뒤 비활성화
                iconConsume.transform.localPosition = originalPosition; // 위치 초기화
                iconConsume.transform.localScale = originalScale; // 크기 초기화
                canvasGroup.alpha = 1f; // 투명도 초기화
            });
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