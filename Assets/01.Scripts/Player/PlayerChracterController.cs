using UnityEngine;

public class PlayerCharacterController : MonoBehaviour
{
    [Header("Components")]
    public PlayerData playerData;
    private WeaponManager _weaponManager;
    private Health _health;
    private Animator _animator;
    
    // Sub-controllers
    private PlayerMovementController _movementController;
    private PlayerCombatController _combatController;
    private PlayerSkillController _skillController;
    private PlayerAnimationController _animationController;
    private PlayerInventoryController _inventoryController;

    private void Awake()
    {
        InitializeComponents();
        InitializeControllers();
    }

    private void InitializeComponents()
    {
        _animator = GetComponent<Animator>();
        _weaponManager = GetComponent<WeaponManager>();
        _health = GetComponent<Health>();
        
        if (_health != null)
        {
            _health.OnHealthDepleted.AddListener(Die);
        }
    }

    private void InitializeControllers()
    {
        _movementController = new PlayerMovementController(this, transform, _animator);
        _combatController = new PlayerCombatController(this, _weaponManager);
        _skillController = new PlayerSkillController(this, playerData);
        _animationController = new PlayerAnimationController(_animator);
        _inventoryController = new PlayerInventoryController(playerData);
    }

    private void Start()
    {
        _movementController.Initialize();
        _inventoryController.Initialize();
        _combatController.UpdateWeaponAnimation();
    }

    private void Update()
    {
        if (_health.IsDead) return;

        UpdateControllers();
    }

    private void UpdateControllers()
    {
        _movementController.HandleMovement();
        _combatController.HandleCombat();
        _skillController.HandleSkills();
        _animationController.UpdateAnimations(_movementController.MovementInfo);
        _inventoryController.HandleInventory();
    }

    private void Die()
    {
        _health.IsDead = true;
        _animationController.TriggerDeathAnimation();
        _skillController.CleanupSkills();
        DisableComponents();
    }

    private void DisableComponents()
    {
        if (TryGetComponent<Collider>(out var col))
        {
            col.enabled = false;
        }
        
        if (TryGetComponent<Rigidbody>(out var rb))
        {
            rb.isKinematic = true;
        }
        
        if (_weaponManager != null)
        {
            _weaponManager.enabled = false;
        }
    }

    private void OnDestroy()
    {
        _skillController.CleanupSkills();
    }
}