using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public UISoundType hoverSoundType = UISoundType.Hover;
    public UISoundType clickSoundType = UISoundType.Click; // 버튼별 클릭 사운드 지정하기

    public bool isLoadSceneBtn;
    
    // Hover 시
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverSoundType != UISoundType.None)
            SoundManager.Instance.PlayUISound(hoverSoundType);
    }

    // Click 시
    public void OnPointerClick(PointerEventData eventData)
    {
        if (isLoadSceneBtn)
        {
            // hyuna Scene 넘어가기전 Shot
            SoundManager.Instance.PlayUISound(UISoundType.LoadScene);
            return;
        }
        
        SoundManager.Instance.PlayUISound(clickSoundType);
    }
}
