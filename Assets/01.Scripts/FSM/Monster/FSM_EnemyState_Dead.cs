using UnityEngine;
using System.Collections;

public class FSM_EnemyState_Dead : VMyState<FSM_EnemyState>
{
    private FSM_Enemy enemy;
    private Animator _animator;
    private MonsterHealth _monsterHealth;
    private BoxCollider _collider;
    private Rigidbody _rigidbody;
    
    [Header("Dissolve Effect Settings")]
    [SerializeField] private Material dissolveMaterial;  // 디졸브 메테리얼
    [SerializeField] private float dissolveTime = 5f;    // 디졸브 효과 지속시간
    [SerializeField] private string dissolveParamName = "_DissolveAmount"; // 쉐이더 파라미터 이름
    
    private SkinnedMeshRenderer[] meshRenderers;
    private Material[] originalMaterials;
    
    public override FSM_EnemyState StateEnum => FSM_EnemyState.FSM_EnemyState_Dead;

    protected override void Awake()
    {
        base.Awake();
        enemy = GetComponent<FSM_Enemy>();
        _animator = GetComponent<Animator>();
        _monsterHealth = GetComponent<MonsterHealth>();
        _collider = GetComponent<BoxCollider>();
        _rigidbody = GetComponent<Rigidbody>();
        
        // 메시 렌더러 컴포넌트들 가져오기
        meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        
        // 원본 메테리얼 저장
        originalMaterials = new Material[meshRenderers.Length];
        for (int i = 0; i < meshRenderers.Length; i++)
        {
            originalMaterials[i] = meshRenderers[i].material;
        }
    }
    
    protected override void EnterState()
    {
        _animator.CrossFade(AnimationHash.DeadHash, 0.0f);
        _collider.enabled = false;
        _rigidbody.useGravity = false;
        StartCoroutine(DissolveEffect());
    }
    
    private IEnumerator DissolveEffect()
    {
        // 모든 메시 렌더러의 메테리얼을 디졸브 메테리얼로 교체
        foreach (var renderer in meshRenderers)
        {
            Material newMaterial = new Material(dissolveMaterial);
            renderer.material = newMaterial;
        }
        
        float elapsedTime = 0f;
        
        // 디졸브 효과 애니메이션
        while (elapsedTime < dissolveTime)
        {
            elapsedTime += Time.deltaTime;
            float dissolveAmount = elapsedTime / dissolveTime;
            
            // 모든 메시 렌더러의 디졸브 값 업데이트
            foreach (var renderer in meshRenderers)
            {
                renderer.material.SetFloat(dissolveParamName, dissolveAmount);
            }
            
            yield return null;
        }
        
        // 디졸브 효과가 완료되면 오브젝트 제거
        Destroy(gameObject);
    }
    
    private void OnDestroy()
    {
        // 메테리얼 정리
        if (meshRenderers != null)
        {
            for (int i = 0; i < meshRenderers.Length; i++)
            {
                if (meshRenderers[i] != null && originalMaterials[i] != null)
                {
                    meshRenderers[i].material = originalMaterials[i];
                }
            }
        }
    }
}