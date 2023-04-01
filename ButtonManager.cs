using SingletonPattern;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    GameManager _gameManager;
    AudioManager _audioManager;
    StoryManager _storyManager;

    private void Awake()
    {
        if(_audioManager == null) _audioManager = AudioManager.Instance;
        if (_gameManager == null) _gameManager = GameManager.Instance;
        if (_storyManager == null) _storyManager = StoryManager.Instance;
    }

    public void OnClickOptionBtn()
    {
        if (_gameManager.GetIsStartedStory()) return; // 게임 시작해서 화면 넘어갈 때 버튼 작동하지 않도록 설정
        _audioManager.PlaySFX("ButtonClick");
        _gameManager._canvas.transform.Find("Option").gameObject.SetActive(true);
    }

    public void OnClickOptionQuitBtn()
    {
        if (_gameManager.GetIsStartedStory()) return; // 게임 시작해서 화면 넘어갈 때 버튼 작동하지 않도록 설정
        _audioManager.PlaySFX("ButtonCancel");
        _gameManager._canvas.transform.Find("Option").gameObject.SetActive(false);
    }

    public void OnClickGameQuitBtn()
    {
        if (GameObject.Find("GameManager").GetComponent<GameManager>().GetIsStartedStory()) return;
        StartCoroutine(CoroutineQuit());
    }

    private IEnumerator CoroutineQuit()
    {
        _audioManager.PlaySFX("ButtonClick");
        yield return new WaitForSeconds(0.5f);
        Application.Quit();
    }

    public void OnClickFPS30Btn()
    {
        _audioManager.PlaySFX("ButtonClick");
        _gameManager._canvas.transform.Find("Option/OptionWindow/FPS/FPS60Btn/Selected").gameObject.SetActive(false);
        _gameManager._canvas.transform.Find("Option/OptionWindow/FPS/FPS30Btn/Selected").gameObject.SetActive(true);
        PlayerPrefs.SetInt("FPS", 30);
        Application.targetFrameRate = 30;
    }

    public void OnClickFPS60Btn()
    {
        _audioManager.PlaySFX("ButtonClick");
        _gameManager._canvas.transform.Find("Option/OptionWindow/FPS/FPS30Btn/Selected").gameObject.SetActive(false);
        _gameManager._canvas.transform.Find("Option/OptionWindow/FPS/FPS60Btn/Selected").gameObject.SetActive(true);
        PlayerPrefs.SetInt("FPS", 60);
        Application.targetFrameRate = 60;
    }

    public void OnClickKoreanBtn()
    {
        _audioManager.PlaySFX("ButtonClick");
        _gameManager._canvas.transform.Find("Option/OptionWindow/Language/KoreanBtn/Selected").gameObject.SetActive(true);
        _gameManager._canvas.transform.Find("Option/OptionWindow/Language/JapaneseBtn/Selected").gameObject.SetActive(false);
        PlayerPrefs.SetString("Language", "Korean");
        Application.Quit();
    }

    public void OnClickJapaneseBtn()
    {
        _audioManager.PlaySFX("ButtonClick");
        _gameManager._canvas.transform.Find("Option/OptionWindow/Language/KoreanBtn/Selected").gameObject.SetActive(false);
        _gameManager._canvas.transform.Find("Option/OptionWindow/Language/JapaneseBtn/Selected").gameObject.SetActive(true);
        PlayerPrefs.SetString("Language", "Japanese");
        Application.Quit();
    }

    public void OnClickEpisodeBackground()
    {
        _storyManager.EpisodeBackgroundClick();
    }

    public void OnClickAutoBtn()
    {
        _storyManager.DoAutoProgress();
        _audioManager.PlaySFX("ButtonClick_BlueArchive");
    }

    public void OnClickMenuBtn()
    {
        if(_gameManager._canvas.transform.Find("Story/Episode/UI/Menu").gameObject.activeSelf)
        {
            _gameManager._canvas.transform.Find("Story/Episode/UI/Menu").gameObject.SetActive(false); // 메뉴창이 열려있으면 메뉴창 닫기
        }
        else
        {
            _gameManager._canvas.transform.Find("Story/Episode/UI/Menu").gameObject.SetActive(true); // 메뉴창이 닫혀있으면 메뉴창 열기
        }
        _audioManager.PlaySFX("ButtonClick_BlueArchive");
    }

    public void OnClickHideUI()
    {
        _storyManager.CheckAutoProgress(); // 스토리가 자동으로 진행 중이라면 멈추도록 함

        _gameManager._canvas.transform.Find("Story/Episode/UI/Menu").gameObject.SetActive(false); // 열린 메뉴창 숨기기
        _gameManager._canvas.transform.Find("Story/Episode/UI").gameObject.SetActive(false); // UI 숨기기
        _gameManager._canvas.transform.Find("Story/Episode/Dialog").gameObject.SetActive(false); // 대화창 숨기기
        _audioManager.PlaySFX("ButtonClick_BlueArchive");
    }

    public void OnClickSelection()
    {
        StartCoroutine(_storyManager.SelectedSelection(0));
        _audioManager.PlaySFX("ButtonClick_BlueArchive");
    }

    public void OnClickSelection1()
    {
        StartCoroutine(_storyManager.SelectedSelection(1));
        _audioManager.PlaySFX("ButtonClick_BlueArchive");
    }

    public void OnClickSelection2()
    {
        StartCoroutine(_storyManager.SelectedSelection(2));
        _audioManager.PlaySFX("ButtonClick_BlueArchive");
    }

    public void OnClickStageSelectQuitBtn()
    {
        if (_gameManager.GetIsStartedStory()) return; // 게임 시작해서 화면 넘어갈 때 버튼 작동하지 않도록 설정
        _audioManager.PlaySFX("ButtonCancel");
        _gameManager._canvas.transform.Find("SelectStage").gameObject.SetActive(false);
    }

    public void OnClickStoryStartBtn(int storyNum)
    {
        if (_gameManager.GetIsStartedStory()) return; // 게임 시작해서 화면 넘어갈 때 버튼 작동하지 않도록 설정
        _gameManager.DoStartStory(storyNum);
    }

    public void OnClickStartBtn()
    {
        if (_gameManager.GetIsStartedStory()) return; // 게임 시작해서 화면 넘어갈 때 버튼 작동하지 않도록 설정
        _audioManager.PlaySFX("ButtonClick");
        _gameManager._canvas.transform.Find("SelectStage").gameObject.SetActive(true);
    }
}