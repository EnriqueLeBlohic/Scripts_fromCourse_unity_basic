using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WayPointsManager : MonoBehaviour
{

    int indexPoint = 0;

    public Vector2 GetNextPoint(){

        if (indexPoint >= this.transform.childCount){
            indexPoint=0;
        }
      
        var position=this.transform.GetChild(indexPoint).transform.position;
        indexPoint++;
        return position;
    }
#if UNITY_EDITOR
    private void OnDrawGizmos(){
        for (int i=0; i < this.transform.childCount; i++){
            Gizmos.DrawWireSphere(this.transform.GetChild(i).transform.position, 0.3f);
            Handles.Label(this.transform.GetChild(i).transform.position," Point"+(i+1));
        }
    }
#endif
}
