using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Skill
{
    [SerializeField] private GameObject TurretPrefab;
    private GameObject currentTurret;

    public void TurretInstall(Vector3 targetPosition)
    {
        // 전달받은 위치에 터렛 생성
        Vector3 direction = Vector3.zero;
        
        currentTurret = Instantiate(TurretPrefab, targetPosition, Quaternion.identity);
        SoundManager.Instance.PlaySFX(SFXSoundType.Skill_Turret);

        TurretObject pr = currentTurret.GetComponent<TurretObject>();
        if (currentTurret != null)
        {
            pr.Initialize(direction, damage);
        }
    }
    
    public override SkillState GetInitialState()
    {
        return new TurretPreparingState(this);
    }
}