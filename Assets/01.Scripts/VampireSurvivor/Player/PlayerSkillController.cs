using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerSkillController : MonoBehaviour
{
    private PlayerController player;
    private PlayerData playerData;
    
    [SerializeField] private List<CardObject> skillPrefabs;
    
    public float currentCooldown = 0f;
    private bool isSkillActive = false;

    private void Awake()
    {
        player = GetComponent<PlayerController>();
    }

    private void Start()
    {
        playerData = GameManager.Instance.playerData;
        
        InitSkills();
    }

    private void Update()
    {
        SkillCooldown();
        CheckSkillInput();
    }
    
    private void InitSkills()
    {
        for (int i = 0; i < 2; i++)
        {
            RandomSkillSelect();
        }
        
        SetSkillCoolTime();
    }
    
    private void CheckSkillInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TryUseSkill();
        }
    }

    private void SetSkillCoolTime()
    {
        // 첫 번째 스킬의 쿨타임 설정
        if (skillPrefabs[0] is SkillCardObject skillCard)
        {
            currentCooldown = skillCard.skillData.Cooldown;
        }
        else if (skillPrefabs[0] is BuffCardObject buffCard)
        {
            currentCooldown = buffCard.buffData.Cooldown;
        }
    }

    private void SkillCooldown()
    {
        isSkillActive = false;
        
        currentCooldown -= Time.deltaTime;
        if (currentCooldown <= 0)
        {
            isSkillActive = true;
            currentCooldown = 0;
        }
    }

    private void TryUseSkill()
    {
        if (isSkillActive)
        {
            UseCardSkill(skillPrefabs[0]);
            
            skillPrefabs.RemoveAt(0);
            RandomSkillSelect();
            
            SetSkillCoolTime();
        }
    }
    
    private void UseCardSkill(CardObject cardObject)
    {
        if (cardObject is SkillCardObject skillCard)
        {
            GameObject skillObject = Instantiate(skillCard.skillData.gameObject, transform.position, Quaternion.identity, transform);
            Skill skill = skillObject.GetComponent<Skill>();
            skill.Initialize(player);
            skill.Execute();
        }
        else if (cardObject is BuffCardObject buffCard)
        {
            BuffSkill buffSkill = buffCard.buffData;
            buffSkill.Initialize(player);
            buffSkill.Execute();
        }
    }
    
    private void RandomSkillSelect()
    {
        int randomIndex = Random.Range(0, playerData.skillCards.Count);
        CardObject randomSkillCard = playerData.skillCards[randomIndex];
        skillPrefabs.Add(randomSkillCard);
    }
}