using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MovementInfo
{
    public Vector3 Movement { get; set; }
    public bool IsAimLocked { get; set; }
    public bool IsSkillActive { get; set; }
}
