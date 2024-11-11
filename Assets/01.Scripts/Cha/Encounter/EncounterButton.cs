using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EncounterButton : MonoBehaviour
{
    public Button btnEncounter;
    
    private void Start()
    {
        btnEncounter?.onClick.AddListener(PrintEncounter);
    }
    
    private void PrintEncounter()
    {
        EncounterManager.Instance.PrintRandomEncounter();
    }
} // end class
