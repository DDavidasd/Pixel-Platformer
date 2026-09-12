using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Build;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioSource[] sfx;
    [SerializeField] private AudioSource[] bgm;

    private int bgmIndexToPlay;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (instance == null) //nem akarjuk, hogy minden egyes szinten egy újabb PlayerManager objektumot létrehozzon.
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

    }

    private void Update()
    {
        if (!bgm[bgmIndexToPlay].isPlaying)
        {
            bgm[bgmIndexToPlay].Play(); 
        } 
    } 

    public void PlaySFX(int sfxToPlay)
    {
        if (sfxToPlay < sfx.Length)
        {
            sfx[sfxToPlay].pitch = Random.Range(0.8f, 1.2f);
            sfx[sfxToPlay].Play();
        }
    }

    public void PlayBGM(int bgmToPlay)
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            bgm[i].Stop();  
        }

        bgm[bgmToPlay].Play();
    }

    public void PlayRandomBGM() 
    { 
        bgmIndexToPlay = Random.Range(0, bgm.Length); 
         
        PlayBGM(bgmIndexToPlay); 
    } 
}
