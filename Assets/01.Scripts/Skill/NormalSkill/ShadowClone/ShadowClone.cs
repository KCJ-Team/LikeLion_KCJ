using UnityEngine;

public class ShadowClone : Skill
{
    [SerializeField] private float cloneDuration = 3f;
    [SerializeField] private float moveSpeed = 20f; // 분신 이동 속도
    [SerializeField] private LayerMask groundLayer; // 바닥 레이어
    
    private GameObject cloneObject;
    private float remainingDuration = 0f;
    private Vector3 targetPosition;
    private bool isMoving = false;

    private void Update()
    {
        // 쿨타임 감소
        if (currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;
        }

        // 분신 지속시간 관리
        if (remainingDuration > 0)
        {
            remainingDuration -= Time.deltaTime;
            if (remainingDuration <= 0)
            {
                DestroyClone();
            }
        }

        // 분신 이동 처리
        if (isMoving && cloneObject != null)
        {
            float step = moveSpeed * Time.deltaTime;
            Vector3 currentPos = cloneObject.transform.position;
            Vector3 targetPos = new Vector3(targetPosition.x, currentPos.y, targetPosition.z);
            
            cloneObject.transform.position = Vector3.MoveTowards(
                currentPos, 
                targetPos, 
                step
            );

            // 목표 지점에 도달했는지 체크 (x, z 평면에서만)
            if (Vector2.Distance(
                    new Vector2(currentPos.x, currentPos.z), 
                    new Vector2(targetPos.x, targetPos.z)) < 0.1f)
            {
                isMoving = false;
            }
        }
    }

    public void CreateClone()
    {
        if (currentCooldown <= 0)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
            {
                GameObject player = GameManager.Instance.Player;
                // 분신 생성 시 현재 플레이어의 y 위치 유지
                Vector3 spawnPosition = player.transform.position;
                targetPosition = new Vector3(hit.point.x, spawnPosition.y, hit.point.z);
                
                cloneObject = Instantiate(player, spawnPosition, player.transform.rotation);
                
                RemoveUnnecessaryComponents(cloneObject);
                SetupClone(cloneObject);
                
                isMoving = true;
                remainingDuration = cloneDuration;
                currentCooldown = cooldown;

                Debug.Log($"Clone created at {targetPosition}");
            }
        }
    }

    private void RemoveUnnecessaryComponents(GameObject clone)
    {
        // 플레이어 스크립트 등 불필요한 컴포넌트 제거 - 반드시 추가해야함!!!
        Destroy(clone.GetComponent<CharacterController>());
        Destroy(clone.GetComponent<Status>());
        // 자식 오브젝트의 스크립트도 제거
        foreach (Transform child in clone.transform)
        {
            Destroy(child.GetComponent<MonoBehaviour>());
        }
    }

    private void SetupClone(GameObject clone)
    {
        // 분신임을 표시하기 위해 이름 변경
        clone.name = "Shadow Clone";
        
        // 분신의 머터리얼을 반투명하게 설정
        MeshRenderer[] renderers = clone.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer renderer in renderers)
        {
            Material material = new Material(renderer.material);
            Color color = material.color;
            color.a = 0.5f;
            material.color = color;
            renderer.material = material;
        }
    }

    private void DestroyClone()
    {
        if (cloneObject != null)
        {
            Destroy(cloneObject);
            Debug.Log("Clone destroyed");
        }
    }

    public bool CanUseSkill()
    {
        return currentCooldown <= 0;
    }

    public override SkillState GetInitialState()
    {
        return new ShadowCloneState(this);
    }
}