using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//몬스터 상태 추가
public enum FSM_EnemyState
{
    None,
    FSM_EnemyState_Idle
}

public class FSM_Enemy : StateMachine<FSM_EnemyState>
{
    
}
