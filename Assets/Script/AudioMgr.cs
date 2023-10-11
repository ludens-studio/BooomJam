using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI; 

public class AudioMgr : BaseMgr<AudioMgr>
{
    /// <summary>
    /// 背景音乐组件
    /// </summary>
    public AudioSource bkMusic = null;
    public Slider bkSlider; 
    /// <summary>
    /// 音效组件
    /// </summary>
    public AudioSource soundMusic = null;
    public Slider soundSlider;


    //音效列表
    public List<AudioClip> soundList = new List<AudioClip>();
    
    //音乐大小
    private float bkValue = 1;
    //音效大小
    private float soundValue = 1;

    private void Update()
    {
        float bk_value = bkSlider.value;
        float sound_value = soundSlider.value;
        ChangeBKValue(bk_value);
        ChangeSoundValue(sound_value);
    }



    /// <summary>
    /// 神父换碟
    /// </summary>
    /// <param name="fileName"></param>
    public void ChangeBKMusic(string fileName)
    {
        bkMusic.clip = ResMgr.GetInstance().Load<AudioClip>(fileName);
    }

    /// <summary>
    /// 播放背景音乐
    /// </summary>
    /// <param name="name"></param>
    public void PlayBkMusic()
    {
        bkMusic.Play();
    }

    /// <summary>
    /// 暂停背景音乐
    /// </summary>
    public void PauseBKMusic()
    {
        if (bkMusic == null)
            return;
        bkMusic.Pause();
    }

    /// <summary>
    /// 停止背景音乐
    /// </summary>
    public void StopBKMusic()
    {
        if (bkMusic == null)
            return;
        bkMusic.Stop();
    }

    /// <summary>
    /// 改变背景音乐 音量大小
    /// </summary>
    /// <param name="v"></param>
    public void ChangeBKValue(float v)
    {
        bkValue = v;
        if (bkMusic == null)
            return;
        bkMusic.volume = bkValue;
    }
    
    /// <summary>
    /// 改变音效 音量大小
    /// </summary>
    /// <param name="v"></param>
    public void ChangeSoundValue(float v)
    {
        soundValue = v;
        if (soundMusic == null)
            return;
        soundMusic.volume = soundValue;
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    public void PlaySound(int i)
    {
        soundMusic.clip = soundList[i];
        soundMusic.Play();
    }

    /// <summary>
    /// 停止音效
    /// </summary>
    public void StopSound()
    {
        if (soundMusic == null)
            return;
        soundMusic.Stop();
    }
}