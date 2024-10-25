using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public PlayerData playerData;
    public WeaponManager weaponManager;
    [SerializeField] private Skill currentSkill;
    [SerializeField] private BuffSkill currentBuff;
    public float currentSpeed;
    private Status _status;
    
    private Vector3 movement;
    private Transform cameraTransform;
    private Vector3 mouseDirection;
    
    public CardObject card1;
    public CardObject card2;
    
    private void Start()
    {
        cameraTransform = Camera.main.transform;
        
        playerData.inventory.Clear();
        playerData.equipment.Clear();
    }

    private void Update()
    {
        HandleInput();
        Move();
        
        if (Input.GetMouseButton(0))
        {
            LookAtMouse();
            weaponManager.Attack();
        }
        else
        {
            RotateTowardsMovement();
        }
        
        if (Input.GetKeyDown(KeyCode.F))
        {
            //인벤토리에 추가하는 방법
            playerData.inventory.AddItem(new Card(card1), 1);
        }
        
        if (Input.GetKeyDown(KeyCode.G))
        {
            //인벤토리에 추가하는 방법
            playerData.inventory.AddItem(new Card(card2), 1);
        }
        
        if (Input.GetKeyDown(KeyCode.LeftShift) && playerData.currentBuff != null)
        {
            GameObject buffObject = Instantiate(playerData.currentBuff.buffData.gameObject, transform.position, transform.rotation);
            Skill buffSkill = buffObject.GetComponent<Skill>();
            SetSkill(buffSkill);
            UseSkill();
        }
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
            Vector3 cameraForward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;
            Vector3 cameraRight = Vector3.Cross(Vector3.up, cameraForward);
            Vector3 moveDirection = cameraForward * movement.z + cameraRight * movement.x;

            transform.position += moveDirection * (playerData.MoveSpeed * Time.deltaTime);
        }
    }

    private void RotateTowardsMovement()
    {
        if (movement != Vector3.zero)
        {
            Vector3 cameraForward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;
            Vector3 cameraRight = Vector3.Cross(Vector3.up, cameraForward);
            Vector3 targetDirection = cameraForward * movement.z + cameraRight * movement.x;

            transform.rotation = Quaternion.LookRotation(targetDirection);
        }
    }

    private void LookAtMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayDistance;

        if (groundPlane.Raycast(ray, out rayDistance))
        {
            Vector3 pointToLook = ray.GetPoint(rayDistance);
            mouseDirection = pointToLook - transform.position;
            mouseDirection.y = 0f; // 수직 회전 방지
            mouseDirection.Normalize();

            if (mouseDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(mouseDirection);
            }
        }
    }
    
    //스킬
    public void SetSkill(Skill skill)
    {
        currentSkill = skill;
        skill.Initialize(this);
    }
    
    public void SetBuff(BuffSkill buff)
    {
        currentBuff = buff;
        buff.Initialize(this);
    }
    
    public void UseSkill()
    {
        if (currentSkill != null)
        {
            currentSkill.Execute();
        }
    }
    
    public void UseBuff()
    {
        if (currentBuff != null)
        {
            currentBuff.Execute();
        }
    }
}