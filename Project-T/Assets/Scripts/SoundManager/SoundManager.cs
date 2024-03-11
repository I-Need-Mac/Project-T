using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : SingleTon<SoundManager>
{
    public string curentBGMPath;

    AudioSource[] _audioSources = new AudioSource[(int)Define.Sound.MaxCount];
    Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();

    AudioMixer mixer;

    public void Init()
    {
        mixer = ResourcesManager.Load<AudioMixer>(Define.SOUND_PATH + "Mixer");

        GameObject root = GameObject.Find("@Sound");
        if (root == null)
        {
            root = new GameObject { name = "@Sound" };
            Object.DontDestroyOnLoad(root);

            string[] soundNames = System.Enum.GetNames(typeof(Define.Sound)); // "Bgm", "Effect"
            for (int i = 0; i < soundNames.Length - 1; i++)
            {
                GameObject go = new GameObject { name = soundNames[i] };
                _audioSources[i] = go.AddComponent<AudioSource>();
                go.transform.parent = root.transform;
            }

            _audioSources[(int)Define.Sound.Bgm].loop = true; // bgm 재생기는 무한 반복 재생

            _audioSources[(int)Define.Sound.Bgm].outputAudioMixerGroup = mixer.FindMatchingGroups("BGM")[0];
            _audioSources[(int)Define.Sound.SFX].outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
            _audioSources[(int)Define.Sound.Voice].outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
        }
    }

    public void Clear()
    {
        // 재생기 전부 재생 스탑, 음반 빼기
        foreach (AudioSource audioSource in _audioSources)
        {
            audioSource.clip = null;
            audioSource.Stop();
        }
        // 효과음 Dictionary 비우기
        _audioClips.Clear();
    }

    public void SetVolume(string type, int volume)
    {
        if (volume != 0)
        {
            mixer.SetFloat(type, -50 + (volume * 5));
        }
        else
        {
            mixer.SetFloat(type, -80);
        }
    }
    public void SetCurentBGM(string path)
    {
        curentBGMPath = path;
    }
    public string GetCurentBGM()
    {
        return curentBGMPath;
    }

    public void Play(AudioClip audioClip, Define.Sound type = Define.Sound.SFX, float pitch = 1.0f)
    {
        if (audioClip == null)
            return;

        if (type == Define.Sound.Bgm) // BGM 배경음악 재생
        {
            AudioSource audioSource = _audioSources[(int)Define.Sound.Bgm];
            if (audioSource.isPlaying)
                audioSource.Stop();

            audioSource.pitch = pitch;
            audioSource.clip = audioClip;
            audioSource.Play();
        }
        else if(type == Define.Sound.Voice)
        {
            AudioSource audioSource = _audioSources[(int)Define.Sound.Voice];
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(audioClip);
        }
        else // Effect 효과음 재생
        {
            AudioSource audioSource = _audioSources[(int)Define.Sound.SFX];
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(audioClip);
        }
    }

    public void Play(string path, Define.Sound type = Define.Sound.SFX, float pitch = 1.0f)
    {
        AudioClip audioClip = GetOrAddAudioClip(path, type);
        Play(audioClip, type, pitch);
    }

    AudioClip GetOrAddAudioClip(string path, Define.Sound type = Define.Sound.SFX)
    {
        AudioClip audioClip = null;

        if (type == Define.Sound.Bgm) // BGM 배경음악 클립 붙이기
        {
            audioClip = ResourcesManager.Load<AudioClip>(path);
        }
        else // Effect 효과음 클립 붙이기
        {
            if (_audioClips.TryGetValue(path, out audioClip) == false)
            {
                audioClip = ResourcesManager.Load<AudioClip>(path);
                _audioClips.Add(path, audioClip);
            }
        }

        if (audioClip == null)
            Debug.Log($"AudioClip Missing ! {path}");

        return audioClip;
    }
}
