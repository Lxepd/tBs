using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MusicMgr : InstanceNoMono<MusicMgr>
{
    /// <summary>
    /// Ψһ�ı����������
    /// </summary>
    private AudioSource bkMusic =null;
    /// <summary>
    /// ���ִ�С
    /// </summary>
    private float bkValue = 1;
    /// <summary>
    /// ��Ч��������
    /// </summary>
    private GameObject soundObj = null;
    /// <summary>
    /// ��Ч�б�
    /// </summary>
    private List<AudioSource> soundList = new List<AudioSource>();
    /// <summary>
    /// ��Ч��С
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
                PoolMgr.GetInstance().PushObj("��Ч", soundList[i].gameObject);
                soundList.RemoveAt(i--);
            }
        }
    }

    /// <summary>
    /// ���ű�������
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
    /// �ı䱳������ ������С
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
    /// ��ͣ��������
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
    /// ֹͣ��������
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
    /// ������Ч
    /// </summary>
    /// <param name="name"></param>
    public void PlaySound(string name,bool isLoop, UnityAction<AudioSource> callBack = null)
    {
        if(soundObj ==null)
        {
            soundObj = new GameObject();
            soundObj.name = "Sound";
        }

        // ����Ч��Դ�첽���ؽ�����  �����һ����Ч
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
    /// �ı���Ч������С
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
    /// ֹͣ��Ч
    /// </summary>
    public void StopSound(AudioSource source)
    {
        if(soundList.Contains(source))
        {
            soundList.Remove(source);
            source.Stop();
            PoolMgr.GetInstance().PushObj("��Ч", source.gameObject);
        }
    }
}
