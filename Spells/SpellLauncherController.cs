using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellLauncherController : MonoBehaviour
{
    [SerializeField] GameObject proyectilePrefab;
    [SerializeField] float force;

    public void Launch(Vector2 direction){
        GameObject go = Instantiate(proyectilePrefab, this.transform.position, Quaternion.identity);
        go.GetComponent<Rigidbody2D>().AddForce(direction*force, ForceMode2D.Impulse);
        go.GetComponent<ProyectileController>().SetDirection(direction);

        
    }
}
