using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : Skill
{
    [SerializeField] private GameObject BlackHolePrefab;
    private GameObject currentBlackHole;
    
    public void BlackHoleInstall(Vector3 targetPosition)
    {
        // 전달받은 위치에 터렛 생성
        currentBlackHole = Instantiate(BlackHolePrefab, targetPosition, Quaternion.identity);
    }
    
    public override SkillState GetInitialState()
    {
        return new BlackHolePreparingState(this);
    }
}
