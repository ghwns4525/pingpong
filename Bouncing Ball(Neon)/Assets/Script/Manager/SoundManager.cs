using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager
{
    // AudioSource�� Ű��(Key, Value)�� �޾� �����ϴ� ����
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

        // SoundManager��� ���� ������Ʈ�� ����
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

    // Ű���� ����ϴ� �Լ�
    public void Regist(int iInAudioKey, AudioClip oInAudioClip)
    {
        Debug.Assert(oInAudioClip != null, "Invalid AudioClip! AudioKey= " + iInAudioKey.ToString());

        // oAudioClipsMap�� iInAudioKey�� �����Ѵٸ� ������� �ʰ� return
        if (oAudioClipsMap.ContainsKey(iInAudioKey) == true)
        {
            Debug.Log("Already Registed AudioClip! AudioKey= " + iInAudioKey.ToString());
            return;
        }

        oAudioClipsMap.Add(iInAudioKey, oInAudioClip);
    }

    // ����� Ŭ�� ��ȣ, �ݺ�����, BGM����
    public void PlayerAudioClip(int iInAudioKey, bool IsLoop, bool IsBGM)
    {
        if (oAudioClipsMap.ContainsKey(iInAudioKey) == false)
        {
            Debug.Log("Not exist AudioClip! AudioKey= " + iInAudioKey.ToString());
            return;
        }

        // �ݺ��Ͽ� ����� AudioSource��� �ݺ����
        // ���⼭ BGM�� ����ϸ� �ƿ� BGM ��ü�� �ٲٴ°��̰�,
        // �Ʒ����� Pause�� ��Ų�� Restart�ϸ� ��� ���߰� �ٽ� Ʈ�°�
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
        // �ѹ��� ����� AudioSource��� �ѹ��� ���
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

    // �ݺ�����, BGM����
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
            // �ٷ� �������� ����� �ּ�
            oAS_Once.Stop();
        }
    }
}
