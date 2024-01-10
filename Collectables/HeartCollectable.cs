using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartCollectable : MonoBehaviour
{
    
    [SerializeField] TagId playerTag;
    [SerializeField] TagId groundTag;
    [SerializeField] AudioClip pickSfx;
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.tag.Equals(groundTag.ToString())){
            GetComponent<Rigidbody2D>().gravityScale = 0;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }

         if (collision.gameObject.tag.Equals(playerTag.ToString())){
            HeroController.instance.GiveHealthPoint();
            AudioManager.instance.PlaySfx(pickSfx);
            Destroy(this.gameObject);
         }
    }
}
