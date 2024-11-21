using System;
using UnityEngine;

public class ShadowClone : Skill
{
    [SerializeField] private float cloneDuration = 3f;
    [SerializeField] private float moveSpeed = 20f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private RuntimeAnimatorController cloneAnimatorController;
    [SerializeField] private float spawnDistance = 2f;
    
    private WeaponType _currentWeaponType;
    private PlayerCharacterController _player;
    
    private GameObject cloneObject;
    private float remainingDuration = 0f;
    private Vector3 targetPosition;
    private bool isMoving = false;
    private Animator cloneAnimator;

    private void Awake()
    {
        _player = GameManager.Instance.Player.GetComponent<PlayerCharacterController>();
    }

    protected override void Update()
    {
        base.Update();

        if (remainingDuration > 0)
        {
            remainingDuration -= Time.deltaTime;
            if (remainingDuration <= 0)
            {
                DestroyClone();
                Destroy(gameObject);
            }
        }

        if (isMoving && cloneObject != null)
        {
            // Run 애니메이션 재생
            cloneAnimator.SetBool("IsRunning", true);

            float step = moveSpeed * Time.deltaTime;
            Vector3 currentPos = cloneObject.transform.position;
            Vector3 targetPos = new Vector3(targetPosition.x, currentPos.y, targetPosition.z);
            
            Vector3 direction = (targetPos - currentPos).normalized;
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                cloneObject.transform.rotation = Quaternion.Slerp(
                    cloneObject.transform.rotation,
                    targetRotation,
                    10f * Time.deltaTime
                );
            }
            
            cloneObject.transform.position = Vector3.MoveTowards(currentPos, targetPos, step);

            if (Vector2.Distance(
                    new Vector2(currentPos.x, currentPos.z), 
                    new Vector2(targetPos.x, targetPos.z)) < 0.1f)
            {
                isMoving = false;
                cloneAnimator.SetBool("IsRunning", false);
            }
        }
    }

    public void CreateClone()
    {
        if (CanUseSkill())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
            {
                GameObject player = GameManager.Instance.Player;
                
                Vector3 playerForward = player.transform.forward;
                Vector3 spawnPosition = player.transform.position + (playerForward * spawnDistance);
                
                //Vector3 spawnPosition = player.transform.position;
                targetPosition = new Vector3(hit.point.x, spawnPosition.y, hit.point.z);
                
                // 분신 생성 시점의 무기 타입 저장
                _currentWeaponType = _player.playerData.currentWeapon.weaponType;
                
                cloneObject = Instantiate(player, spawnPosition, player.transform.rotation);
                
                SoundManager.Instance.PlaySFX(SFXSoundType.Skill_Shadow);
                
                RemoveUnnecessaryComponents(cloneObject);
                
                cloneAnimator = cloneObject.GetComponent<Animator>();
                
                if (cloneAnimator != null && cloneAnimatorController != null)
                {
                    cloneAnimator.runtimeAnimatorController = cloneAnimatorController;
                    // 저장된 무기 타입으로 애니메이터 파라미터 즉시 설정
                    cloneAnimator.SetInteger("WeaponType", (int)_currentWeaponType);
                }
                
                SetupClone(cloneObject);
                
                isMoving = true;
                remainingDuration = cloneDuration;
                currentCooldown = cooldown;
            }
        }
    }

    private void RemoveUnnecessaryComponents(GameObject clone)
    {
        Destroy(clone.GetComponent<PlayerCharacterController>());
        Destroy(clone.GetComponent<Status>());
        Destroy(clone.GetComponent<Rigidbody>());
        Destroy(clone.GetComponent<WeaponManager>());
        Destroy(clone.GetComponent<EquipmentManager>());
        Destroy(clone.GetComponent<PlayerDamageable>());
        Destroy(clone.GetComponent<PlayerHealth>());
        Destroy(clone.GetComponent<CapsuleCollider>());
    }

    private void SetupClone(GameObject clone)
    {
        clone.name = "Shadow Clone";
        //clone.GetComponent<CapsuleCollider>().isTrigger = true;
    }

    private void DestroyClone()
    {
        if (cloneObject != null)
        {
            Destroy(cloneObject);
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