using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//위치 노드 Scene뷰에서 보이게 
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
