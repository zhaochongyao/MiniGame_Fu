using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    [Header("背景音乐")]
    public AudioClip[] backGroundClips;
    private AudioSource backGroundSource;
    [Range(0,1)]
    public float backGround_Volume = 0.5f;  //背景音乐声音大小 
    [Header("人物音效")]
    public AudioClip[] footStepClips;       //脚步声
    private AudioSource playerSource;
    [Range(0, 1)]
    public float player_Volume = 0.5f;      //人物音效声音大小

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
        SourceInit();
        VolumeInit();
    }
    private void SourceInit()
    {
        backGroundSource = gameObject.AddComponent<AudioSource>();
        playerSource = gameObject.AddComponent<AudioSource>();
    }
    private void VolumeInit()
    {
        backGroundSource.volume = backGround_Volume;
        playerSource.volume = player_Volume;
    }
    public static AudioManager getInstance()
    {
        return instance;
    }
    public void playBackGround(int index)
    {
        backGroundSource.clip = backGroundClips[index];
        backGroundSource.Play();
    }
    public void stopPlayBackGround(int index)
    {
        backGroundSource.Stop();
    }
}
