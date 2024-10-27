using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileObject : MonoBehaviour
{
    public float explosionRadius = 5f;
    public GameObject explosionEffectPrefab; // 폭발 이펙트 프리팹
    public LayerMask targetLayer; // 데미지를 줄 대상 레이어

    private void OnDrawGizmos()
    {
        // 디버그용: 폭발 반경 시각화
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}