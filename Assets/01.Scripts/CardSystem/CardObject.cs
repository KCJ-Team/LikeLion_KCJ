using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory System/Cards/CardObject")]
public class CardObject : ScriptableObject
{
    public Sprite uiDisplay;
    public GameObject characterDisplay;
    public bool stackable;
    public CardType type;
    public Card data = new Card();
    
    public List<string> boneNames = new List<string>();

    public Card CreateCard()
    {
        Card newCard = new Card(this);
        return newCard;
    }

    private void OnValidate()
    {
        boneNames.Clear();
        
        if (characterDisplay == null) return;
        
        if(!characterDisplay.GetComponent<SkinnedMeshRenderer>()) return;

        var renderer = characterDisplay.GetComponent<SkinnedMeshRenderer>();
        
        var bones = renderer.bones;

        foreach (var t in bones)
        {
            boneNames.Add(t.name);
        }
    }
}
