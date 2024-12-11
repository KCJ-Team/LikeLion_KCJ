using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invisibility : Skill 
{
    public float duration;
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    private void OnEnable()
    {
        spriteRenderer = GameManager.Instance.Player.GetComponent<SpriteRenderer>();
    }

    public void Invisible()
    {
        StartCoroutine(InvisibleCoroutine());
    }
    
    private IEnumerator InvisibleCoroutine()
    {
        GameManager.Instance.playerData.IsInvisable = true;
        
        Color currentColor = spriteRenderer.color;

        spriteRenderer.color = new Color(currentColor.r, currentColor.g, currentColor.b, 0.3f);
        
        yield return new WaitForSeconds(duration);
        
        GameManager.Instance.playerData.IsInvisable = false;
        
        spriteRenderer.color = new Color(currentColor.r, currentColor.g, currentColor.b, 1);
        
        Destroy(gameObject);
    }
    
    public override SkillState GetInitialState()
    {
        return new InvisibilityState(this);
    }
}