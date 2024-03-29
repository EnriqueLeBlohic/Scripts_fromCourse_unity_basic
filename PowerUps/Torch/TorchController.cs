using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchController : MonoBehaviour, ITargetCombat
{

    [SerializeField] GameObject destructionEffect;
    [SerializeField] AudioClip destructionSFX;
    [SerializeField] GameObject coin;
    [SerializeField] GameObject heart;

    [SerializeField] List<GameObject> powerUps;

    public bool indestuctible = false;
    

    public void TakeDamage(int damagePoints){
        Instantiate(destructionEffect, this.transform.position, Quaternion.identity);

        GameObject prefabToInstance = null;
        if(Random.Range(-10, 10) > 0){
            if (Random.Range(-10, 10) > 0){
                prefabToInstance = coin;
            }else{
                if (Random.Range(-10, 10) > 0){
                    prefabToInstance = heart;
                }else{
                    if (Random.Range(-10, 10) > 0){
                        prefabToInstance = powerUps[Random.Range(0, powerUps.Count)];
                    }
                }
            }
        }
        if (prefabToInstance != null){
            Instantiate(prefabToInstance, this.transform.position, Quaternion.identity);
        }

        AudioManager.instance.PlaySfx(destructionSFX);

        if(!indestuctible)
         Destroy(this.gameObject);
    }

}
