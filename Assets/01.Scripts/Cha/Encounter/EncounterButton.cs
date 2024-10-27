using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EncounterButton : MonoBehaviour
{
    public Button btnEncounter;
    public Button btnChoice1;
    public Button btnChoice2;
    
    private void Start()
    {
        btnEncounter?.onClick.AddListener(PrintEncounter);
        btnChoice1?.onClick.AddListener(ChooseChoice1);
        btnChoice2?.onClick.AddListener(ChooseChoice2);
    }
    
    private void ChooseChoice1()
    {
        EncounterManager.Instance.ChooseOption1();
    }
    
    private void ChooseChoice2()
    {
        EncounterManager.Instance.ChooseOption2();
    }

    private void PrintEncounter()
    {
        EncounterManager.Instance.PrintRandomEncounter();
    }
} // end class
