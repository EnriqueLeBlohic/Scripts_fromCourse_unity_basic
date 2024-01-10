using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]

public class BoundaryRange {
    public float min;
    public float max;
}

[Serializable]

public class CameraSize {
    public float min;
    public float max;
}

public class CameraController : MonoBehaviour
{

    [SerializeField] TagId target;
    [SerializeField] float offsetZ;
    [SerializeField] BoundaryRange boundaryX;
    [SerializeField] BoundaryRange boundaryY;

    private GameObject targetGameObject;

    CameraSize cameraSizeX;
    CameraSize cameraSizeY;

    Vector3 vel;
    void Start()
    {
        targetGameObject=GameObject.FindGameObjectWithTag(target.ToString());
        cameraSizeX = new CameraSize();
        cameraSizeY = new CameraSize();

        cameraSizeX.min = Camera.main.ViewportToWorldPoint(new Vector3(0,0,0)).x -this.transform.position.x;
        cameraSizeX.max = Camera.main.ViewportToWorldPoint(new Vector3(1,0,0)).x -this.transform.position.x;

        cameraSizeY.min = Camera.main.ViewportToWorldPoint(new Vector3(0,0,0)).y -this.transform.position.y;
        cameraSizeY.max = Camera.main.ViewportToWorldPoint(new Vector3(0,1,0)).y -this.transform.position.y;
    }

    
    void Update()
    {
        if (targetGameObject){
            var targetPosition = new Vector3(
                    Mathf.Clamp(targetGameObject.transform.position.x, boundaryX.min - cameraSizeX.min, boundaryX.max - cameraSizeX.max),
                    Mathf.Clamp(targetGameObject.transform.position.y, boundaryY.min - cameraSizeY.min, boundaryY.max - cameraSizeY.max),
                    offsetZ
                );
            this.transform.position = Vector3.SmoothDamp(this.transform.position, targetPosition, ref vel, 0.3f);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos(){
        
        var pointA = new Vector3(boundaryX.min, boundaryY.min);
        var pointB = new Vector3(boundaryX.min, boundaryY.max);
        Gizmos.DrawLine(pointA, pointB);

        pointA = new Vector3(boundaryX.max, boundaryY.min);
        pointB = new Vector3(boundaryX.max, boundaryY.max);
        Gizmos.DrawLine(pointA, pointB);

        pointA = new Vector3(boundaryX.min, boundaryY.min);
        pointB = new Vector3(boundaryX.max, boundaryY.min);
        Gizmos.DrawLine(pointA, pointB);

        pointA = new Vector3(boundaryX.min, boundaryY.max);
        pointB = new Vector3(boundaryX.max, boundaryY.max);
        Gizmos.DrawLine(pointA, pointB);
    }
#endif
}