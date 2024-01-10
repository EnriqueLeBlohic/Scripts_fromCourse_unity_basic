using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    [SerializeField] int damagePoints = 10;
    [SerializeField] TagId targetTag;
    private Collider2D collider2D;

    private void Awake()
    {
        collider2D = GetComponent<Collider2D>();
        collider2D.enabled = false;
    }

 
    public void Attack(float delay, float attackDuration){

        //collider2D.enabled = true;
        StartCoroutine(_Attack(delay, attackDuration));
    }
    private IEnumerator _Attack(float delay, float attackDuration){
        yield return new WaitForSeconds(delay);
        collider2D.enabled = true;

        yield return new WaitForSeconds(attackDuration);

        collider2D.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision){
         if (collision.gameObject.tag.Equals(targetTag.ToString())){
             Debug.Log("Collision with " + collision.gameObject.tag);
             var component = collision.gameObject.GetComponent<ITargetCombat>();
             if(component != null){
                component.TakeDamage(damagePoints);
             }
         }
    }
}
