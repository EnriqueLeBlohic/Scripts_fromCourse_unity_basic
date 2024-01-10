using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageController : MonoBehaviour
{
    [SerializeField] int damagePoints = 10;
    [SerializeField] TagId targetTag;

    private void Awake()
    {
        
    }

 

    private void OnTriggerEnter2D(Collider2D collision){
         if (collision.gameObject.tag.Equals(targetTag.ToString())){
            
             var component = collision.gameObject.GetComponent<ITargetCombat>();
             if(component != null){
                component.TakeDamage(damagePoints);
             }
         }
    }
}
