using SingletonPattern;
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
    
    private AudioManager _audioManager;
    private StoryManager _storyManager;

    public void Init()
    {
        if(_audioManager == null) 
            _audioManager = AudioManager.Instance;
        if (_storyManager == null) 
            _storyManager = StoryManager.Instance;

        if (optionQuitBtn)
            optionQuitBtn.onClick.AddListener(OnClickOptionQuitBtn);
        if (fps30Btn)
            fps30Btn.onClick.AddListener(OnClickFPS30Btn);
        if (fps60Btn)
            fps60Btn.onClick.AddListener(OnClickFPS60Btn);
        if (koreanBtn)
            koreanBtn.onClick.AddListener(() => OnClickKoreanBtn());
        if (japaneseBtn)
            japaneseBtn.onClick.AddListener(() => OnClickJapaneseBtn());
        if (bgmSlider)
            bgmSlider.onValueChanged.AddListener(OnValueChangedBGM);
        if (sfxSlider)
            sfxSlider.onValueChanged.AddListener(OnValueChangedSFX);
        
        InitOptionSettings();
    }

    private void InitOptionSettings()
    {
        // 초기 PlayerPrefs 설정
        if (!PlayerPrefs.HasKey("Language"))
            PlayerPrefs.SetString("Language", "Korean");
        if (!PlayerPrefs.HasKey("BGM"))
            PlayerPrefs.SetFloat("BGM", 1);
        if (!PlayerPrefs.HasKey("SFX"))
            PlayerPrefs.SetFloat("SFX", 1);
        if (!PlayerPrefs.HasKey("FPS"))
            PlayerPrefs.SetInt("FPS", 60);

        if (PlayerPrefs.HasKey("Language"))
        {
            var language = PlayerPrefs.GetString("Language");
            switch (language)
            {
                case "Korean":
                    OnClickKoreanBtn(false);
                    break;
                case "Japanese":
                    OnClickJapaneseBtn(false);
                    break;
            }
        }
        if (PlayerPrefs.HasKey("BGM"))
        {
            var bgm = PlayerPrefs.GetFloat("BGM");
            _audioManager.SetBGMVolume(bgm);
            bgmSlider.value = bgm;
        }
        if (PlayerPrefs.HasKey("SFX"))
        {
            var sfx = PlayerPrefs.GetFloat("SFX");
            _audioManager.SetSFXVolume(sfx);
            sfxSlider.value = sfx;
        }
        if (PlayerPrefs.HasKey("FPS"))
        {
            var fps = PlayerPrefs.GetInt("FPS");
            Application.targetFrameRate = fps;
            if (fps == 30)
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
    }

    private void OnClickOptionQuitBtn()
    {
        _audioManager.PlaySFX("ButtonCancel");
        gameObject.SetActive(false);
    }

    private void OnClickFPS30Btn()
    {
        _audioManager.PlaySFX("ButtonClick");
        fps30Selected.SetActive(true);
        fps60Selected.SetActive(false);
        PlayerPrefs.SetInt("FPS", 30);
        Application.targetFrameRate = 30;
    }

    private void OnClickFPS60Btn()
    {
        _audioManager.PlaySFX("ButtonClick");
        fps30Selected.SetActive(false);
        fps60Selected.SetActive(true);
        PlayerPrefs.SetInt("FPS", 60);
        Application.targetFrameRate = 60;
    }

    private void OnClickKoreanBtn(bool playSound = true)
    {
        if (playSound)
            _audioManager.PlaySFX("ButtonClick");
        koreanSelected.SetActive(true);
        japaneseSelected.SetActive(false);
        LocalizeManager.Instance.ChangeLanguage(LocalizeManager.Language.Korean);
        PlayerPrefs.SetString("Language", "Korean");
    }

    private void OnClickJapaneseBtn(bool playSound = true)
    {
        if (playSound)
            _audioManager.PlaySFX("ButtonClick");
        koreanSelected.SetActive(false);
        japaneseSelected.SetActive(true);
        LocalizeManager.Instance.ChangeLanguage(LocalizeManager.Language.Japanese);
        PlayerPrefs.SetString("Language", "Japanese");
    }
    
    private void OnValueChangedBGM(float value)
    {
        _audioManager.SetBGMVolume(value);
        PlayerPrefs.SetFloat("BGM", value);
    }

    private void OnValueChangedSFX(float value)
    {
        _audioManager.SetSFXVolume(value);
        PlayerPrefs.SetFloat("SFX", value);
    }
}
