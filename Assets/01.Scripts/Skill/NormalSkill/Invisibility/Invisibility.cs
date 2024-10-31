using UnityEngine;

public class Invisibility : Skill 
{
    [SerializeField] private Material cloakingMat;
    public float duration = 5f;

    private Material originalMat;
    private bool isInvisible = false;
    private MeshRenderer playerRenderer;
    private float remainingDuration;

    private void Awake()
    {
        playerRenderer = GameManager.Instance.Player.GetComponent<MeshRenderer>();
        
        if (playerRenderer.material != null)
        {
            originalMat = playerRenderer.material;
        }
    }

    private void Update()
    {
        if (currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;
        }

        // 지속시간 카운트다운을 여기서도 체크
        if (isInvisible)
        {
            remainingDuration -= Time.deltaTime;

            if (remainingDuration <= 0)
            {
                Visible();
            }
        }
    }

    public void Invisible()
    {
        if (currentCooldown <= 0 && !isInvisible && playerRenderer != null && cloakingMat != null)
        {
            originalMat = new Material(playerRenderer.material);
            playerRenderer.material = cloakingMat;
            isInvisible = true;
            remainingDuration = duration; // 지속시간 초기화
        }
    }

    public void Visible()
    {
        if (isInvisible && playerRenderer != null && originalMat != null)
        {
            playerRenderer.material = originalMat;
            isInvisible = false;
            currentCooldown = cooldown;
            remainingDuration = 0f;
        }
    }

    public bool CanUseSkill()
    {
        return currentCooldown <= 0 && !isInvisible && 
               playerRenderer != null && cloakingMat != null;
    }

    public bool IsInvisible()
    {
        return isInvisible;
    }

    public float GetRemainingDuration()
    {
        return remainingDuration;
    }
    
    public override SkillState GetInitialState()
    {
        return new InvisibilityState(this);
    }
}