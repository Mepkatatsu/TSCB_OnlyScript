using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}

namespace SingletonPattern
{
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField] Sound[] _sfx = null;
        [SerializeField] Sound[] _bgm = null;

        [SerializeField] AudioSource _bgmPlayer = null;
        [SerializeField] AudioSource[] _sfxPlayer = null;

        private bool _isBGMChanged = false;

        public IEnumerator FadeOutMusic()
        {
            _isBGMChanged = false;

            float volume = GetBGMVolume();

            while (volume > 0)
            {
                if (_isBGMChanged)
                {
                    SetBGMVolume(PlayerPrefs.GetFloat("BGM"));
                    yield break;
                }
                SetBGMVolume(volume - 0.01f);
                volume = GetBGMVolume();
                yield return new WaitForSeconds(0.01f);
            }
            StopBGM();
            SetBGMVolume(PlayerPrefs.GetFloat("BGM"));
        }

        public void PlayBGM(string p_bgmName)
        {
            _isBGMChanged = true;

            for (int i = 0; i < _bgm.Length; i++)
            {
                if (p_bgmName == _bgm[i].name)
                {
                    _bgmPlayer.clip = _bgm[i].clip;
                    _bgmPlayer.Play();
                }
            }
        }

        public void StopBGM()
        {
            _bgmPlayer.Stop();
        }

        public void PlaySFX(string p_sfxName)
        {
            for (int i = 0; i < _sfx.Length; i++)
            {
                if (p_sfxName == _sfx[i].name)
                {
                    for (int j = 0; j < _sfxPlayer.Length; j++)
                    {
                        // SFXPlayer���� ��� ������ ���� Audio Source�� �߰��ߴٸ� 
                        if (!_sfxPlayer[j].isPlaying)
                        {
                            _sfxPlayer[j].clip = _sfx[i].clip;
                            _sfxPlayer[j].Play();
                            return;
                        }
                    }
                    Debug.Log("��� ����� �÷��̾ ������Դϴ�.");
                    return;
                }
            }
            Debug.Log(p_sfxName + " �̸��� ȿ������ �����ϴ�.");
            return;
        }

        public float GetBGMVolume()
        {
            return _bgmPlayer.volume;
        }

        public void SetBGMVolume(float volume)
        {
            _bgmPlayer.volume = volume;
        }

        public void SetSFXVolume(float volume)
        {
            for (int i = 0; i < _sfxPlayer.Length; i++)
            {
                _sfxPlayer[i].volume = volume;
            }
        }
    }
}
