using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : Skill
{
    [SerializeField] private GameObject MinePrefab;
    private GameObject currentMine;

    public void MineInstall(Vector3 targetPosition)
    {
        // 전달받은 위치에 터렛 생성
        currentMine = Instantiate(MinePrefab, targetPosition, Quaternion.identity);

        Vector3 direction = Vector3.zero;
        
        Projectile pr = currentMine.GetComponent<Projectile>();

        if (pr != null)
        {
            pr.Initialize(direction, damage);
        }
    }
    
    public override SkillState GetInitialState()
    {
        return new MinePreparingState(this);
    }
}
