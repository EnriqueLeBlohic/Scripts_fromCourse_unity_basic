using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    [SerializeField] float delay = 3;

    private void Awake(){
        Destroy(this.gameObject, delay);
    }
}
