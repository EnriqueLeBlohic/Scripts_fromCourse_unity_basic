using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] AudioClip levelMusic;
    void Start()
    {
        AudioManager.instance.PlayMusic(levelMusic);
    }

    
    void Update()
    {
        
    }
}
