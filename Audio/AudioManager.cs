using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private float _musicVolume;
    [Range(0,1)]
    [SerializeField] float musicVolume;
    private float _sfxVolume;
    [Range(0,1)]
    [SerializeField] float sfxVolume;

    private static AudioSource musicAudioSource;
    private static AudioSource sfxAudioSource;

    private static AudioManager _instance;
    public static AudioManager instance{
        get{
            if(_instance == null){

                _instance = FindObjectOfType<AudioManager>();

                GameObject gameO;
                if (_instance == null){ 

                  gameO = new GameObject("AudioManager");
                 gameO.AddComponent<AudioManager>();
                 _instance=gameO.GetComponent<AudioManager>();
                 
                }

                if (_instance != null){
                 var gameMusic= new GameObject("Music");
                 gameMusic.AddComponent<AudioSource>();
                 musicAudioSource = gameMusic.GetComponent<AudioSource>();
                 gameMusic.transform.parent= _instance.gameObject.transform;
                      var gameSfx= new GameObject("Sfx");

                 gameSfx.AddComponent<AudioSource>();
                 gameSfx.transform.parent= _instance.gameObject.transform;
                 sfxAudioSource = gameSfx.GetComponent<AudioSource>();
                 DontDestroyOnLoad(_instance.gameObject);
                }

            }
            return _instance;
        }
    }
    public void PlaySfx(AudioClip audioClip){
        sfxAudioSource.PlayOneShot(audioClip);

    }
    public void PlayMusic(AudioClip audioClip){
        if (musicAudioSource.clip!= audioClip){ 

         musicAudioSource.clip = audioClip;
         musicAudioSource.loop = true;
         musicAudioSource.Play();

        }

    }

    private void Update(){
        if(musicVolume != _musicVolume){
            _musicVolume = musicVolume;
            musicAudioSource.volume = musicVolume;
        }

        if(sfxVolume != _sfxVolume){
            _sfxVolume = sfxVolume;
            sfxAudioSource.volume = sfxVolume;
        }
    }



}
