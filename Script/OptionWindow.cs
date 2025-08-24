using CoreLib;
using UnityEngine;
using UnityEngine.UI;

public class OptionWindow : MonoBehaviour
{
    public Button optionQuitBtn;
    public Button fps30Btn;
    public Button fps60Btn;
    public Button koreanBtn;
    public Button japaneseBtn;

    public GameObject fps30Selected;
    public GameObject fps60Selected;
    public GameObject koreanSelected;
    public GameObject japaneseSelected;

    public Slider bgmSlider;
    public Slider sfxSlider;

    public void Init()
    {
        optionQuitBtn.AddOnClickListener(OnClickOptionQuitBtn);
        fps30Btn.AddOnClickListener(OnClickFPS30Btn);
        fps60Btn.AddOnClickListener(OnClickFPS60Btn);
        koreanBtn.AddOnClickListener(() => OnClickKoreanBtn());
        japaneseBtn.AddOnClickListener(() => OnClickJapaneseBtn());
        bgmSlider.AddOnValueChangedListener(OnValueChangedBGM);
        sfxSlider.AddOnValueChangedListener(OnValueChangedSFX);
        
        InitOptionSettings();
    }

    private void InitOptionSettings()
    {
        // Language
        var language = ClientSaveData.Language;
        switch (language)
        {
            case "Korean":
                OnClickKoreanBtn(false);
                break;
            case "Japanese":
                OnClickJapaneseBtn(false);
                break;
        }
        
        // BGM
        var bgmVolume = ClientSaveData.BGMVolume;
        AudioManager.Instance.SetBGMVolume(bgmVolume);
        bgmSlider.value = bgmVolume;
        
        // SFX
        var sfxVolume = ClientSaveData.SFXVolume;
        AudioManager.Instance.SetSFXVolume(sfxVolume);
        sfxSlider.value = sfxVolume;
        
        // FrameRate
        var frameRate = ClientSaveData.FrameRate;
        Application.targetFrameRate = frameRate;
        if (frameRate == 30)
        {
            fps30Selected.SetActive(true);
            fps60Selected.SetActive(false);
        }
        else
        {
            fps30Selected.SetActive(false);
            fps60Selected.SetActive(true);
        }
    }

    private void OnClickOptionQuitBtn()
    {
        AudioManager.Instance.PlaySFX("ButtonCancel");
        gameObject.SetActive(false);
    }

    private void OnClickFPS30Btn()
    {
        AudioManager.Instance.PlaySFX("ButtonClick");
        fps30Selected.SetActive(true);
        fps60Selected.SetActive(false);
        ClientSaveData.FrameRate = 30;
        Application.targetFrameRate = 30;
    }

    private void OnClickFPS60Btn()
    {
        AudioManager.Instance.PlaySFX("ButtonClick");
        fps30Selected.SetActive(false);
        fps60Selected.SetActive(true);
        ClientSaveData.FrameRate = 60;
        Application.targetFrameRate = 60;
    }

    private void OnClickKoreanBtn(bool playSound = true)
    {
        if (playSound)
            AudioManager.Instance.PlaySFX("ButtonClick");
        koreanSelected.SetActive(true);
        japaneseSelected.SetActive(false);
        LocalizeManager.Instance.ChangeLanguage(LocalizeManager.Language.Korean);
        ClientSaveData.Language = "Korean";
    }

    private void OnClickJapaneseBtn(bool playSound = true)
    {
        if (playSound)
            AudioManager.Instance.PlaySFX("ButtonClick");
        koreanSelected.SetActive(false);
        japaneseSelected.SetActive(true);
        LocalizeManager.Instance.ChangeLanguage(LocalizeManager.Language.Japanese);
        ClientSaveData.Language = "Japanese";
    }
    
    private void OnValueChangedBGM(float value)
    {
        AudioManager.Instance.SetBGMVolume(value);
        ClientSaveData.BGMVolume = value;
    }

    private void OnValueChangedSFX(float value)
    {
        AudioManager.Instance.SetSFXVolume(value);
        ClientSaveData.SFXVolume = value;
    }
}
