using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MissileBombardment))]
public class MissileSkillTest : MonoBehaviour
{
    private Character character;
    private MissileBombardment missileSkill;

    void Start()
    {
        // Character 컴포넌트 가져오기
        character = GetComponent<Character>();
        
        // MissileBombardment 스킬 초기화
        missileSkill = GetComponent<MissileBombardment>();
        
        // 캐릭터에 미사일 스킬 할당
        character.SetSkill(missileSkill);
    }

    void Update()
    {
        // 스페이스바를 누르면 스킬 실행
        if (Input.GetKeyDown(KeyCode.Space))
        {
            character.UseSkill();
        }
    }
}
