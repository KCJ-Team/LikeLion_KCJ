using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FSM_MonsterState
{
    None,
    FSM_Monster_Idle,
    FSM_Monster_Move,
    FSM_Monster_RangeAttack,
    FSM_Monster_Skill,
    FSM_Monster_Dead
}

public class FSM_Monster : StateMachine<FSM_MonsterState>
{
    
}