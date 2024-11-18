using UnityEngine;

public class BuffSkillState : SkillState
{
    private BuffSkill buffSkill;         // 버프 스킬 참조
    private Status targetStatus;         // 버프를 적용할 대상의 Status 컴포넌트
    private GameObject buffObject;        // 버프 스킬 오브젝트
    private float buffEndTime;           // 버프 종료 시간
    private bool isBuffActive = false;   // 버프 활성화 상태
    
    public BuffSkillState(Skill skill) : base(skill)
    {
        buffSkill = skill as BuffSkill;
        buffObject = buffSkill.gameObject;
    }
    
    // 상태 진입 시 버프 적용
    public override void EnterState()
    {
        // 플레이어의 Status 컴포넌트 가져오기
        targetStatus = GameManager.Instance.Player.GetComponent<Status>();
    
        if (targetStatus != null)
        {
            // 버프 이펙트 생성
            if (buffSkill.EffectPrefab != null)
            {
                GameObject effectObject = Object.Instantiate(
                    buffSkill.EffectPrefab,
                    GameManager.Instance.Player.transform.position,
                    Quaternion.Euler(-90f, 0f, 0f),  // x: -90, y: 0, z: 0 회전 적용
                    GameManager.Instance.Player.transform
                );
            
                // 버프 지속시간과 함께 이펙트도 제거되도록 Destroy 호출
                Object.Destroy(effectObject, buffSkill.skillDuration);
            }

            // 새로운 버프 생성
            Buff newBuff = new Buff(
                buffSkill.GetBuffId(),
                buffSkill.GetBuffName(),
                buffSkill.skillDuration,
                buffSkill.buffEffects
            );
        
            // 대상에게 버프 적용
            targetStatus.AddBuff(newBuff);
        
            // 버프 종료 시간 설정
            buffEndTime = Time.time + buffSkill.skillDuration;
            isBuffActive = true;
        }
    }
    
    // 버프 지속시간 체크 및 오브젝트 삭제
    public override void UpdateState()
    {
        if (isBuffActive && Time.time >= buffEndTime)
        {
            // 버프 제거
            targetStatus.RemoveBuff(buffSkill.GetBuffId());
            isBuffActive = false;
            
            // 오브젝트 삭제
            if (buffObject != null)
            {
                Object.Destroy(buffObject);
            }
            
            // 상태 종료
            skill.ChangeState(null);
        }
    }

    public override void ExitState()
    {
        
    }
}