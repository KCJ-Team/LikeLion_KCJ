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
        SoundManager.Instance.PlaySFX(SFXSoundType.Skill_BlackHole);
        
        Vector3 direction = Vector3.zero;
        Projectile pr = currentBlackHole.GetComponent<Projectile>();
            
        if (currentBlackHole != null)
        {
            pr.Initialize(direction, damage);
        }
    }
    
    public override SkillState GetInitialState()
    {
        return new BlackHolePreparingState(this);
    }
}
