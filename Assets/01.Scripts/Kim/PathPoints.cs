using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathPoints : MonoBehaviour
{
    public Color gizmoColor = Color.red;
    public float gizmoRadius = 0.5f;
    
    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        
        Gizmos.DrawSphere(transform.position, gizmoRadius);
    }
}
