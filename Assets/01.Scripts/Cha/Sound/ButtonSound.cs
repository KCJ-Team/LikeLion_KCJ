using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public UISoundType hoverSoundType = UISoundType.Hover;
    public UISoundType clickSoundType = UISoundType.Click; // 버튼별 클릭 사운드 지정하기
    
    // Hover 시
    public void OnPointerEnter(PointerEventData eventData)
    {
        SoundManager.Instance.PlayUISound(hoverSoundType);
    }

    // Click 시
    public void OnPointerClick(PointerEventData eventData)
    {
        SoundManager.Instance.PlayUISound(clickSoundType);
    }
}
