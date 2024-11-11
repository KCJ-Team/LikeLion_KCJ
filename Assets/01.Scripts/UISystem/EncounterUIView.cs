using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//UI 요소 관리 스크립트
public class EncounterUIView : MonoBehaviour
{
    [Header("Encounter 버튼 오브젝트 풀")]
    public List<Button> encounterObjects;

    [Header("인카운터 팝업 버튼")] 
    public Button btnChoice1;
    public Button btnChoice2;

    [Header("팝업창의 자원 이미지들")] 
    public Image imageEnergy;
    public Image imageFood;
    public Image imageWorkforce;
    public Image imageFuel;
} // end class