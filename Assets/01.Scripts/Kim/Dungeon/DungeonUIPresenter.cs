using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonUIPresenter
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
    
    public void SetAmmo(int currentAmmo, int maxAmmo)
    {
        uiView.UpdateAmmo(currentAmmo, maxAmmo);
    }

    public void SetRemainingMonsters(int remaining)
    {
        uiView.UpdateRemainingMonsters(remaining);
    }

    public void SetBossSpawned(bool isSpawned)
    {
        uiView.UpdateBossSpawned(isSpawned);
    }

    public void SetSkillCooldownBySlotNumber(int slotNum, int coolDown)
    {
        uiView.UpdateSkillCooldowns(slotNum, coolDown);
    }
}
