using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private Skill currentSkill;
    private MissileBombardment missileSkill;
    private SpeedBuffSkill speedBuffSkill;
    private Status _status;
    public float currentSpeed;

    private void Start()
    {
        missileSkill = GetComponent<MissileBombardment>();
        speedBuffSkill = GetComponent<SpeedBuffSkill>();
        _status = GetComponent<Status>();
    }

    public void SetSkill(Skill skill)
    {
        currentSkill = skill;
        //skill.Initialize(this);
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
            SetSkill(missileSkill);
            UseSkill();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            SetSkill(speedBuffSkill);
            UseSkill();
        }
    }

    private void FixedUpdate()
    {
        currentSpeed = _status.GetCurrentMoveSpeed();
    }
}
