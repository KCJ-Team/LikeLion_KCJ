using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DOTWeenCustomHandler : MonoBehaviour
{
    private DOTweenAnimation[] dotweenAnimations;

    private void Awake()
    {
        // Cache all DOTweenAnimation components on this GameObject
        dotweenAnimations = GetComponents<DOTweenAnimation>();
    }

    private void OnEnable()
    {
        // Play all DOTweenAnimations on enable
        foreach (var animation in dotweenAnimations)
        {
            animation.DOPlayForward();
        }
    }

    private void OnDisable()
    {
        // Pause all DOTweenAnimations on disable
        foreach (var animation in dotweenAnimations)
        {
            animation.DORewind();
        }
    }
}
