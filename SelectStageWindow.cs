using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectStageWindow : MonoBehaviour
{
    [Serializable]
    public class SelectStage
    {
        public Button storyStartBtn;
        public TMP_Text chapterNameText;
        public Image storyReadImage;
    }

    [Serializable]
    public class StageEnterImage
    {
        public LocalizeManager.Language language;
        public Sprite stageEnterImage;
        public Sprite stageEnterLockedImage;
    }

    [Header("UI")]
    public List<SelectStage> selectStageList = new List<SelectStage>();
    public List<StageEnterImage> stageEnterImageList = new List<StageEnterImage>();
    public Button selectStageQuitBtn;
    public GameObject storyStartText;

    [Header("Select Stage Sprite")]
    [SerializeField] private Sprite storyReadImage;
    [SerializeField] private Sprite storyUnreadImage;

    private void Awake()
    {
        if (selectStageQuitBtn)
            selectStageQuitBtn.onClick.AddListener(OnClickStageSelectQuitBtn);
        
        for (var i = 0; i < selectStageList.Count; i++)
        {
            int stageNum = i + 1;
            selectStageList[i].storyStartBtn.onClick.AddListener(() => OnClickStoryStartBtn(stageNum));
        }
    }

    public void DoStartStory(int storyNum)
    {
        StoryManager.Instance.currentStage = storyNum;

        var stageProgress = ClientSaveData.StageProgress;
        
        if (storyNum == 1)
        {
            StoryManager.Instance.SetStoryNumber(0); // 1화 시작 부분

            // TODO: key로 설정하게 변경
            if(LocalizeManager.language == LocalizeManager.Language.Korean)
            {
                storyStartText.transform.Find("ChapterNumber").GetComponent<TMP_Text>().text = "제1화";
                storyStartText.transform.Find("ChapterName").GetComponent<TMP_Text>().text = "테일즈 사가 크로니클";
            }
            else if (LocalizeManager.language == LocalizeManager.Language.Japanese)
            {
                storyStartText.transform.Find("ChapterNumber").GetComponent<TMP_Text>().text = "第1話";
                storyStartText.transform.Find("ChapterName").GetComponent<TMP_Text>().text = "テイルズ・サガ・クロニクル";
            }
        }
        else if (storyNum == 2)
        {
            if (stageProgress < 1)
            {
                AudioManager.Instance.PlaySFX("ButtonCancel");
                return;
            }

            // 현재 일본어 1장까지만 번역되어있음
            if (LocalizeManager.language == LocalizeManager.Language.Japanese)
            {
                AudioManager.Instance.PlaySFX("ButtonCancel");
                return;
            }

            StoryManager.Instance.SetStoryNumber(45); // 2화 시작 부분

            if (LocalizeManager.language == LocalizeManager.Language.Korean)
            {
                storyStartText.transform.Find("ChapterNumber").GetComponent<TMP_Text>().text = "제2화";
                storyStartText.transform.Find("ChapterName").GetComponent<TMP_Text>().text = "모험의 시작";
            }
            else if (LocalizeManager.language == LocalizeManager.Language.Japanese)
            {
                storyStartText.transform.Find("ChapterNumber").GetComponent<TMP_Text>().text = "第2話";
                storyStartText.transform.Find("ChapterName").GetComponent<TMP_Text>().text = "冒険のはじまり";
            }
        }
        else if (storyNum == 3)
        {
            if (stageProgress < 2)
            {
                AudioManager.Instance.PlaySFX("ButtonCancel");
                return;
            }

            // 현재 3화 미구현
            AudioManager.Instance.PlaySFX("ButtonCancel");
            return;

            /*
            StoryManager.Instance.SetStoryNumber(0); // 3화 시작 부분
            storyStartText.transform.Find("ChapterNumber").GetComponent<TMP_Text>().text = "제3화";
            storyStartText.transform.Find("ChapterName").GetComponent<TMP_Text>().text = "제3화";
            */
        }
        else if (storyNum == 4)
        {
            if (stageProgress < 3)
            {
                AudioManager.Instance.PlaySFX("ButtonCancel");
                return;
            }

            // 현재 4화 미구현
            AudioManager.Instance.PlaySFX("ButtonCancel");
            return;

            /*
            StoryManager.Instance.SetStoryNumber(0); // 4화 시작 부분
            storyStartText.transform.Find("ChapterNumber").GetComponent<TMP_Text>().text = "제4화";
            storyStartText.transform.Find("ChapterName").GetComponent<TMP_Text>().text = "제4화";
            */
        }
        
        AudioManager.Instance.PlaySFX("ButtonStart");
        StartCoroutine(StoryManager.Instance.InitializeStory());
    }

    public void SetSelectStage()
    {
        var stageProgress = ClientSaveData.StageProgress;

        for (var i = 0; i < selectStageList.Count; i++)
        {
            if (i > stageProgress)
            {
                selectStageList[i].chapterNameText.text = "???";
                selectStageList[i].storyReadImage.sprite = storyUnreadImage;
                selectStageList[i].storyStartBtn.image.sprite = GetStageEnterImage(LocalizeManager.language, true);
                continue;
            }
            if (i < stageProgress)
            {
                selectStageList[i].storyReadImage.sprite = storyReadImage;
            }
            
            selectStageList[i].chapterNameText.GetComponent<TextLocalizer>().SetLocalizeText();
            selectStageList[i].storyStartBtn.image.sprite = GetStageEnterImage(LocalizeManager.language, false);
        }
    }

    private Sprite GetStageEnterImage(LocalizeManager.Language language, bool isLocked)
    {
        for (var i = 0; i < stageEnterImageList.Count; i++)
        {
            if (stageEnterImageList[i].language != language)
                continue;

            if (isLocked)
                return stageEnterImageList[i].stageEnterLockedImage;
            else
                return stageEnterImageList[i].stageEnterImage;
        }

        return null;
    }
    
    private void OnClickStoryStartBtn(int stageNum)
    {
        var stageProgress = ClientSaveData.StageProgress;

        if (stageNum > 1 && stageProgress + 1 < stageNum)
        {
            AudioManager.Instance.PlaySFX("ButtonCancel");
            return;
        }
        
        DoStartStory(stageNum);
    }
    
    private void OnClickStageSelectQuitBtn()
    {
        AudioManager.Instance.PlaySFX("ButtonCancel");
        gameObject.SetActive(false);
    }
}