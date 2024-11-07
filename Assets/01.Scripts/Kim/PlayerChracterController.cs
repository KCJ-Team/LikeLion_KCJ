using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterController : MonoBehaviour
{
    public PlayerData playerData;
    public WeaponManager _weaponManager;
    [SerializeField] private Skill currentSkill;
    private float rotationSpeed = 10f;

    private Animator _animator;
    private Vector3 movement;
    private Transform cameraTransform;
    private Vector3 mouseDirection;
    private Quaternion targetRotation;
    private bool isAimLocked = false;
    
    private readonly string IS_RUNNING = "IsRunning";
    private readonly string IS_AIM = "IsAim";
    private readonly string MovementX = "MovementX";
    private readonly string MovementY = "MovementY";
    private readonly string WEAPON_TYPE = "WeaponType";
    private readonly string ATTACK_TRIGGER = "Attack";
    
    private WeaponType currentWeaponType = WeaponType.None;
    
    private Dictionary<KeyCode, Skill> activeSkills = new Dictionary<KeyCode, Skill>();
    private Dictionary<KeyCode, float> skillCooldowns = new Dictionary<KeyCode, float>();

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _weaponManager = GetComponent<WeaponManager>();
    }

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        targetRotation = transform.rotation;
        
        playerData.inventory.Clear();
        playerData.equipment.Clear();
        
        UpdateWeaponAnimation();
    }

    private void Update()
    {
        HandleInput();
        Move();
        
        isAimLocked = Input.GetMouseButton(1);
        
        UpdateAnimations();
        UpdateWeaponAnimation();
        
        if (isAimLocked)
        {
            LookAtMouse();
            
            if (Input.GetMouseButton(0))
            {
                _weaponManager.Attack();
            }
        }
        else
        {
            RotateTowardsMovement();
        }
        
        HandleSkillInputs();
        InvenToCard();
    }

    private void UpdateWeaponAnimation()
    {
        // 현재 장착된 무기가 있는지 확인
        if (playerData.currentWeapon != null)
        {
            currentWeaponType = playerData.currentWeapon.weaponType;
        }
        else
        {
            currentWeaponType = WeaponType.None;
        }
        
        // Animator의 WeaponType 파라미터 업데이트
        _animator.SetInteger(WEAPON_TYPE, (int)currentWeaponType);
    }
    
    private void HandleInput()
    {
        Vector2 input = InputManager.Instance.GetMovementInput();
        movement = new Vector3(input.x, 0f, input.y);
    }

    private void Move()
    {
        if (movement.magnitude >= 0.1f)
        {
            Vector3 moveDirection;
            
            if (isAimLocked)
            {
                Vector3 cameraForward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;
                Vector3 cameraRight = Vector3.Cross(Vector3.up, cameraForward);
                moveDirection = cameraForward * movement.z + cameraRight * movement.x;
            }
            else
            {
                Vector3 cameraForward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;
                Vector3 cameraRight = Vector3.Cross(Vector3.up, cameraForward);
                moveDirection = cameraForward * movement.z + cameraRight * movement.x;
            }

            transform.position += moveDirection * (playerData.MoveSpeed * Time.deltaTime);
        }
        
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    private void UpdateAnimations()
    {
        if (isAimLocked)
        {
            Vector3 localMovement = transform.InverseTransformDirection(
                new Vector3(movement.x, 0, movement.z)
            );
            
            Vector3 cameraForward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;
            Vector3 cameraRight = Vector3.Cross(Vector3.up, cameraForward);
            Vector3 worldMovement = cameraForward * movement.z + cameraRight * movement.x;
            
            Vector3 characterLocalMovement = transform.InverseTransformDirection(worldMovement);
            
            _animator.SetFloat(MovementX, characterLocalMovement.x);
            _animator.SetFloat(MovementY, characterLocalMovement.z);
        }
        else
        {
            _animator.SetFloat(IS_RUNNING, movement.magnitude);
        }
        
        _animator.SetBool(IS_AIM, isAimLocked);
    }

    private void RotateTowardsMovement()
    {
        if (movement != Vector3.zero)
        {
            Vector3 cameraForward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;
            Vector3 cameraRight = Vector3.Cross(Vector3.up, cameraForward);
            Vector3 targetDirection = cameraForward * movement.z + cameraRight * movement.x;

            targetRotation = Quaternion.LookRotation(targetDirection);
        }
    }

    private void LookAtMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        if (groundPlane.Raycast(ray, out float rayDistance))
        {
            Vector3 pointToLook = ray.GetPoint(rayDistance);
            mouseDirection = pointToLook - transform.position;
            mouseDirection.y = 0f;
            mouseDirection.Normalize();

            if (mouseDirection != Vector3.zero)
            {
                targetRotation = Quaternion.LookRotation(mouseDirection);
            }
        }
    }
    
    //스킬
    public void SetSkill(Skill skill)
    {
        currentSkill = skill;
        skill.Initialize(this);
    }
    
    private void HandleSkillInputs()
    {
        // Q 스킬
        if (Input.GetKeyDown(KeyCode.Q))
        {
            UseSkillWithKey(KeyCode.Q, playerData.currentQSkill?.skillData.gameObject);
        }
        
        // E 스킬
        if (Input.GetKeyDown(KeyCode.E))
        {
            UseSkillWithKey(KeyCode.E, playerData.currentESkill?.skillData.gameObject);
        }
        
        // Shift 버프 스킬
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            UseSkillWithKey(KeyCode.LeftShift, playerData.currentBuff?.buffData.gameObject);
        }
        
        // 쿨다운 업데이트
        List<KeyCode> keysToUpdate = new List<KeyCode>(skillCooldowns.Keys);
        foreach (KeyCode key in keysToUpdate)
        {
            if (skillCooldowns[key] > 0)
            {
                skillCooldowns[key] -= Time.deltaTime;
                if (skillCooldowns[key] <= 0)
                {
                    skillCooldowns.Remove(key);
                }
            }
        }
    }
    
    private void UseSkillWithKey(KeyCode key, GameObject skillPrefab)
    {
        if (skillPrefab == null) return;
        
        // 쿨다운 체크
        if (skillCooldowns.TryGetValue(key, out float remainingCooldown) && remainingCooldown > 0)
        {
            return; // 아직 쿨다운 중이면 실행하지 않음
        }
        
        // 새로운 스킬 생성
        GameObject skillObject = Instantiate(skillPrefab, transform.position, transform.rotation);
        Skill skill = skillObject.GetComponent<Skill>();
        skill.Initialize(this);
        
        // 스킬 실행
        if (skill.TryExecute())
        {
            // 쿨다운 설정
            skillCooldowns[key] = skill.Cooldown;
        }
    }
    
    // 쿨다운 관련 유틸리티 메서드들
    public bool IsSkillOnCooldown(KeyCode key)
    {
        return skillCooldowns.TryGetValue(key, out float cooldown) && cooldown > 0;
    }
    
    public float GetSkillCooldown(KeyCode key)
    {
        return skillCooldowns.TryGetValue(key, out float cooldown) ? cooldown : 0f;
    }
    
    private void OnDestroy()
    {
        foreach (var skill in activeSkills.Values)
        {
            if (skill != null && skill.gameObject != null)
            {
                Destroy(skill.gameObject);
            }
        }
        activeSkills.Clear();
    }
    
    
    //테스트용 메소드(삭제예정)
    [Header("삭제예정")]
    public CardObject card;

    private void InvenToCard()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            playerData.inventory.AddItem(new Card(card), 1);
        }
    }
}