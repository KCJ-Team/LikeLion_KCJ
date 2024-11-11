using System;
using UnityEngine;

public class StandardProjectile : Projectile
{
    [SerializeField]
    private LayerMask destructibleLayers; // Inspector에서 설정할 레이어

    protected Vector3 direction;
    protected Vector3 startPosition;
    
    public override void Initialize(Vector3 direction, float damage)
    {
        this.direction = direction;
        this.startPosition = transform.position;
        base.Initialize(direction, damage);
    }
    
    protected override ProjectileState GetInitialState()
    {
        return new ProjectileMovingState(this, direction);
    }

    private void OnTriggerEnter(Collider other)
    {
        // 충돌한 오브젝트의 레이어가 destructibleLayers에 포함되어 있는지 확인
        if (((1 << other.gameObject.layer) & destructibleLayers.value) != 0)
        {
            // 프로젝타일 파괴
            Destroy(gameObject);
        }
    }
}