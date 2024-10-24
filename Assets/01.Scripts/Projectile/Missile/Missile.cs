using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : Projectile
{
    protected override ProjectileState GetInitialState()
    {
        return new MissileMovingState(this);
    }
}
