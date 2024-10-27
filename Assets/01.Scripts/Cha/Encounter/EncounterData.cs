using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "EncounterData", menuName = "Encounter/Encounter Data", order = 1)]
public class EncounterData : ScriptableObject
{
    public List<Encounter> encounters;
}
