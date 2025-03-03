using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

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
        public float fadeOutSecond = 1;
        
        [SerializeField] private Sound[] sfx;
        [SerializeField] private Sound[] bgm;

        [SerializeField] private AudioSource bgmPlayer;
        [SerializeField] private AudioSource[] sfxPlayer;
        
        private Coroutine _fadeOutMusicCoroutine;
        private WaitForSeconds _fadeOutWaitForSeconds;

        public override void Awake()
        {
            _fadeOutWaitForSeconds = new WaitForSeconds(fadeOutSecond / 100);
            base.Awake();
        }

        public void FadeOutMusic()
        {
            _fadeOutMusicCoroutine = StartCoroutine(FadeOutMusicCoroutine());
        }

        private IEnumerator FadeOutMusicCoroutine()
        {
            var volumeTick = bgmPlayer.volume / 100;

            while (bgmPlayer.volume > 0)
            {
                bgmPlayer.volume -= volumeTick;
                yield return _fadeOutWaitForSeconds;
            }
            StopBGM();
            bgmPlayer.volume = PlayerPrefs.GetFloat("BGM");
        }

        public void PlayBGM(string bgmName)
        {
            if (_fadeOutMusicCoroutine != null)
            {
                StopCoroutine(_fadeOutMusicCoroutine);
                SetBGMVolume(PlayerPrefs.GetFloat("BGM"));
            }

            for (int i = 0; i < bgm.Length; i++)
            {
                if (!bgmName.Equals(bgm[i].name)) 
                    continue;
                
                bgmPlayer.clip = bgm[i].clip;
                bgmPlayer.Play();
            }
        }

        public void StopBGM()
        {
            bgmPlayer.Stop();
        }

        public void PlaySFX(string sfxName)
        {
            for (int i = 0; i < sfx.Length; i++)
            {
                if (!sfxName.Equals(sfx[i].name))
                    continue;
                
                for (int j = 0; j < sfxPlayer.Length; j++)
                {
                    if (sfxPlayer[j].isPlaying)
                        continue;
                    
                    sfxPlayer[j].clip = sfx[i].clip;
                    sfxPlayer[j].Play();
                    return;
                }
                
                Debug.LogWarning("모든 오디오 플레이어가 재생중입니다.");
                return;
            }
            Debug.LogError($"{sfxName} 이름의 효과음이 없습니다.");
        }

        public void SetBGMVolume(float volume)
        {
            bgmPlayer.volume = volume;
        }

        public void SetSFXVolume(float volume)
        {
            for (int i = 0; i < sfxPlayer.Length; i++)
            {
                sfxPlayer[i].volume = volume;
            }
        }
    }
}
