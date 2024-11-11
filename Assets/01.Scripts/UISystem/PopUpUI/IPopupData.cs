using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PopupUI에서 사용할 데이터 모음 인터페이스
/// </summary>
public interface IPopupData
{
    string Title { get; }          // 헤더 텍스트에 사용할 제목
    string Description { get; }     // 기본 설명
}
