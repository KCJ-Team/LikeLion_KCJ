using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//UI 요소 관리 스크립트
public class UIView : MonoBehaviour
{
    public TextMeshProUGUI nameText;

    public void UpdateUI(string name)
    {
        nameText.text = name;
    }
}