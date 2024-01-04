using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] Sound[] bgm, sfx;
    [SerializeField] AudioSource bgmSource, sfxSource;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }

        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlayBGM(string name)
    {
        Sound sound = Array.Find(bgm, x => x.name == name);

        if (sound != null)
            return;

        else
        {
            if (bgmSource.isPlaying)
            {
                bgmSource.Stop();
            }

            bgmSource.clip = sound.clip;
            bgmSource.loop = true;
            bgmSource.Play();
        }
    }

    public void StopBGM() => bgmSource.Stop();

    public void PlaySFX(string name)
    {
        Sound sound = Array.Find(sfx, x => x.name == name);

        if (sound == null)
        {
            Debug.Log("���� ����");
            return;
        }


        else
        {
            sfxSource.PlayOneShot(sound.clip);
        }
    }

    #region ����� ȯ�漳��
    public void ToggleBGM() => bgmSource.mute = !bgmSource.mute;
    public void ToggleSFX() => sfxSource.mute = !sfxSource.mute;
    public void BGMVolume(float volume) => bgmSource.volume = volume;
    public void SFXVolume(float volume) => sfxSource.volume = volume;
    #endregion

}
