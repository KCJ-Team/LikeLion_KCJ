using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPlant : BaseBuilding
{
    public override void Build() {
        base.Build();
    }

    public override void Upgrade() {
        base.Upgrade();
    }

    public override int GetProductOutput() {
        return base.GetProductOutput(); // 필요에 따라 추가 생산 로직 작성 가능
    }
}
