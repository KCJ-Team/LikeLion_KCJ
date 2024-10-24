using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private Skill currentSkill;
    
    public void SetSkill(Skill skill)
    {
        currentSkill = skill;
        skill.Initialize(this);
    }
    
    public void UseSkill()
    {
        if (currentSkill != null)
        {
            currentSkill.Execute();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            UseSkill();
        }
    }
}
