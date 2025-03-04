using DG.Tweening;
using System.Collections;
using CoreLib;
using TMPro;
using UnityEngine;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

public class GameManager : Singleton<GameManager>
{
    #region Variables

    private enum StartFrom
    {
        Intro,
        Story,
        Game1
    }

    [Header("Start Settings")]
    [SerializeField] private StartFrom startFrom;

    [SerializeField] private int storyNumber;

    [Header("UI")]
    public GameObject canvas;
    public OptionWindow optionWindow;

    public GameObject startBtn;
    public GameObject optionBtn;
    public GameObject gameQuitBtn;
    public Button tempBtn;
    public TMP_Text guideText;
    public Image aronaImage;

    public SelectStageWindow selectStageWindow;

    #endregion Variables

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            tempBtn.Select(); // 마우스를 클릭한 뒤 다른 지점으로 이동해서 뗐을 때 다른 버튼을 선택하여 애니메이션 정상화
        }
    }

    #region Initialize Game

    public override void Awake()
    {
        optionWindow.Init();
        
        startBtn.AddOnClickListener(OnClickStartBtn);
        optionBtn.AddOnClickListener(OnClickOptionBtn);
        gameQuitBtn.AddOnClickListener(OnClickGameQuitBtn);
    }

    private void Start()
    {
        StartCoroutine(DoLoading());
    }

    // 스토리부터 시작할 때는 잠깐 로딩을 갖도록 함
    private IEnumerator DoLoading()
    {
        switch (startFrom)
        {
            case StartFrom.Intro:
                StartCoroutine(DoOpening());
                break;
            case StartFrom.Story:
                yield return new WaitForSeconds(1);
                StoryManager.Instance.storyWindow.SetActive(true);
                StoryManager.Instance.SetStoryNumber(storyNumber);
                StoryManager.Instance.SetIsAvailableProgress(true);
                StoryManager.Instance.DoStoryProgress();
                break;
            case StartFrom.Game1:
                ShootingGameManager.Instance.StartShootingGame();
                break;
        }
    }

    // 게임이 처음부터 시작할 때 오프닝을 보여줌
    private IEnumerator DoOpening()
    {
        // 시작할 때 필요없는 것들 꺼주기
        startBtn.SetActiveSafe(false);
        optionBtn.SetActiveSafe(false);
        gameQuitBtn.SetActiveSafe(false);
        optionWindow.SetActiveSafe(false);
        StoryManager.Instance.storyWindow.SetActiveSafe(false);
        
        aronaImage.gameObject.SetActiveSafe(true);
        aronaImage.color = new Color(1, 1, 1, 0);
        guideText.color = new Color(1, 1, 1, 0);

        DOTween.Sequence().Append(aronaImage.DOFade(1.0f, 3).SetEase(Ease.Linear)).AppendInterval(2)
            .Append(aronaImage.DOFade(0f, 2).SetEase(Ease.Linear));

        DOTween.Sequence().Append(guideText.DOFade(1.0f, 3).SetEase(Ease.Linear)).AppendInterval(2)
            .Append(guideText.DOFade(0f, 2).SetEase(Ease.Linear));

        yield return new WaitForSeconds(7);
        aronaImage.gameObject.SetActiveSafe(false);
        AudioManager.Instance.PlayBGM("Restart");

        yield return DoOpeningAnimation();
    }

    private IEnumerator DoOpeningAnimation()
    {
        canvas.transform.Find("Main/MainBackground").GetComponent<Image>().DOFade(1, 2);
        yield return new WaitForSeconds(1);
        canvas.transform.Find("Main/MainCharacter").GetComponent<Image>().DOFade(1, 2);
        yield return new WaitForSeconds(1);
        canvas.transform.Find("Main/Title1").GetComponent<Image>().DOFade(1, 2);
        yield return new WaitForSeconds(1);
        AudioManager.Instance.PlaySFX("8bitBomb");
        canvas.transform.Find("Main/Title2").GetComponent<Image>().DOFade(1, 2);
        canvas.transform.Find("Main/Title2").GetComponent<Animator>().Play("Blast");
        yield return new WaitForSeconds(2);
        startBtn.SetActive(true);
        canvas.transform.Find("Main/StartBtn/Text").GetComponent<TMP_Text>().DOFade(1, 1);
        yield return new WaitForSeconds(0.5f);
        optionBtn.SetActive(true);
        canvas.transform.Find("Main/OptionBtn/Text").GetComponent<TMP_Text>().DOFade(1, 1);
        yield return new WaitForSeconds(0.5f);
        gameQuitBtn.SetActive(true);
        canvas.transform.Find("Main/GameQuitBtn/Text").GetComponent<TMP_Text>().DOFade(1, 1);
    }

    #endregion Initialize Game

    #region For StoryManager.cs

    // 스토리가 끝났을 때 작동
    public void DoFinishStory(int storyNum)
    {
        StoryManager.Instance.storyWindow.SetActive(false);
        StoryManager.Instance.storyWidget.SetActive(false);
        AudioManager.Instance.PlayBGM("Restart");
        ClientSaveData.StageProgress = storyNum;
        selectStageWindow.SetSelectStage();
    }

    #endregion For StoryManager.cs

    #region For ShootingGameManager.cs

    // 슈팅 게임을 시작함
    public void DoStartShootingGame()
    {
        ShootingGameManager.Instance.StartShootingGame();
    }

    // 슈팅 게임을 종료함
    public void DoFinishShootingGame()
    {
        ShootingGameManager.Instance.StopShootingGame();
    }

    #endregion For ShootingGameManager.cs

    public static IEnumerator FadeInImage(float timeSpeed, Image image)
    {
        while (image.color.a < 1.0f)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b,
                image.color.a + (Time.deltaTime * timeSpeed));
            yield return null;
        }
    }

    public static IEnumerator FadeOutImage(float timeSpeed, Image image)
    {
        while (image.color.a > 0.0f)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b,
                image.color.a - (Time.deltaTime * timeSpeed));
            yield return null;
        }
    }

    private void OnClickStartBtn()
    {
        AudioManager.Instance.PlaySFX("ButtonClick");
        selectStageWindow.SetSelectStage();
        selectStageWindow.gameObject.SetActive(true);
    }

    private void OnClickOptionBtn()
    {
        AudioManager.Instance.PlaySFX("ButtonClick");
        optionWindow.gameObject.SetActive(true);
    }

    private void OnClickGameQuitBtn()
    {
        StartCoroutine(CoroutineQuit());
    }

    private IEnumerator CoroutineQuit()
    {
        AudioManager.Instance.PlaySFX("ButtonClick");
        yield return new WaitForSeconds(0.5f);
        Application.Quit();
    }
}