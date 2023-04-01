using SingletonPattern;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    AudioManager _audioManager;

    private void Awake()
    {
        if(_audioManager == null) _audioManager = AudioManager.Instance;
    }

    public void OnValueChangedBGM()
    {
        if (_audioManager == null) _audioManager = AudioManager.Instance;

        _audioManager.SetBGMVolume(gameObject.GetComponent<Slider>().value);
        PlayerPrefs.SetFloat("BGM", gameObject.GetComponent<Slider>().value);
    }

    public void OnValueChangedSFX()
    {
        if (_audioManager == null) _audioManager = AudioManager.Instance;

        _audioManager.SetSFXVolume(gameObject.GetComponent<Slider>().value);
        PlayerPrefs.SetFloat("SFX", gameObject.GetComponent<Slider>().value);
    }
}
