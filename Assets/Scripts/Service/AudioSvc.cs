/****************************************************
    文件：AudioSvc.cs
	作者：HUANGZIYANG
    邮箱: kousiyo741@gmail.com
    日期：2019/10/31 22:51:14
	功能：音效服务
*****************************************************/

using UnityEngine;

public class AudioSvc : MonoBehaviour 
{
    public static AudioSvc Instance = null;
    public AudioSource bgAudio;
    public AudioSource uiAudio;

    public void InitSvc()
    {
        Instance = this;
        PECommon.Log("Init AudioSvc...");
    }

    public void PlayBGMusic(string name,bool isLoop)
    {
        AudioClip audio = ResSvc.Instance.LoadAudio("ResAudio/"+name,true);
        if(bgAudio.clip == null || bgAudio.clip.name != audio.name)
        {
            bgAudio.clip = audio;
            bgAudio.loop = true;
            bgAudio.Play();
        }
    }

    public void PlayUIAuido(string name, bool isLoop)
    {
        AudioClip audio = ResSvc.Instance.LoadAudio("ResAudio/" + name, true);
        uiAudio.clip = audio;
        uiAudio.Play();
    }
}