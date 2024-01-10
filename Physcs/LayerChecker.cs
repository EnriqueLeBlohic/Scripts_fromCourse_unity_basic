using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerChecker : MonoBehaviour
{
    
    [SerializeField] LayerMask targetMask;
    [SerializeField] Vector2 direction;
    [SerializeField] float distance;
    public bool isTouching;

  
    void Update()
    {
        isTouching=Physics2D.Raycast(this.transform.position, direction, distance, targetMask);
    }
#if UNITY_EDITOR
    private void OnDrawGizmos(){
        if (isTouching){
            Gizmos.color=Color.green;
        }
        else{
            Gizmos.color=Color.yellow;
        }

        Gizmos.DrawRay(this.transform.position, direction * distance);
    }
#endif
}
