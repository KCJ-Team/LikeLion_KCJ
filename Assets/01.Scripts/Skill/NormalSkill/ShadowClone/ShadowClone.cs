using UnityEngine;

public class ShadowClone : Skill
{
    [SerializeField] private float cloneDuration = 3f;
    [SerializeField] private float moveSpeed = 20f;
    [SerializeField] private LayerMask groundLayer;
    
    private GameObject cloneObject;
    private float remainingDuration = 0f;
    private Vector3 targetPosition;
    private bool isMoving = false;

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
            float step = moveSpeed * Time.deltaTime;
            Vector3 currentPos = cloneObject.transform.position;
            Vector3 targetPos = new Vector3(targetPosition.x, currentPos.y, targetPosition.z);
            
            cloneObject.transform.position = Vector3.MoveTowards(
                currentPos, 
                targetPos, 
                step
            );

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
        if (CanUseSkill())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
            {
                GameObject player = GameManager.Instance.Player;
                Vector3 spawnPosition = player.transform.position;
                targetPosition = new Vector3(hit.point.x, spawnPosition.y, hit.point.z);
                
                cloneObject = Instantiate(player, spawnPosition, player.transform.rotation);
                
                RemoveUnnecessaryComponents(cloneObject);
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
        
        foreach (Transform child in clone.transform)
        {
            Destroy(child.GetComponent<MonoBehaviour>());
        }
    }

    private void SetupClone(GameObject clone)
    {
        clone.name = "Shadow Clone";
        
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