using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonUIPresenter : MonoBehaviour
{
    private readonly DungeonUIView uiView;

    public DungeonUIPresenter(DungeonUIView uiView)
    {
        this.uiView = uiView;
    }

    public void SetStats(float newHp, float newDefense, float newAttack)
    {
        DungeonManager.Instance.hp = Mathf.Clamp(newHp, 0.0f, 
            GameManager.Instance.playerData.BaseHP);

        DungeonManager.Instance.defense = newDefense;
        DungeonManager.Instance.attack = newAttack;
        
        uiView.UpdateStats();
    }
}
