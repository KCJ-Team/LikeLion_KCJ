using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDecteting : MonoBehaviour
{
    public float detectingRange;
    public LayerMask targetLayer;
    public RaycastHit2D[] targets;
    public Transform nearestTarget;

    private void Start()
    {
        detectingRange = GameManager.Instance.playerData.currentWeapon.attackRange;
    }

    private void FixedUpdate()
    {
        DetectingTargets();
    }

    public void DetectingTargets()
    {
        targets = Physics2D.CircleCastAll(transform.position, detectingRange, Vector2.zero, 0, targetLayer);

        nearestTarget = GetNearest();
    }

    private Transform GetNearest()
    {
        Transform result = null;
        float diff = 100;

        foreach (RaycastHit2D target in targets)
        {
            Vector3 myPos = transform.position;
            Vector3 targetPos = target.transform.position;
            float curDiff = Vector3.Distance(myPos, targetPos);

            if (curDiff < diff)
            {
                diff = curDiff;
                result = target.transform;
            }
        }

        return result;
    }
}