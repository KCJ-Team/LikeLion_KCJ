using System.Collections;
using UnityEngine;

public class MissileFiringState : SkillState
{
    private MissileBombing _missileBombing;
    private bool effectCreated = false;
    private Vector3 targetPosition;
    private Coroutine shakeCoroutine;
    
    public MissileFiringState(Skill skill, Vector3 target) : base(skill)
    {
        _missileBombing = skill as MissileBombing;
        targetPosition = target;
    }

    public override void EnterState()
    {
        shakeCoroutine = _missileBombing.StartCoroutine(RepeatShakeCamera());
    }
    
    public override void UpdateState()
    {
        if (!effectCreated)
        {
            _missileBombing.CreateEffectWithDamage(targetPosition);
            effectCreated = true;
        }

        // 이펙트 생성 후 스킬 오브젝트 파괴
        if (effectCreated)
        {
            Object.Destroy(_missileBombing.gameObject,5f);
        }
    }
    
    public override void ExitState() 
    {
        
    }

    private IEnumerator RepeatShakeCamera()
    {
        float elapsedTime = 0f;
        float duration = 5f; // 총 지속시간 3초
        float interval = 0.5f; // 흔들기 간격 0.5초

        yield return new WaitForSeconds(1f);
        
        while (elapsedTime < duration)
        {
            CameraShaking.Instance.OnShakeCamera(0.1f, 0.1f); // 0.2초 동안 0.1 강도로 흔들기
            yield return new WaitForSeconds(interval);
            elapsedTime += interval;
        }
    }
}