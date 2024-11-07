using System.Collections.Generic;
using UnityEngine;

public class Invisibility : Skill 
{
    [SerializeField] private Material cloakingMat;
    public float duration = 5f;

    private List<RendererInfo> renderersInfo = new List<RendererInfo>();
    private bool isInvisible = false;
    private float remainingDuration;

    private class RendererInfo
    {
        public SkinnedMeshRenderer Renderer;
        public Material OriginalMaterial;

        public RendererInfo(SkinnedMeshRenderer renderer, Material originalMaterial)
        {
            Renderer = renderer;
            OriginalMaterial = originalMaterial;
        }
    }

    private void Awake()
    {
        SkinnedMeshRenderer[] allRenderers = GameManager.Instance.Player.GetComponentsInChildren<SkinnedMeshRenderer>();
        
        foreach (SkinnedMeshRenderer renderer in allRenderers)
        {
            if (renderer.material != null)
            {
                renderersInfo.Add(new RendererInfo(renderer, new Material(renderer.material)));
            }
        }
    }

    protected override void Update()
    {
        base.Update();
        
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
        if (CanUseSkill())
        {
            foreach (var rendererInfo in renderersInfo)
            {
                if (rendererInfo.Renderer != null && cloakingMat != null)
                {
                    rendererInfo.Renderer.material = cloakingMat;
                }
            }
            isInvisible = true;
            remainingDuration = duration;
        }
    }

    public void Visible()
    {
        if (isInvisible)
        {
            foreach (var rendererInfo in renderersInfo)
            {
                if (rendererInfo.Renderer != null && rendererInfo.OriginalMaterial != null)
                {
                    rendererInfo.Renderer.material = rendererInfo.OriginalMaterial;
                }
            }
            isInvisible = false;
            currentCooldown = cooldown;
            remainingDuration = 0f;
        }
    }

    public bool CanUseSkill()
    {
        return currentCooldown <= 0 && !isInvisible && 
               renderersInfo.Count > 0 && cloakingMat != null;
    }

    public override SkillState GetInitialState()
    {
        return new InvisibilityState(this);
    }
}