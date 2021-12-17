using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MusicMgr : InstanceNoMono<MusicMgr>
{
    /// <summary>
    /// 唯一的背景音乐组件
    /// </summary>
    private AudioSource bkMusic =null;
    /// <summary>
    /// 音乐大小
    /// </summary>
    private float bkValue = 1;
    /// <summary>
    /// 音效依附对象
    /// </summary>
    private GameObject soundObj = null;
    /// <summary>
    /// 音效列表
    /// </summary>
    private List<AudioSource> soundList = new List<AudioSource>();
    /// <summary>
    /// 音效大小
    /// </summary>
    private float soundValue = 1;

    public MusicMgr()
    {
        MonoMgr.GetInstance().AddUpdateListener(Update);
    }

    private void Update()
    {
        for (int i = soundList.Count-1; i >=0; i--)
        {
            if(soundList[i].isPlaying)
            {
                PoolMgr.GetInstance().PushObj("音效", soundList[i].gameObject);
                soundList.RemoveAt(i--);
            }
        }
    }

    /// <summary>
    /// 播放背景音乐
    /// </summary>
    /// <param name="name"></param>
    public void PlayBkMusic(string name)
    {
        if (bkMusic == null)
        {
            GameObject obj = new GameObject();
            obj.name = "BkMusic";
            bkMusic = obj.AddComponent<AudioSource>();
        }

        ResMgr.GetInstance().LoadAsync<AudioClip>("Music/Bk" + name, (clip) =>
        {
            bkMusic.clip = clip;
            bkMusic.loop = true;
            bkMusic.volume = bkValue;
            bkMusic.Play();
        });
    }
    /// <summary>
    /// 改变背景音乐 音量大小
    /// </summary>
    /// <param name="v"></param>
    public void ChangeBkValue(float v)
    {
        bkValue = v;
        if(bkMusic==null)
        {
            return;
        }

        bkMusic.volume = bkValue;
    }
    /// <summary>
    /// 暂停背景音乐
    /// </summary>
    public void PauseBkMusic()
    {
        if(bkMusic ==null)
        {
            return;
        }

        bkMusic.Pause();
    }
    /// <summary>
    /// 停止背景音乐
    /// </summary>
    public void StopBkMusic()
    {
        if(bkMusic==null)
        {
            return;
        }

        bkMusic.Stop();
    }
    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="name"></param>
    public void PlaySound(string name,bool isLoop, UnityAction<AudioSource> callBack = null)
    {
        if(soundObj ==null)
        {
            soundObj = new GameObject();
            soundObj.name = "Sound";
        }

        // 当音效资源异步加载结束后  再添加一个音效
        ResMgr.GetInstance().LoadAsync<AudioClip>("Music/Sound" + name, (clip) =>
          {
              AudioSource source = soundObj.AddComponent<AudioSource>();
              source.clip = clip;
              source.loop = isLoop;
              source.volume = bkValue;
              source.Play();
              soundList.Add(source);
              if (callBack != null)
              {
                  callBack(source);
              }
          });
    }
    /// <summary>
    /// 改变音效声音大小
    /// </summary>
    /// <param name="value"></param>
    public void ChangeSoundValue(float value)
    {
        soundValue = value;

        foreach (var s in soundList)
        {
            s.volume = value;
        }
    }
    /// <summary>
    /// 停止音效
    /// </summary>
    public void StopSound(AudioSource source)
    {
        if(soundList.Contains(source))
        {
            soundList.Remove(source);
            source.Stop();
            PoolMgr.GetInstance().PushObj("音效", source.gameObject);
        }
    }
}
