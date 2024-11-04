using System;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public PlayerData playerData;
    public WeaponManager weaponManager;
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

    private void Awake()
    {
        _animator = GetComponent<Animator>();
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
                weaponManager.Attack();
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
    
    public void SetSkill(Skill skill)
    {
        currentSkill = skill;
        skill.Initialize(this);
    }
    
    public void UseSkill()
    {
        if (currentSkill != null)
        {
            currentSkill.Execute();
        }
    }
    
    private void HandleSkillInputs()
    {
        if (Input.GetKeyDown(KeyCode.Q) && playerData.currentQSkill != null)
        {
            CreateAndUseSkill(playerData.currentQSkill.skillData.gameObject);
        }
        
        if (Input.GetKeyDown(KeyCode.E) && playerData.currentESkill != null)
        {
            CreateAndUseSkill(playerData.currentESkill.skillData.gameObject);
        }
        
        if (Input.GetKeyDown(KeyCode.LeftShift) && playerData.currentBuff != null)
        {
            CreateAndUseSkill(playerData.currentBuff.buffData.gameObject);
        }
    }

    private void CreateAndUseSkill(GameObject skillPrefab)
    {
        GameObject skillObject = Instantiate(skillPrefab, transform.position, transform.rotation);
        Skill skill = skillObject.GetComponent<Skill>();
        SetSkill(skill);
        UseSkill();
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