using System;
using UnityEngine;

public class MonsterTargetManager : MonoBehaviour
{
    public static event Action<GameObject> OnTargetChanged;

    public static void ChangeAllMonsterTargets(GameObject newTarget)
    {
        OnTargetChanged?.Invoke(newTarget);
    }
}