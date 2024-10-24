using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public PlayerData playerData;
    public WeaponManager weaponManager;
    
    private Vector3 movement;
    private Transform cameraTransform;
    
    public CardObject card1;

    private Vector3 mouseDirection;

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
}