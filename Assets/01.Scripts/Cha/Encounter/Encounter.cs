using System;
using UnityEngine;

[Serializable]
public class Encounter
{
    public int encounterId;
    public string name;
    public string description;
    public Choice choice1;
    public Choice choice2;
}

[Serializable]
public class Choice
{
    public string text;
    public string result;
    public int energy;
    public int food;
    public int fuel;
    public int workforce;
    public string faction;
    public float factionSupport;
}