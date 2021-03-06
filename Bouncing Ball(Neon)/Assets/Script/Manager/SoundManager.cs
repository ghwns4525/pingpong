using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager
{
    // AudioSource의 키값(Key, Value)을 받아 저장하는 변수
    Dictionary<int, AudioClip> oAudioClipsMap = new Dictionary<int, AudioClip>();
    AudioSource oAS_Once = null;
    AudioSource oAS_Loop0 = null;
    AudioSource oAS_Loop1 = null;


    private static SoundManager _instance = null;

    public static SoundManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new SoundManager();
            }
            return _instance;
        }
    }

    public void CreateDefaultAudioSource()
    {
        if (oAS_Loop0 != null && oAS_Loop1 != null && oAS_Once != null)
        {
            Debug.Log("Already Created Dafault AudioSources!");
            return;
        }

        // SoundManager라는 게임 오브젝트를 생성
        GameObject oSoundManager = GameObject.Find("SoundManager");
        {
            oSoundManager = new GameObject("SoundManager");
            Debug.Assert(oSoundManager != null, "Can not create new SoundManager GameeObject");
        }
        GameObject.DontDestroyOnLoad(oSoundManager);

        oAS_Once = oSoundManager.AddComponent<AudioSource>();
        oAS_Once.loop = false;

        oAS_Loop0 = oSoundManager.AddComponent<AudioSource>();
        oAS_Loop0.loop = true;

        oAS_Loop1 = oSoundManager.AddComponent<AudioSource>();
        oAS_Loop1.loop = true;
    }

    // 키값을 등록하는 함수
    public void Regist(int iInAudioKey, AudioClip oInAudioClip)
    {
        Debug.Assert(oInAudioClip != null, "Invalid AudioClip! AudioKey= " + iInAudioKey.ToString());

        // oAudioClipsMap에 iInAudioKey가 존재한다면 등록하지 않고 return
        if (oAudioClipsMap.ContainsKey(iInAudioKey) == true)
        {
            Debug.Log("Already Registed AudioClip! AudioKey= " + iInAudioKey.ToString());
            return;
        }

        oAudioClipsMap.Add(iInAudioKey, oInAudioClip);
    }

    // 오디오 클립 번호, 반복여부, BGM여부
    public void PlayerAudioClip(int iInAudioKey, bool IsLoop, bool IsBGM)
    {
        if (oAudioClipsMap.ContainsKey(iInAudioKey) == false)
        {
            Debug.Log("Not exist AudioClip! AudioKey= " + iInAudioKey.ToString());
            return;
        }

        // 반복하여 사용할 AudioSource라면 반복사용
        // 여기서 BGM을 등록하면 아예 BGM 자체를 바꾸는것이고,
        // 아래에서 Pause를 시킨후 Restart하면 잠시 멈추고 다시 트는것
        if (IsLoop && IsBGM)
        {
            Debug.Assert(oAS_Loop0 != null, "AudioSource is null!");
            oAS_Loop0.Stop();
            oAS_Loop0.clip = oAudioClipsMap[iInAudioKey];
            oAS_Loop0.Play();
        }
        else if (IsLoop && !IsBGM)
        {
            Debug.Assert(oAS_Loop0 != null, "AudioSource is null!");
            oAS_Loop1.Stop();
            oAS_Loop1.clip = oAudioClipsMap[iInAudioKey];
            oAS_Loop1.Play();
        }
        // 한번만 사용할 AudioSource라면 한번만 사용
        else if (!IsLoop && !IsBGM)
        {
            Debug.Assert(oAS_Once != null, "AudioSource is null!");
            oAS_Once.PlayOneShot(oAudioClipsMap[iInAudioKey]);
        }
    }

    public void PauseAudioClip()
    {
        oAS_Loop0.Pause();
    }

    public void RestartAudioClip()
    {
        oAS_Loop0.Play();
    }

    // 반복여부, BGM여부
    public void StopAudioClip(bool IsLoop, bool IsBGM)
    {
        if (IsLoop && IsBGM)
        {
            oAS_Loop0.Stop();
        }
        else if (IsLoop && !IsBGM)
        {
            oAS_Loop1.Stop();
        }
        else if (!IsLoop && !IsBGM)
        {
            // 바로 스테이지 실행시 주석
            oAS_Once.Stop();
        }
    }
}
