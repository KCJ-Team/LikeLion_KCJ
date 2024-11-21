using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : Skill
{
    [SerializeField] private GameObject MinePrefab;
    private GameObject currentMine;

    public void MineInstall(Vector3 targetPosition)
    {
        
        currentMine = Instantiate(MinePrefab, targetPosition, Quaternion.identity);
        SoundManager.Instance.PlaySFX(SFXSoundType.Skill_Mine);
        
        Vector3 direction = Vector3.zero;
        
        Projectile pr = currentMine.GetComponent<Projectile>();

        if (pr != null)
        {
            Debug.Log("넘김");
            pr.Initialize(direction, damage);
        }
    }
    
    public override SkillState GetInitialState()
    {
        return new MinePreparingState(this);
    }
}
