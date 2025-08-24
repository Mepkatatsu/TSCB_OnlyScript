using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using CoreLib;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoryManager : Singleton<StoryManager>
{
    #region Variables

    public GameObject storyWindow;
    public GameObject storyWidget;
    public Image windowFadeOut;
    public Image storyStartWindowFadeOut;
    public Image storyBackground;
    public Image storyMovingBackground;
    public Button storyProgressBtn;
    public Button autoBtn;
    public Button menuBtn;
    public Button hideUIBtn;
    public Button skipBtn;
    public GameObject singleSelectionBtn;
    public GameObject selection1Btn;
    public GameObject selection2Btn;

    public GameObject yuzu;
    public GameObject aris;
    public GameObject midori;
    public GameObject momoi;

    [Serializable]
    public class MomoiImage
    {
        public Sprite[] imageArray;
    }

    [Serializable]
    public class MidoriImage
    {
        public Sprite[] imageArray;
    }

    [Serializable]
    public class ArisImage
    {
        public Sprite[] imageArray;
    }

    [Serializable]
    public class YuzuImage
    {
        public Sprite[] imageArray;
    }

    [Serializable]
    public class ButtonImage
    {
        public Sprite[] buttonArray;
    }

    [Serializable]
    public class StoryBackgroundImage
    {
        public Sprite[] backgroundArray;
    }

    // 이미지가 9개 있는 캐릭터의 경우, 9번에 더미 이미지를 집어넣어 10~19까지 사용할 수 있도록 설정
    [SerializeField] private MomoiImage momoiImage;
    [SerializeField] private MidoriImage midoriImage;
    [SerializeField] private ArisImage arisImage;
    [SerializeField] private YuzuImage yuzuImage;

    // ButtonImage 0: 흰색 배경, 1: 노란색 배경
    [SerializeField] private ButtonImage buttonImage;

    // 0: Background, 1: GameDevelopment, 2: TSCBackground, 3: RingOfRight, 4: Ruins, 5: GameCenter
    [SerializeField] private StoryBackgroundImage storyBackgroundImage;

    public TMP_Text characterName;
    public TMP_Text departmentName;
    public TMP_Text dialogText;
    public TMP_Text windowText;
    public TMP_Text gameOverText;
    public TMP_Text gameOver2Text;

    public GameObject dialogEnd;

    public Image momoiCharacterImage;
    public Image midoriCharacterImage;
    public Image yuzuCharacterImage;
    public Image arisCharacterImage;
    public Image momoiHaloImage;
    public Image midoriHaloImage;
    public Image yuzuHaloImage;
    public Image arisHaloImage;

    [NonSerialized] public int currentStage;

    private List<string> _characterNameList; // 캐릭터 이름 목록
    private List<string> _departmentNameList; // 소속 이름 목록
    private List<string> _dialogList; // 대화 내용 목록
    private List<float> _textSpeedList; // 대화 내용 목록

    private int _storyNum; // 스토리가 어디까지 진행됐는지 저장하는 번호

    private Coroutine _coroutineStoryProgress; // 대사를 한글자씩 출력하는 코루틴, 화면을 터치하면 중지시키고 한 번에 표시하기 위해 사용
    private Coroutine _autoStoryProgress; // 자동으로 스토리를 진행시켜주는 코루틴
    private Coroutine _coroutineWindowFadeOut; // 화면이 검게 Fade Out되는 코루틴, 화면을 다시 Fade In 할 때 충돌이 생기지 않도록 저장하여 중단시킴

    private bool _isAvailableProgress; // 현재 스토리를 진행할 수 있는지 여부
    private bool _isProgressingStory; // 스토리에서 현재 대사가 나오고 있는지 확인
    private bool _isAutoStoryProgress; // 현재 자동으로 스토리가 진행 중인지 여부를 판단
    private bool _isDialogOff; // 대화창이 꺼져있는 상태인지 확인
    private bool _isSelectionOn; // 선택지가 켜졌는지 확인
    private bool _isPressedButton; // 버튼이 눌렀는지 확인

    private float _elapsedTime;

    #endregion Variables

    public void Awake()
    {
        StartCoroutine(StartTimer());

        // 왜인진 모르겠지만 선언할 떄 new로 해줘도 null임
        _characterNameList = new List<string>();
        _departmentNameList = new List<string>();
        _dialogList = new List<string>();
        _textSpeedList = new List<float>();

        if (storyProgressBtn)
            storyProgressBtn.onClick.AddListener(OnClickStoryProgressBtn);
        if (autoBtn)
            autoBtn.onClick.AddListener(OnClickAutoBtn);
        if (menuBtn)
            menuBtn.onClick.AddListener(OnClickMenuBtn);
        if (hideUIBtn)
            hideUIBtn.onClick.AddListener(OnClickHideUI);
        if (singleSelectionBtn)
            singleSelectionBtn.GetComponent<Button>().onClick.AddListener(OnClickSingleSelection);
        if (selection1Btn)
            selection1Btn.GetComponent<Button>().onClick.AddListener(OnClickSelection1);
        if (selection2Btn)
            selection2Btn.GetComponent<Button>().onClick.AddListener(OnClickSelection2);
        if (skipBtn)
            skipBtn.onClick.AddListener(OnClickSkipBtn);
    }

    // 스토리에 처음 진입했을 때 작동하는 함수
    public IEnumerator InitializeStory()
    {
        storyWindow.SetActive(true);
        storyStartWindowFadeOut.color = new Color(1, 1, 1, 0);
        storyWindow.transform.Find("StoryStart").gameObject.SetActive(true);
        storyWidget.gameObject.SetActive(false);
        AudioManager.Instance.FadeOutMusic();
        yield return new WaitForSeconds(6.5f);
        storyStartWindowFadeOut.DOFade(1, 2);

        // 여기부터 스토리 초기 세팅

        dialogText.GetComponent<TMP_Text>().fontSize = 42.5f;
        SetCharacterImage(momoi, 0);
        SetCharacterImage(midori, 0);
        SetCharacterImage(aris, 0);
        SetCharacterImage(yuzu, 0);
        storyBackground.color = new Color(1, 1, 1, 0);
        momoi.SetActive(false);
        midori.SetActive(false);
        aris.SetActive(false);
        yuzu.SetActive(false);

        momoiCharacterImage.color = new Color(1, 1, 1, 0);
        momoiHaloImage.color = new Color(1, 1, 1, 0);
        midoriCharacterImage.color = new Color(1, 1, 1, 0);
        midoriHaloImage.color = new Color(1, 1, 1, 0);
        arisCharacterImage.color = new Color(1, 1, 1, 0);
        arisHaloImage.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        yuzuCharacterImage.color = new Color(1, 1, 1, 0);
        yuzuHaloImage.color = new Color(1, 1, 1, 0);

        momoi.SetActive(false);
        midori.SetActive(false);
        aris.SetActive(false);
        yuzu.SetActive(false);

        // 여기까지 스토리 초기 세팅

        yield return new WaitForSeconds(2.5f);
        storyWidget.SetActive(true);
        storyWidget.transform.Find("UI").gameObject.SetActive(true);
        storyWidget.transform.Find("Dialog").gameObject.SetActive(true);
        storyProgressBtn.gameObject.SetActive(true);
        _isAvailableProgress = true;
        DoStoryProgress();
    }

    private IEnumerator StartTimer()
    {
        while (true)
        {
            if (_isAvailableProgress && !_isProgressingStory) _elapsedTime += 0.5f;
            yield return new WaitForSeconds(0.5f);
        }
    }

    #region Method for UI(Dialog, Button, Selection)

    // 대화창을 끄고 키는 함수
    private void SetDialogOn(bool isDialogOff)
    {
        if (!isDialogOff)
        {
            _isAvailableProgress = false;
            _isDialogOff = true;
            dialogText.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            _isAvailableProgress = true;
            _isDialogOff = false;
            dialogText.transform.parent.gameObject.SetActive(true);
        }
    }

    // 선택지로 갈린 스토리 번호 이동 체크하고 이동하는 함수
    private void DoStoryJump()
    {
        if (_storyNum == 23)
        {
            _storyNum = 28;
        }
    }

    // 선택지를 눌렀을 때 작동하는 함수
    public IEnumerator SelectedSelection(int num)
    {
        if (_isPressedButton) yield break;
        _isPressedButton = true;
        yield return new WaitForSeconds(0.5f); // 버튼이 눌린 후 진행되도록 잠시 대기
        _isSelectionOn = false;
        _storyNum++;
        if (num == 0)
        {
            singleSelectionBtn.SetActive(false);
        }
        else if (num == 1)
        {
            selection1Btn.SetActive(false);
            selection2Btn.SetActive(false);

            // 선택지를 고르면 알맞은 번호로 이동
            if (_storyNum == 14) // A키를 입력한다.
            {
                _storyNum = 23;
            }
            else if (_storyNum == 30) // A키 입력
            {
                _storyNum = 30;
            }
        }
        else if (num == 2)
        {
            selection1Btn.SetActive(false);
            selection2Btn.SetActive(false);

            // 선택지를 고르면 알맞은 번호로 이동
            if (_storyNum == 14) // B키를 입력한다.
            {
                _storyNum = 14;
            }
            else if (_storyNum == 30) // 푸니젤리에게 조심스럽게 접근한다.
            {
                _storyNum = 36;
            }
        }

        _isAvailableProgress = true;
        DoStoryProgress();
    }

    // 1개짜리 선택지를 보여주는 함수
    private void ShowSelection(string selectionDialog)
    {
        _isPressedButton = false;
        _isSelectionOn = true;
        singleSelectionBtn.SetActive(true);
        singleSelectionBtn.transform.Find("Text").GetComponent<TMP_Text>().text = selectionDialog;
        singleSelectionBtn.GetComponent<Animator>().Play("ButtonPopup");
    }

    // 2개짜리 선택지를 보여주는 함수
    private void ShowSelection(string selectionDialog1, string selectionDialog2)
    {
        _isPressedButton = false;
        _isSelectionOn = true;
        selection1Btn.SetActive(true);
        selection2Btn.SetActive(true);
        selection1Btn.transform.Find("Text").GetComponent<TMP_Text>().text = selectionDialog1;
        selection2Btn.transform.Find("Text").GetComponent<TMP_Text>().text = selectionDialog2;
        selection1Btn.GetComponent<Animator>().Play("ButtonPopup");
        selection2Btn.GetComponent<Animator>().Play("ButtonPopup");
    }

    public bool CheckAutoProgress()
    {
        if (_isAutoStoryProgress)
        {
            StopCoroutine(_autoStoryProgress);
            _isAutoStoryProgress = false;
            storyWidget.transform.Find("UI/AutoBtn").GetComponent<Image>().sprite = buttonImage.buttonArray[0];
            return true;
        }
        else return false;
    }

    // 스토리에서 AUTO 버튼을 눌렀을 때 자동으로 스토리가 진행되도록 해주는 함수
    public void DoAutoProgress()
    {
        if (CheckAutoProgress()) return;

        _autoStoryProgress = StartCoroutine(DoAutoProgressCoroutine());
        _isAutoStoryProgress = true;
        storyWidget.transform.Find("UI/AutoBtn").GetComponent<Image>().sprite = buttonImage.buttonArray[1];
    }

    // 자동으로 스토리가 진행되도록 해주는 코루틴
    private IEnumerator DoAutoProgressCoroutine()
    {
        while (true)
        {
            // 컷씬이 진행중일 때는 대기
            while (!_isAvailableProgress)
            {
                yield return new WaitForSeconds(1);
            }

            // 대화가 출력되고 있지 않을 때만 진행
            if (!_isProgressingStory)
            {
                if (_elapsedTime >= 1f)
                {
                    DoStoryProgress();
                }
                /*
                // 대화 출력이 완료된 후 최소 1초 대기
                _autoStoryNum = _storyNum;
                yield return new WaitForSeconds(1);
                // 스토리 자동 진행 중 유저가 화면을 터치해서 다음 대사로 넘어가면 카운트를 다시 실행
                // 이게 없으면 유저가 2번 터치를 해서 다음 대사로 타이밍이 안좋게 넘어갈 경우, 대사가 곧바로 스킵되는 문제가 발생함
                // 확인해보니 블루 아카이브에서도 동일한 이슈가 있으나.. AUTO 버튼을 누른 채로 터치하는 유저가 많지는 않을테니...
                if (_autoStoryNum == _storyNum)
                {
                    DoStoryProgress();
                }
                */
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    #endregion Method for UI(Dialog, Button, Selection)

    // 스토리 진행시 텍스트 외에 캐릭터 움직임, 효과음 등을 관리하는 파트
    private IEnumerator DoStoryAction(int num)
    {
        if (!_isProgressingStory)
        {
            _isAvailableProgress = false;
            if (num == 0)
            {
                AudioManager.Instance.PlaySFX("Silence");
            }
            else if (num == 1)
            {
                //audioManager.PlaySFX("Silence");
            }
            else if (num == 2)
            {
                dialogText.GetComponent<TMP_Text>().fontSize = 60;
                AudioManager.Instance.PlaySFX("Start");
            }
            else if (num == 3)
            {
                momoi.SetActive(true);
                momoi.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);

                storyBackground.sprite = storyBackgroundImage.backgroundArray[1];
                storyBackground.DOFade(1, 2);
                momoiCharacterImage.DOFade(1, 2);
                momoiHaloImage.DOFade(1, 2);

                AudioManager.Instance.PlayBGM("PixelTime");
                yield return new WaitForSeconds(1.5f);
                dialogText.GetComponent<TMP_Text>().fontSize = 42.5f;
                PlayCharacterEffectAnimation(momoi, "Talking");
                momoi.GetComponent<Animator>().Play("Jumping");
            }
            else if (num == 4)
            {
                midori.SetActive(true);
                midori.transform.parent.GetComponent<RectTransform>().anchoredPosition =
                    new Vector2(600, 0); // midori right side : 625
                midoriCharacterImage.color = Color.white;
                midoriHaloImage.color = Color.white;

                momoi.transform.parent.GetComponent<RectTransform>().DOLocalMoveX(-700, 0.5f); // momoi left side : -700
            }
            else if (num == 5)
            {
                midori.gameObject.SetActive(false);
                momoi.gameObject.SetActive(false);
                aris.gameObject.SetActive(true);
                yuzu.gameObject.SetActive(true);

                SetCharacterImage(yuzu, 8);
                SetCharacterImage(aris, 0);
                arisCharacterImage.color = Color.white;
                arisHaloImage.color = Color.white;
                aris.transform.parent.GetComponent<RectTransform>().anchoredPosition =
                    new Vector2(600, 0); // aris right side : 580
                yuzuCharacterImage.color = Color.white;
                yuzuHaloImage.color = Color.white;
                yuzu.transform.parent.GetComponent<RectTransform>().anchoredPosition =
                    new Vector2(-700, 0); // left side : -700
                yuzu.GetComponent<Animator>().Play("Shiver");
                PlayCharacterEffectAnimation(yuzu, "Mess");
            }
            else if (num == 6)
            {
                SetCharacterImage(aris, 7);
                PlayCharacterEffectAnimation(aris, "Shine");
            }
            else if (num == 7)
            {
                SetCharacterImage(momoi, 3);
                momoi.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
                momoi.gameObject.SetActive(true);
                aris.gameObject.SetActive(false);
                yuzu.gameObject.SetActive(false);

                momoi.GetComponent<Animator>().Play("Jumping");
                PlayCharacterEffectAnimation(momoi, "Talking");
            }
            else if (num == 8) // 선택지
            {
                if (LocalizeManager.language == LocalizeManager.Language.Korean) ShowSelection("( 자리에 앉는다. )");
                else if (LocalizeManager.language == LocalizeManager.Language.Japanese) ShowSelection("（席に座る）");
            }
            else if (num == 9) // Window Fade Out
            {
                StartCoroutine(DoFadeStoryWindowFadeOut());
                AudioManager.Instance.PlaySFX("TSCStart");

                AudioManager.Instance.FadeOutMusic();
                yield return new WaitForSeconds(2f);
                SetDialogOn(false);
                momoi.transform.gameObject.SetActive(false);
                storyBackground.sprite = storyBackgroundImage.backgroundArray[2];

                yield return new WaitForSeconds(2f);

                SetDialogOn(true);
            }
            else if (num == 10)
            {
                SetDialogOn(false);
                yield return new WaitForSeconds(0.5f);
                AudioManager.Instance.PlayBGM("Theme101");

                if (LocalizeManager.language == LocalizeManager.Language.Korean)
                    StartCoroutine(AppendTextOneByOne(windowText, "< 코스모스 세기 2354년, 인류는 업화의 불길에 휩싸였다. >\r\n\r\n", 1));
                else if (LocalizeManager.language == LocalizeManager.Language.Japanese)
                    StartCoroutine(AppendTextOneByOne(windowText, "コスモス世紀2354年、人類は劫火の炎につつまれた……\r\n\r\n", 1));

                yield return new WaitForSeconds(2.5f);
                if (LocalizeManager.language == LocalizeManager.Language.Korean) ShowSelection("“……동화적 색채?”");
                else if (LocalizeManager.language == LocalizeManager.Language.Japanese)
                    ShowSelection("“……童話テイストで、色彩豊か？”");
            }
            else if (num == 11)
            {
                SetDialogOn(true);
            }
            else if (num == 12)
            {
                if (LocalizeManager.language == LocalizeManager.Language.Korean)
                    ShowSelection("“……。”", "( 버튼을 입력한다. )");
                else if (LocalizeManager.language == LocalizeManager.Language.Japanese)
                    ShowSelection("“…….”", "（ボタンを押す）");
            }
            else if (num == 13)
            {
                SetDialogOn(false);
                if (LocalizeManager.language == LocalizeManager.Language.Korean)
                    StartCoroutine(AppendTextOneByOne(windowText, "< 튜토리얼을 시작합니다. >\r\n\r\n", 1));
                else if (LocalizeManager.language == LocalizeManager.Language.Japanese)
                    StartCoroutine(AppendTextOneByOne(windowText, "チュートリアルを開始します。\r\n\r\n", 1));

                yield return new WaitForSeconds(2);

                if (LocalizeManager.language == LocalizeManager.Language.Korean)
                    StartCoroutine(AppendTextOneByOne(windowText, "< 먼저 B 키를 눌러, 눈앞의 무기를 장착해보세요. >\n\n", 1));
                else if (LocalizeManager.language == LocalizeManager.Language.Japanese)
                    StartCoroutine(AppendTextOneByOne(windowText, "まずはBボタンを押して、目の前の武器を装着してみてください。\n\n", 1));

                yield return new WaitForSeconds(2.5f);

                if (ClientSaveData.Language.Equals("Korean"))
                    ShowSelection("( A 키를 입력한다. )", "( B 키를 입력한다. )");
                else if (ClientSaveData.Language.Equals("Japanese")) ShowSelection("（Aボタンを押す）", "（Bボタンを押す）");
            }
            // B 버튼 루트 시작
            else if (num == 14)
            {
                AudioManager.Instance.StopBGM();
                AudioManager.Instance.PlaySFX("SelectButton");
                yield return new WaitForSeconds(1);
                AudioManager.Instance.PlaySFX("8bitBomb");
                SetDialogOn(true);
            }
            else if (num == 15)
            {
                if (ClientSaveData.Language.Equals("Korean")) ShowSelection("“???”");
                else if (ClientSaveData.Language.Equals("Japanese")) ShowSelection("“？？？”");
            }
            else if (num == 16)
            {
                SetDialogOn(false);
                // 게임 오버
                windowText.text = "";
                AudioManager.Instance.PlaySFX("GameOver");
                gameOverText.DOFade(1, 2f);
                yield return new WaitForSeconds(4);
                if (ClientSaveData.Language.Equals("Korean")) ShowSelection("“?!”");
                else if (ClientSaveData.Language.Equals("Japanese")) ShowSelection("“？！”");
            }
            else if (num == 17)
            {
                SetDialogOn(true);
                AudioManager.Instance.PlaySFX("Laughing");
            }
            else if (num == 18) // WindowFadeOut
            {
                SetCharacterImage(momoi, 3);

                StartCoroutine(DoFadeStoryWindowFadeOut());
                yield return new WaitForSeconds(2f);
                momoi.transform.gameObject.SetActive(true);
                windowText.text = "";

                //GameOver 글씨 투명하게 변경
                ColorUtility.TryParseHtmlString("#FF656B00", out Color color);
                gameOverText.color = color;

                storyBackground.sprite = storyBackgroundImage.backgroundArray[1];
                yield return new WaitForSeconds(2f);


                momoi.GetComponent<Animator>().Play("Jumping");
                PlayCharacterEffectAnimation(momoi, "Laughing");
                AudioManager.Instance.PlayBGM("MischievousStep");
            }
            else if (num == 19)
            {
                if (ClientSaveData.Language.Equals("Korean")) ShowSelection("“…….”");
                else if (ClientSaveData.Language.Equals("Japanese")) ShowSelection("“……。”");
            }
            else if (num == 20)
            {
                aris.SetActive(true);
                SetCharacterImage(aris, 9);

                momoi.transform.parent.GetComponent<RectTransform>().DOLocalMoveX(-700, 0.5f); // momoi left side : -700
                PlayCharacterEffectAnimation(aris, "Sweat");
            }
            else if (num == 21)
            {
                if (ClientSaveData.Language.Equals("Korean")) ShowSelection("( 다시 시도한다. )");
                else if (ClientSaveData.Language.Equals("Japanese")) ShowSelection("（もう一回やる）");
            }
            else if (num == 22)
            {
                StartCoroutine(DoFadeStoryWindowFadeOut());
                AudioManager.Instance.FadeOutMusic();
                AudioManager.Instance.PlaySFX("TSCStart");
                yield return new WaitForSeconds(2f);
                SetDialogOn(false);
                momoi.transform.gameObject.SetActive(false);
                aris.transform.gameObject.SetActive(false);
                storyBackground.sprite = storyBackgroundImage.backgroundArray[2];
                yield return new WaitForSeconds(2f);
                if (ClientSaveData.Language.Equals("Korean"))
                    StartCoroutine(AppendTextOneByOne(windowText, "< 무기 장착에 성공했습니다. >\n\n", 1));
                else if (ClientSaveData.Language.Equals("Japanese"))
                    StartCoroutine(AppendTextOneByOne(windowText, "武器を装着しました。\n\n", 1));
                AudioManager.Instance.PlaySFX("Message");
                yield return new WaitForSeconds(2f);
                SetDialogOn(true);
            }
            // B 버튼 루트 종료

            // A버튼 루트
            else if (num == 23)
            {
                windowText.text = "";
                AudioManager.Instance.StopBGM();
                if (ClientSaveData.Language.Equals("Korean"))
                    StartCoroutine(AppendTextOneByOne(windowText, "< 무기 장착에 성공했습니다. >\n\n", 1));
                else if (ClientSaveData.Language.Equals("Japanese"))
                    StartCoroutine(AppendTextOneByOne(windowText, "武器を装着しました。\n\n", 1));
                AudioManager.Instance.PlaySFX("Message");
                yield return new WaitForSeconds(2f);
                SetDialogOn(true);
                AudioManager.Instance.PlaySFX("Talking");
            }
            else if (num == 24)
            {
                AudioManager.Instance.PlaySFX("Sweat");
            }
            else if (num == 25)
            {
                AudioManager.Instance.PlaySFX("Shine");
            }
            else if (num == 26)
            {
            }
            else if (num == 27)
            {
            }
            // A버튼 루트 종료
            else if (num == 28)
            {
                SetDialogOn(false);
                AudioManager.Instance.PlayBGM("MechanicalJungle");
                windowText.text += "<#FF656B>";
                if (ClientSaveData.Language.Equals("Korean"))
                    StartCoroutine(AppendTextOneByOne(windowText, "< 전투 발생! 전투 발생! >\n\n", 1));
                else if (ClientSaveData.Language.Equals("Japanese"))
                    StartCoroutine(AppendTextOneByOne(windowText, "エンカウントが発生しました！\n\n", 1));

                yield return new WaitForSeconds(2);

                windowText.text += "</color>";
                if (ClientSaveData.Language.Equals("Korean"))
                    StartCoroutine(AppendTextOneByOne(windowText, "< 야생의 푸니젤리가 나타났다! >\n\n", 1));
                else if (ClientSaveData.Language.Equals("Japanese"))
                    StartCoroutine(AppendTextOneByOne(windowText, "野生のプニプニが現れた！\n\n", 1));
                yield return new WaitForSeconds(2);
                SetDialogOn(true);
            }
            else if (num == 29)
            {
                if (ClientSaveData.Language.Equals("Korean"))
                    ShowSelection("( A버튼 입력, < 비검 츠바메가에시, 한 번에 적을 2회 공격한다. > )", "( 푸니젤리에게 조심스럽게 접근한다. )");
                else if (ClientSaveData.Language.Equals("Japanese"))
                    ShowSelection("（Aボタン「秘剣つばめが返し：敵に対して２回攻撃をする」）", "（プニプニに用心深く接近する）");
            }
            // A버튼을 누른다 루트 시작
            else if (num == 30)
            {
                SetDialogOn(false);
                AudioManager.Instance.StopBGM();
                if (ClientSaveData.Language.Equals("Korean"))
                    StartCoroutine(AppendTextOneByOne(windowText, "< 탕!! >\n\n", 1));
                else if (ClientSaveData.Language.Equals("Japanese"))
                    StartCoroutine(AppendTextOneByOne(windowText, "ッダーン！\n\n", 1));
                AudioManager.Instance.PlaySFX("8bitBang");

                yield return new WaitForSeconds(2);

                if (ClientSaveData.Language.Equals("Korean"))
                    StartCoroutine(AppendTextOneByOne(windowText, "< 명중, 당신의 캐릭터가 즉사했다. >\n\n", 1));
                else if (ClientSaveData.Language.Equals("Japanese"))
                    StartCoroutine(AppendTextOneByOne(windowText, "攻撃が命中、即死しました\n\n", 1));
                AudioManager.Instance.PlaySFX("KnockDown");

                yield return new WaitForSeconds(2);

                // 게임 오버
                windowText.text = "";
                AudioManager.Instance.PlaySFX("GameOver");
                gameOverText.DOFade(1, 2);
                yield return new WaitForSeconds(4);

                if (ClientSaveData.Language.Equals("Korean")) ShowSelection("“??!!”");
                else if (ClientSaveData.Language.Equals("Japanese")) ShowSelection("“？？！！”");
            }
            else if (num == 31)
            {
                gameOver2Text.DOFade(1, 2);
                AudioManager.Instance.PlayBGM("FadeOut");
                AudioManager.Instance.PlaySFX("RobotTalk");
                yield return new WaitForSeconds(4);

                SetCharacterImage(momoi, 7);

                momoi.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(-700, 0);
                StartCoroutine(DoFadeStoryWindowFadeOut());
                yield return new WaitForSeconds(2);
                momoi.transform.gameObject.SetActive(true);
                windowText.text = "";

                //GameOver 글씨 투명하게 변경
                ColorUtility.TryParseHtmlString("#FF656B00", out var color);
                gameOverText.color = color;
                ColorUtility.TryParseHtmlString("#5DC48100", out color);
                gameOver2Text.color = color;
                storyBackground.sprite = storyBackgroundImage.backgroundArray[1];
                yield return new WaitForSeconds(2);

                PlayCharacterEffectAnimation(momoi, "Silence");
                SetDialogOn(true);
            }
            else if (num == 32)
            {
                midori.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(600, 0);
                midori.transform.gameObject.SetActive(true);
                SetCharacterImage(midori, 7);

                PlayCharacterEffectAnimation(midori, "Sweat");
            }
            else if (num == 33)
            {
                momoi.SetActive(false);
                midori.SetActive(false);
                aris.SetActive(true);
                SetCharacterImage(aris, 10);

                aris.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
                aris.GetComponent<Animator>().Play("Jumping");
                PlayCharacterEffectAnimation(aris, "Mess");
            }
            else if (num == 34)
            {
                if (ClientSaveData.Language.Equals("Korean")) ShowSelection("( 다시 시도한다. )");
                else if (ClientSaveData.Language.Equals("Japanese")) ShowSelection("（もう一回やる）");
            }
            else if (num == 35) // Window Fade Out(게임 부분으로)
            {
                StartCoroutine(DoFadeStoryWindowFadeOut());
                AudioManager.Instance.FadeOutMusic();
                AudioManager.Instance.PlaySFX("TSCStart");
                yield return new WaitForSeconds(2);
                SetDialogOn(false);
                momoi.transform.gameObject.SetActive(false);
                aris.transform.gameObject.SetActive(false);
                storyBackground.sprite = storyBackgroundImage.backgroundArray[2];

                yield return new WaitForSeconds(2);

                if (ClientSaveData.Language.Equals("Korean"))
                    StartCoroutine(AppendTextOneByOne(windowText, "< 무기 장착에 성공했습니다. >\n\n", 1));
                else if (ClientSaveData.Language.Equals("Japanese"))
                    StartCoroutine(AppendTextOneByOne(windowText, "武器を装着しました。\n\n", 1));
                AudioManager.Instance.PlaySFX("Message");

                yield return new WaitForSeconds(2f);

                AudioManager.Instance.PlayBGM("MechanicalJungle");
                windowText.text += "<#FF656B>";
                if (ClientSaveData.Language.Equals("Korean"))
                    StartCoroutine(AppendTextOneByOne(windowText, "< 전투 발생! 전투 발생! >\n\n", 1));
                else if (ClientSaveData.Language.Equals("Japanese"))
                    StartCoroutine(AppendTextOneByOne(windowText, "エンカウントが発生しました！\n\n", 1));

                yield return new WaitForSeconds(2);

                windowText.text += "</color>";
                if (ClientSaveData.Language.Equals("Korean"))
                    StartCoroutine(AppendTextOneByOne(windowText, "< 야생의 푸니젤리가 나타났다! >\n\n", 1));
                else if (ClientSaveData.Language.Equals("Japanese"))
                    StartCoroutine(AppendTextOneByOne(windowText, "野生のプニプニが現れた！\n\n", 1));

                yield return new WaitForSeconds(2);

                if (ClientSaveData.Language.Equals("Korean")) ShowSelection("( 푸니젤리에게 조심스럽게 접근한다. )");
                else if (ClientSaveData.Language.Equals("Japanese")) ShowSelection("（プニプニに用心深く接近する）");
            }
            // 푸니젤리에게 조심스럽게 접근한다. (공통 루트)
            else if (num == 36)
            {
                SetDialogOn(false);
                yield return new WaitForSeconds(2);

                if (ClientSaveData.Language.Equals("Korean"))
                    StartCoroutine(AppendTextOneByOne(windowText, "< 오류 발생! 오류 발생! >\n\n", 1));
                else if (ClientSaveData.Language.Equals("Japanese"))
                    StartCoroutine(AppendTextOneByOne(windowText, "エラー！エラー！\n\n", 1));

                AudioManager.Instance.StopBGM();
                AudioManager.Instance.PlaySFX("RobotTalk");
                yield return new WaitForSeconds(2);

                if (ClientSaveData.Language.Equals("Korean"))
                    StartCoroutine(AppendTextOneByOne(windowText, "< 심각한 오류 발생으로 시스템을 긴급 정지합니다. >\n\n", 1.5f));
                else if (ClientSaveData.Language.Equals("Japanese"))
                    StartCoroutine(AppendTextOneByOne(windowText, "深刻なエラーが発生してシステムを緊急停止します。\n\n", 1));

                AudioManager.Instance.PlaySFX("RobotTalk");
                yield return new WaitForSeconds(2);

                if (ClientSaveData.Language.Equals("Korean")) ShowSelection("“!?”");
                else if (ClientSaveData.Language.Equals("Japanese")) ShowSelection("“！？”");
            }
            else if (num == 37)
            {
                SetDialogOn(true);
                AudioManager.Instance.PlaySFX("Explode");
            }
            else if (num == 38)
            {
                SetCharacterImage(momoi, 4);
                momoi.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(-700, 0);

                StartCoroutine(DoFadeStoryWindowFadeOut());
                yield return new WaitForSeconds(2);
                SetDialogOn(false);
                momoi.SetActive(true);
                windowText.text = "";

                storyBackground.sprite = storyBackgroundImage.backgroundArray[1];
                yield return new WaitForSeconds(2);
                // 흙먼지 연출?

                SetDialogOn(true);
                momoi.GetComponent<Animator>().Play("Shiver");
                momoi.transform.Find("MessParent/Mess").GetComponent<Animator>().Play("Mess");
                AudioManager.Instance.PlaySFX("Mess");

                AudioManager.Instance.PlayBGM("MischievousStep");
            }
            else if (num == 39)
            {
                SetCharacterImage(midori, 7);
                midori.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(600, 0);

                midori.SetActive(true);
                midori.GetComponent<Animator>().Play("Jumping");
                PlayCharacterEffectAnimation(midori, "Talking");
            }
            else if (num == 40)
            {
                SetCharacterImage(yuzu, 8);
                SetCharacterImage(aris, 4);
                momoi.SetActive(false);
                midori.SetActive(false);
                aris.SetActive(true);
                yuzu.SetActive(true);
                yuzu.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(-700, 0);
                aris.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(600, 0);

                yuzu.GetComponent<Animator>().Play("Shiver");
                PlayCharacterEffectAnimation(yuzu, "Mess");
            }
            else if (num == 41)
            {
                SetCharacterImage(aris, 4);
                aris.GetComponent<Animator>().Play("Shiver");
                PlayCharacterEffectAnimation(aris, "Mess");
            }
            else if (num == 42)
            {
                if (ClientSaveData.Language.Equals("Korean")) ShowSelection("( 정신이 흐려진다…… )");
                else if (ClientSaveData.Language.Equals("Japanese")) ShowSelection("“視界がぼやける……”");
            }
            else if (num == 43)
            {
                SetDialogOn(false);
                windowFadeOut.gameObject.SetActive(true);
                // 화면 깜빡이는 연출, DOTween을 사용하면 중간중간 끊기는 느낌이 들어서 따로 구현한 함수 사용

                _coroutineWindowFadeOut = StartCoroutine(GameManager.FadeInImage(0.5f, windowFadeOut));
                yield return new WaitForSeconds(0.5f);

                StopCoroutine(_coroutineWindowFadeOut);
                _coroutineWindowFadeOut = StartCoroutine(GameManager.FadeOutImage(0.3f, windowFadeOut));
                yield return new WaitForSeconds(0.5f);

                StopCoroutine(_coroutineWindowFadeOut);
                _coroutineWindowFadeOut = StartCoroutine(GameManager.FadeOutImage(0.01f, windowFadeOut));
                yield return new WaitForSeconds(0.2f);

                StopCoroutine(_coroutineWindowFadeOut);
                _coroutineWindowFadeOut = StartCoroutine(GameManager.FadeInImage(0.5f, windowFadeOut));
                yield return new WaitForSeconds(0.5f);

                StopCoroutine(_coroutineWindowFadeOut);
                _coroutineWindowFadeOut = StartCoroutine(GameManager.FadeOutImage(0.3f, windowFadeOut));
                yield return new WaitForSeconds(0.5f);

                StopCoroutine(_coroutineWindowFadeOut);
                _coroutineWindowFadeOut = StartCoroutine(GameManager.FadeOutImage(0.01f, windowFadeOut));
                yield return new WaitForSeconds(0.2f);

                StopCoroutine(_coroutineWindowFadeOut);
                _coroutineWindowFadeOut = StartCoroutine(GameManager.FadeInImage(0.5f, windowFadeOut));
                yield return new WaitForSeconds(2);


                // 대화창을 가리는 WindowFadeOut 감추고, 배경을 검은색으로 변경
                storyBackground.sprite = storyBackgroundImage.backgroundArray[0];
                storyBackground.color = Color.black;
                StartCoroutine(GameManager.FadeOutImage(4f, windowFadeOut));
                yuzu.SetActive(false);
                aris.SetActive(false);
                AudioManager.Instance.PlaySFX("KnockDown"); // 쓰러지는 소리
                AudioManager.Instance.StopBGM();
                yield return new WaitForSeconds(1);

                windowFadeOut.gameObject.SetActive(false);
                SetDialogOn(true);
                dialogText.GetComponent<TMP_Text>().fontSize = 60;
                AudioManager.Instance.PlaySFX("Confuse");
            }
            else if (num == 44) // 메인화면으로 이동
            {
                CheckAutoProgress();

                GameManager.Instance.DoFinishStory(1);

                yield break;
            }
            // 1장 종료

            // 2장 시작
            else if (num == 45)
            {
                SetDialogOn(false);

                AudioManager.Instance.PlayBGM("MoroseDreamer");

                storyMovingBackground.sprite = storyBackgroundImage.backgroundArray[3];
                storyMovingBackground.DOFade(1, 2);
                yield return new WaitForSeconds(2);
                storyMovingBackground.transform.DOLocalMoveY(-100, 3).SetEase(Ease.Linear);
                yield return new WaitForSeconds(4);

                _isAvailableProgress = true;
                _storyNum++;
                DoStoryProgress();
                yield break;
            }
            else if (num == 46)
            {
                SetDialogOn(true);

                midori.SetActive(true);
                SetCharacterImage(midori, 10);
                midoriCharacterImage.color = Color.black;
                midoriCharacterImage.DOFade(1, 2);
                midori.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(-500, 0);
            }
            else if (num == 48)
            {
                ShowSelection("( ……. )");
            }
            else if (num == 52)
            {
                ShowSelection("( ……. )");
            }
            else if (num == 53)
            {
                storyMovingBackground.DOFade(0, 2);
                yield return new WaitForSeconds(2);

                midori.SetActive(false);
            }
            else if (num == 54)
            {
                ShowSelection("( 눈을 뜬다. )");
            }
            else if (num == 55)
            {
                SetDialogOn(false);
                storyBackground.sprite = storyBackgroundImage.backgroundArray[4];
                storyBackground.DOFade(1, 3);

                midori.SetActive(true);
                midori.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
                midoriCharacterImage.color = new Color(1, 1, 1, 0);
                midoriCharacterImage.DOFade(1, 3);
                AudioManager.Instance.FadeOutMusic();

                yield return new WaitForSeconds(3);

                SetDialogOn(true);
                SetCharacterImage(midori, 12);
                PlayCharacterEffectAnimation(midori, "Talking");
                midori.GetComponent<Animator>().Play("Jumping");

                AudioManager.Instance.PlayBGM("Morning");
            }
            else if (num == 56)
            {
                ShowSelection("( 미도리? )", "( ……아니, 헤일로가 없는 걸 보니 우리 학생이 아니군. )");
            }
            else if (num == 57)
            {
                SetCharacterImage(midori, 14);
                PlayCharacterEffectAnimation(midori, "Sweat");
            }
            else if (num == 58)
            {
                SetCharacterImage(midori, 18);
            }
            else if (num == 59)
            {
                SetCharacterImage(midori, 15);
            }
            else if (num == 62)
            {
                ShowSelection("“……미증유의 위기 아니었어?”");
            }
            else if (num == 63)
            {
                SetCharacterImage(midori, 14);
                PlayCharacterEffectAnimation(midori, "Mess");
                midori.GetComponent<Animator>().Play("Shiver");
            }
            else if (num == 64)
            {
                ShowSelection("“그럴 수도 있지.”");
            }
            else if (num == 65)
            {
                SetCharacterImage(midori, 18);
                midori.GetComponent<Animator>().Play("Jumping");
            }
            else if (num == 66)
            {
                SetCharacterImage(midori, 15);
            }
            else if (num == 67)
            {
                ShowSelection("“하지만 나는 싸움을 잘 못하는 걸.”");
            }
            else if (num == 68)
            {
                SetCharacterImage(midori, 10);
                PlayCharacterEffectAnimation(midori, "Talking");
                midori.GetComponent<Animator>().Play("Jumping");
            }
            else if (num == 69)
            {
                SetCharacterImage(midori, 12);
                PlayCharacterEffectAnimation(midori, "Question");
            }
            else if (num == 70)
            {
                SetCharacterImage(midori, 13);
                PlayCharacterEffectAnimation(midori, "Shine");
            }
            else if (num == 71)
            {
                SetCharacterImage(midori, 17);
                PlayCharacterEffectAnimation(midori, "Silence");
            }
            else if (num == 72)
            {
                SetCharacterImage(midori, 10);
            }
            else if (num == 73)
            {
                SetCharacterImage(midori, 17);
            }
            else if (num == 74)
            {
                ShowSelection("“내가 도움이 된다면야 얼마든지.”");
            }
            else if (num == 75)
            {
                SetCharacterImage(midori, 13);
                PlayCharacterEffectAnimation(midori, "Talking");
                midori.GetComponent<Animator>().Play("Jumping");
            }
            else if (num == 76)
            {
                _isAvailableProgress = true;
                ShowStoryText();
                _isAvailableProgress = false;

                yield return new WaitForSeconds(1.5f);

                AudioManager.Instance.PlaySFX("Run");
                midori.transform.parent.GetComponent<RectTransform>().DOLocalMoveX(2000, 1f);
                _isAvailableProgress = true;

                yield break;
            }
            else if (num == 77)
            {
                ShowSelection("“자, 잠시만!”");
            }
            else if (num == 78)
            {
                SetDialogOn(false);
                StartCoroutine(DoFadeStoryWindowFadeOut());
                AudioManager.Instance.FadeOutMusic();

                yield return new WaitForSeconds(2);

                AudioManager.Instance.PlayBGM("RoundAndRound");
                storyBackground.sprite = storyBackgroundImage.backgroundArray[5];

                SetCharacterImage(midori, 10);
                midori.transform.parent.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                yield return new WaitForSeconds(2);

                SetDialogOn(true);

                PlayCharacterEffectAnimation(midori, "Talking");
                midori.GetComponent<Animator>().Play("Jumping");
            }
            else if (num == 79)
            {
                ShowSelection("( 신전이라기보다는 오락실처럼 보이는데…… )");
            }
            else if (num == 80)
            {
                SetCharacterImage(midori, 15);
            }
            else if (num == 81)
            {
                SetCharacterImage(midori, 14);
                PlayCharacterEffectAnimation(midori, "Sweat");
            }
            else if (num == 82)
            {
                ShowSelection("( 게임기를 살펴본다. )");
            }
            else if (num == 83)
            {
                StartCoroutine(DoFadeStoryWindowFadeOut());

                yield return new WaitForSeconds(2);

                midori.SetActive(false);
                SetDialogOn(false);
                storyBackground.color = Color.black;
                storyWidget.transform.Find("WindowText_Ranking").gameObject.SetActive(true);

                yield return new WaitForSeconds(2);
                AudioManager.Instance.PlaySFX("Mess");
                SetDialogOn(true);
            }
            else if (num == 85)
            {
                AudioManager.Instance.PlaySFX("Sweat");
            }
            else if (num == 86)
            {
                ShowSelection("“최근 날짜로 올수록 점수가 오히려 낮아졌네?”");
            }
            else if (num == 90)
            {
                ShowSelection("( 미도리의 플레이를 지켜본다. )");
            }
            else if (num == 91)
            {
                StartCoroutine(DoFadeStoryWindowFadeOut());
                AudioManager.Instance.FadeOutMusic();

                yield return new WaitForSeconds(2);

                AudioManager.Instance.PlayBGM("KurameNoMyojo");
                SetDialogOn(false);
                SetCharacterImage(midori, 15);
                midori.SetActive(true);
                //AudioManager.Instance.PlayBGM("RoundAndRound");
                storyBackground.sprite = storyBackgroundImage.backgroundArray[5];
                storyBackground.color = Color.white;
                storyWidget.transform.Find("WindowText_Ranking").gameObject.SetActive(false);

                yield return new WaitForSeconds(2);

                midori.GetComponent<Animator>().Play("Jumping");
                AudioManager.Instance.PlaySFX("Shoot");

                yield return new WaitForSeconds(0.3f);

                AudioManager.Instance.PlaySFX("Shoot");

                yield return new WaitForSeconds(0.3f);

                AudioManager.Instance.PlaySFX("UseSkill");

                yield return new WaitForSeconds(0.3f);

                AudioManager.Instance.PlaySFX("EnemyDestroy");

                yield return new WaitForSeconds(0.3f);

                midori.GetComponent<Animator>().Play("Jumping");
                AudioManager.Instance.PlaySFX("Shoot");

                yield return new WaitForSeconds(0.3f);

                AudioManager.Instance.PlaySFX("Shoot");

                yield return new WaitForSeconds(0.3f);

                AudioManager.Instance.PlaySFX("EnemyDestroy");

                yield return new WaitForSeconds(1f);

                AudioManager.Instance.PlaySFX("Win");
                SetDialogOn(true);

                AudioManager.Instance.PlayBGM("RoundAndRound");
            }
            else if (num == 93)
            {
                SetCharacterImage(midori, 17);
                PlayCharacterEffectAnimation(midori, "Silence");
            }
            else if (num == 94)
            {
                ShowSelection("“공격을 거의 전부 적중시켰는데도 랭킹 진입에 실패하다니……”");
            }
            else if (num == 95)
            {
                //PlayCharacterEffectAnimation(_midori, "Silence");
            }
            else if (num == 96)
            {
                ShowSelection("( 그렇다면 혹시…… )", "“내가 플레이해 봐도 괜찮을까?”");
            }
            else if (num == 97)
            {
                SetCharacterImage(midori, 10);
                midori.GetComponent<Animator>().Play("Jumping");
                PlayCharacterEffectAnimation(midori, "Talking");

                _isAvailableProgress = true;
                ShowStoryText();
                _isAvailableProgress = false;

                yield return new WaitForSeconds(1);

                DOTween.Sequence()
                    .Append(midori.transform.parent.GetComponent<RectTransform>().DOLocalMoveX(-200, 0.5f))
                    .AppendInterval(0.5f)
                    .Append(midori.transform.parent.GetComponent<RectTransform>().DOLocalMoveX(-400, 0.5f));
                AudioManager.Instance.PlaySFX("Step");

                yield return new WaitForSeconds(1);

                AudioManager.Instance.PlaySFX("Step");
                _isAvailableProgress = true;

                yield break;
            }
            else if (num == 99)
            {
                ShowSelection("( 게임을 시작한다. )");
            }
            else if (num == 100)
            {
                StartCoroutine(DoFadeStoryWindowFadeOut());
                AudioManager.Instance.FadeOutMusic();

                yield return new WaitForSeconds(2);

                SetDialogOn(false);
                storyWidget.transform.Find("UI").gameObject.SetActive(false); // UI 숨기기
                storyWidget.transform.Find("UI/Menu").gameObject.SetActive(false); // 열린 메뉴창 숨기기
                storyWidget.transform.Find("Dialog").gameObject.SetActive(false); // 대화창 숨기기
                GameManager.Instance.DoStartShootingGame();

                _isAvailableProgress = false;

                yield break;
            }
            else if (num == 102)
            {
                SetCharacterImage(midori, 13);
                PlayCharacterEffectAnimation(midori, "Talking");
                midori.GetComponent<Animator>().Play("Jumping");
            }
            else if (num == 103)
            {
                ShowSelection("( 다시 시도한다. )");
                _storyNum = 99;
            }
            else if (num == 104)
            {
                SetCharacterImage(midori, 13);
                PlayCharacterEffectAnimation(midori, "Talking");
                midori.GetComponent<Animator>().Play("Jumping");
            }
            else if (num == 105)
            {
                SetCharacterImage(midori, 17);
            }
            else if (num == 106)
            {
                SetCharacterImage(midori, 18);
                PlayCharacterEffectAnimation(midori, "Silence");
            }
            else if (num == 107)
            {
                SetCharacterImage(midori, 13);
                PlayCharacterEffectAnimation(midori, "Talking");
                midori.GetComponent<Animator>().Play("Jumping");
            }
            else if (num == 108)
            {
                ShowSelection("( 다시 시도한다. )");
                _storyNum = 99;
            }
            else if (num == 109)
            {
                AudioManager.Instance.PlaySFX("Win");
            }
            else if (num == 110)
            {
                SetCharacterImage(midori, 13);
                PlayCharacterEffectAnimation(midori, "Talking");
                midori.GetComponent<Animator>().Play("Jumping");
            }
            else if (num == 111)
            {
                SetCharacterImage(midori, 18);
            }
            else if (num == 112)
            {
                PlayCharacterEffectAnimation(midori, "Silence");
            }
            else if (num == 113)
            {
                SetCharacterImage(midori, 13);
                PlayCharacterEffectAnimation(midori, "Shine");
            }
            else if (num == 114)
            {
                ShowSelection("“아니야, 결국엔 미도리엘도 눈치챘을 거라고 생각해.”", "“잘하고 싶은 마음에 최선을 다하다 보니 주변에 신경을 못썼을 뿐이니까.”");
            }
            else if (num == 115)
            {
                SetCharacterImage(midori, 12);
                PlayCharacterEffectAnimation(midori, "Silence");
            }
            else if (num == 116)
            {
                SetCharacterImage(midori, 18);
            }
            else if (num == 117)
            {
                SetCharacterImage(midori, 17);
            }
            else if (num == 118)
            {
                ShowSelection("“언니?”");
            }
            else if (num == 119)
            {
                SetCharacterImage(midori, 14);
                PlayCharacterEffectAnimation(midori, "Sweat");
            }
            else if (num == 120)
            {
                SetCharacterImage(midori, 17);
                PlayCharacterEffectAnimation(midori, "Silence");
            }
            else if (num == 123)
            {
                ShowSelection("“동생은 여신인데, 언니는 마왕이라니……”");
            }
            else if (num == 124)
            {
                SetCharacterImage(midori, 14);
                PlayCharacterEffectAnimation(midori, "Sweat");
            }
            else if (num == 125)
            {
                SetCharacterImage(midori, 17);
            }
            else if (num == 126)
            {
                ShowSelection("“미도리엘은 언니랑 사이가 좋지 않은 거야?”");
            }
            else if (num == 127)
            {
                SetCharacterImage(midori, 12);
            }
            else if (num == 128)
            {
                SetCharacterImage(midori, 17);
            }
            else if (num == 131)
            {
                SetCharacterImage(midori, 14);
                PlayCharacterEffectAnimation(midori, "Sweat");
            }
            else if (num == 132)
            {
                ShowSelection("“아니야. 형제자매끼리 다투는 일은 종종 있는 일이지.”");
            }
            else if (num == 133)
            {
                ShowSelection("“아마 나쁘기만 한 일은 아니었을 거야.”", "“옆에서 게임하는 것을 지켜보는 것도 재미있잖아.”");
            }
            else if (num == 134)
            {
                SetCharacterImage(midori, 12);
            }
            else if (num == 135)
            {
                ShowSelection("“그리고 다른 사람의 플레이를 보면서 깨닫게 되는 점도 있지.”", "“나는 이렇게 플레이하는데, 이 사람은 이렇게 하는구나, 하고.”");
            }
            else if (num == 136)
            {
                SetCharacterImage(midori, 18);
                PlayCharacterEffectAnimation(midori, "Silence");
            }
            else if (num == 137)
            {
                SetCharacterImage(midori, 10);
            }
            else if (num == 138)
            {
                AudioManager.Instance.PlaySFX("RankingWarning");
            }
            else if (num == 139)
            {
                SetCharacterImage(midori, 12);
                PlayCharacterEffectAnimation(midori, "Talking");
                midori.GetComponent<Animator>().Play("Jumping");
            }
            else if (num == 140)
            {
                ShowSelection("“미도리엘의 이름을 등록한다.”");
            }
            else if (num == 142)
            {
                ShowSelection("“어쩔 수 없이 미도리까지만 입력한다.”");
            }
            else if (num == 144)
            {
                ShowSelection("“미도리엘이 단서를 알려주지 않았더라면”", "“나도 하이 스코어를 달성할 수 없었을 테니까.”");
            }
            else if (num == 145)
            {
                PlayCharacterEffectAnimation(midori, "Silence");
            }
            else if (num == 146)
            {
                SetCharacterImage(midori, 14);
                PlayCharacterEffectAnimation(midori, "Talking");
                midori.GetComponent<Animator>().Play("Jumping");
            }
            else if (num == 147)
            {
                ShowSelection("“미, 미도리엘!?”");
            }
            else if (num == 150)
            {
                SetCharacterImage(midori, 13);
                PlayCharacterEffectAnimation(midori, "Shine");
            }
            else if (num == 151)
            {
                PlayCharacterEffectAnimation(midori, "Talking");
                midori.GetComponent<Animator>().Play("Jumping");

                _isAvailableProgress = true;
                ShowStoryText();
                _isAvailableProgress = false;

                yield return new WaitForSeconds(1.5f);

                AudioManager.Instance.PlaySFX("Run");
                midori.transform.parent.GetComponent<RectTransform>().DOLocalMoveX(2000, 1f);
                _isAvailableProgress = true;

                yield break;
            }
            else if (num == 152)
            {
                storyBackground.DOColor(Color.black, 2);
            }
            else if (num == 153) // 메인화면으로 이동
            {
                CheckAutoProgress();

                GameManager.Instance.DoFinishStory(2);

                yield break;
            }
        }

        if (_isSelectionOn) yield break; // 선택지를 보여줄 때 대화창 갱신하지 않도록 체크

        _isAvailableProgress = true;

        ShowStoryText();
    }

    #region Method for DoStoryAction Method

    public void SetCharacterImage(GameObject characterName, int num)
    {
        if (characterName == momoi)
        {
            momoiCharacterImage.sprite = momoiImage.imageArray[num];
            // 모모이의 표정이 0번일 때는 눈을 깜빡이는 모션이 있음
            if (num == 0)
            {
                momoiCharacterImage.GetComponent<Animator>().enabled = true;
            }
            else
            {
                momoiCharacterImage.GetComponent<Animator>().enabled = false;
            }
        }
        else if (characterName == midori)
        {
            midoriCharacterImage.sprite = midoriImage.imageArray[num];
        }
        else if (characterName == aris)
        {
            arisCharacterImage.sprite = arisImage.imageArray[num];
        }
        else if (characterName == yuzu)
        {
            yuzuCharacterImage.sprite = yuzuImage.imageArray[num];
        }
    }

    private IEnumerator DoFadeStoryWindowFadeOut()
    {
        windowFadeOut.gameObject.SetActive(true);
        DOTween.Sequence().Append(windowFadeOut.DOFade(1, 2f)).Append(windowFadeOut.DOFade(0, 2f));

        yield return new WaitForSeconds(4);

        windowFadeOut.gameObject.SetActive(false);
    }

    // 해당 캐릭터의 이펙트 애니메이션을 재생하는 함수
    private void PlayCharacterEffectAnimation(GameObject characterName, string animationName)
    {
        characterName.transform.Find($"{animationName}Parent/{animationName}").GetComponent<Animator>()
            .Play(animationName);
        AudioManager.Instance.PlaySFX(animationName);
    }

    #endregion Method for DoStoryAction Method

    #region Method for Text Process

    // 캐릭터 이름, 부서 이름, 대화 내용, 텍스트 속도를 간편하게 쓰고 저장하기 위한 함수
    public void AppendDialog(string characterNameText, string departmentNameText, string dialogText, float textSpeed)
    {
        _characterNameList.Add(characterNameText);
        _departmentNameList.Add(departmentNameText);
        _dialogList.Add(dialogText);
        _textSpeedList.Add(textSpeed); // 텍스트 속도 : (textSpeed) 배
    }

    public bool ClearDialog()
    {
        if (_characterNameList == null || _departmentNameList == null || _dialogList == null || _textSpeedList == null)
            return false;

        _characterNameList.Clear();
        _departmentNameList.Clear();
        _dialogList.Clear();
        _textSpeedList.Clear();

        return true;
    }

    private IEnumerator AppendTextOneByOne(TMP_Text textBox, string text, float textSpeed)
    {
        for (int i = 0; i < text.Length; i++)
        {
            textBox.text += text[i];
            yield return new WaitForSeconds(0.05f / textSpeed);
        }
    }

    // 스토리에서 텍스트를 나타내는 함수
    private void ShowStoryText()
    {
        if (_isProgressingStory)
        {
            StopCoroutine(_coroutineStoryProgress);
            SetTextBox(_storyNum);
            return;
        }

        _coroutineStoryProgress = StartCoroutine(SetTextBoxCoroutine(_storyNum));
    }

    // 대화를 한 글자씩 출력해주는 함수(기본)
    private IEnumerator SetTextBoxCoroutine(int num)
    {
        if (_characterNameList.Count <= num) yield break;
        dialogEnd.SetActive(false);

        // 이름 길이에 따라 소속 표시 위치 변경
        if (characterName.GetComponent<TMP_Text>().text.Length != _characterNameList[num].Length) // 이름 길이가 변경되었을 때만 동작
        {
            departmentName.GetComponent<RectTransform>().anchoredPosition = new Vector2(
                -457 + ((_characterNameList[num].Length - 2) * 52),
                departmentName.GetComponent<RectTransform>().anchoredPosition.y);
        }

        // 얘기 중인 학생의 이미지를 밝게, 얘기하고 있지 않은 학생의 이미지를 어둡게 변경
        if (_characterNameList[num].Equals("모모이"))
        {
            momoiCharacterImage.color = new Color(1, 1, 1, momoiCharacterImage.color.a);
            momoiHaloImage.color = new Color(1, 1, 1, momoiHaloImage.color.a);

            midoriCharacterImage.color = new Color(0.5f, 0.5f, 0.5f, midoriCharacterImage.color.a);
            midoriHaloImage.color = new Color(0.5f, 0.5f, 0.5f, midoriHaloImage.color.a);

            arisCharacterImage.color = new Color(0.5f, 0.5f, 0.5f, arisCharacterImage.color.a);
            arisHaloImage.color = new Color(0.5f, 0.5f, 0.5f, arisHaloImage.color.a);

            yuzuCharacterImage.color = new Color(0.5f, 0.5f, 0.5f, yuzuCharacterImage.color.a);
            yuzuHaloImage.color = new Color(0.5f, 0.5f, 0.5f, yuzuHaloImage.color.a);
        }
        else if (_characterNameList[num].Equals("미도리"))
        {
            momoiCharacterImage.color = new Color(0.5f, 0.5f, 0.5f, momoiCharacterImage.color.a);
            momoiHaloImage.color = new Color(0.5f, 0.5f, 0.5f, momoiHaloImage.color.a);

            midoriCharacterImage.color = new Color(1, 1, 1, midoriCharacterImage.color.a);
            midoriHaloImage.color = new Color(1, 1, 1, midoriHaloImage.color.a);

            arisCharacterImage.color = new Color(0.5f, 0.5f, 0.5f, arisCharacterImage.color.a);
            arisHaloImage.color = new Color(0.5f, 0.5f, 0.5f, arisHaloImage.color.a);

            yuzuCharacterImage.color = new Color(0.5f, 0.5f, 0.5f, yuzuCharacterImage.color.a);
            yuzuHaloImage.color = new Color(0.5f, 0.5f, 0.5f, yuzuHaloImage.color.a);
        }
        else if (_characterNameList[num].Equals("아리스"))
        {
            momoiCharacterImage.color = new Color(0.5f, 0.5f, 0.5f, momoiCharacterImage.color.a);
            momoiHaloImage.color = new Color(0.5f, 0.5f, 0.5f, momoiHaloImage.color.a);

            midoriCharacterImage.color = new Color(0.5f, 0.5f, 0.5f, midoriCharacterImage.color.a);
            midoriHaloImage.color = new Color(0.5f, 0.5f, 0.5f, midoriHaloImage.color.a);

            arisCharacterImage.color = new Color(1, 1, 1, arisCharacterImage.color.a);
            arisHaloImage.color = new Color(1, 1, 1, arisHaloImage.color.a);

            yuzuCharacterImage.color = new Color(0.5f, 0.5f, 0.5f, yuzuCharacterImage.color.a);
            yuzuHaloImage.color = new Color(0.5f, 0.5f, 0.5f, yuzuHaloImage.color.a);
        }
        else if (_characterNameList[num].Equals("유즈"))
        {
            momoiCharacterImage.color = new Color(0.5f, 0.5f, 0.5f, momoiCharacterImage.color.a);
            momoiHaloImage.color = new Color(0.5f, 0.5f, 0.5f, momoiHaloImage.color.a);

            midoriCharacterImage.color = new Color(0.5f, 0.5f, 0.5f, midoriCharacterImage.color.a);
            midoriHaloImage.color = new Color(0.5f, 0.5f, 0.5f, midoriHaloImage.color.a);

            arisCharacterImage.color = new Color(0.5f, 0.5f, 0.5f, arisCharacterImage.color.a);
            arisHaloImage.color = new Color(0.5f, 0.5f, 0.5f, arisHaloImage.color.a);

            yuzuCharacterImage.color = new Color(1, 1, 1, yuzuCharacterImage.color.a);
            yuzuHaloImage.color = new Color(1, 1, 1, yuzuHaloImage.color.a);
        }


        characterName.GetComponent<TMP_Text>().text = _characterNameList[num];
        departmentName.GetComponent<TMP_Text>().text = _departmentNameList[num];
        dialogText.GetComponent<TMP_Text>().text = "";
        _isProgressingStory = true;
        float textSpeed = _textSpeedList[num];
        for (int i = 0; i < _dialogList[num].Length; i++)
        {
            dialogText.GetComponent<TMP_Text>().text += _dialogList[num][i];
            yield return new WaitForSeconds(0.05f / textSpeed);
        }

        _storyNum++;
        DoStoryJump(); // 선택지로 갈린 스토리 번호 이동 체크

        _isProgressingStory = false;
        dialogEnd.SetActive(true);
    }

    // 대화 나오고 있을 때 클릭하면 한 번에 출력되도록 해주는 함수
    private void SetTextBox(int num)
    {
        if (_characterNameList.Count <= num) return;
        characterName.GetComponent<TMP_Text>().text = _characterNameList[num];
        departmentName.GetComponent<TMP_Text>().text = _departmentNameList[num];
        dialogText.GetComponent<TMP_Text>().text = _dialogList[num];
        _storyNum++;
        DoStoryJump(); // 선택지로 갈린 스토리 번호 이동 체크

        _isProgressingStory = false;
        dialogEnd.SetActive(true);
    }

    #endregion Method for Text Process

    #region Method for ShootingGame

    public IEnumerator ShowSadMidori()
    {
        if (midori.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Shiver")) yield break;

        SetCharacterImage(midori, 14);
        midori.GetComponent<Animator>().Play("Shiver");

        yield return new WaitForSeconds(2f);

        SetCharacterImage(midori, 10);
    }

    public IEnumerator ShowHappyMidori()
    {
        if (midori.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Jumping")) yield break;

        SetCharacterImage(midori, 13);
        midori.GetComponent<Animator>().Play("Jumping");

        yield return new WaitForSeconds(1f);

        SetCharacterImage(midori, 10);
    }

    public void DoShootingGameEnd(string endType)
    {
        StartCoroutine(DoShootingGameEndCoroutine(endType));
    }

    public IEnumerator DoShootingGameEndCoroutine(string endType)
    {
        yield return new WaitForSeconds(2);

        StartCoroutine(DoFadeStoryWindowFadeOut());

        yield return new WaitForSeconds(2);

        AudioManager.Instance.PlayBGM("RoundAndRound");

        GameManager.Instance.DoFinishShootingGame();

        midori.SetActive(true);
        midori.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(-400, 0);
        midori.transform.parent.GetComponent<RectTransform>().localScale = new Vector2(1, 1);

        if (endType.Equals("GameOver")) SetCharacterImage(midori, 17);
        else SetCharacterImage(midori, 13);

        yield return new WaitForSeconds(2);

        SetDialogOn(true);
        storyWidget.transform.Find("UI").gameObject.SetActive(true);
        _isAvailableProgress = true;

        if (endType.Equals("GameOver")) _storyNum = 101;
        else if (endType.Equals("GameWin")) _storyNum = 104;
        else _storyNum = 109;

        DoStoryProgress();
    }

    #endregion Method For ShootingGame

    // 스토리를 진행시키는 함수
    public void DoStoryProgress()
    {
        if (!_isAvailableProgress)
        {
            return;
        }

        _elapsedTime = 0f;
        StartCoroutine(DoStoryAction(_storyNum));
    }

    // GameManager에서 스토리 시작 번호를 지정할 때 사용하는 함수
    public void SetStoryNumber(int num)
    {
        _storyNum = num;
    }

    // 외부에서 스토리를 진행할 수 있는 상태인지 여부를 변경하는 함수
    public void SetIsAvailableProgress(bool isCanProgress)
    {
        _isAvailableProgress = isCanProgress;
    }

    // 스토리 진행 중 화면을 클릭했을 때 작동하는 함수
    private void OnClickStoryProgressBtn()
    {
        if (storyWidget.transform.Find("UI/Menu").gameObject.activeSelf)
            storyWidget.transform.Find("UI/Menu").gameObject.SetActive(false);
        if (storyWidget.transform.Find("UI").gameObject.activeSelf == false)
        {
            storyWidget.transform.Find("UI").gameObject.SetActive(true);
            if (!_isDialogOff) storyWidget.transform.Find("Dialog").gameObject.SetActive(true);
        }
        else
        {
            DoStoryProgress();
        }
    }

    private void OnClickAutoBtn()
    {
        DoAutoProgress();
        AudioManager.Instance.PlaySFX("ButtonClick_BlueArchive");
    }

    private void OnClickMenuBtn()
    {
        // 메뉴창 여닫기
        storyWidget.transform.Find("UI/Menu").gameObject
            .SetActive(!storyWidget.transform.Find("UI/Menu").gameObject.activeSelf);
        AudioManager.Instance.PlaySFX("ButtonClick_BlueArchive");
    }

    private void OnClickHideUI()
    {
        CheckAutoProgress(); // 스토리가 자동으로 진행 중이라면 멈추도록 함

        storyWidget.transform.Find("UI/Menu").gameObject.SetActive(false); // 열린 메뉴창 숨기기
        storyWidget.transform.Find("UI").gameObject.SetActive(false); // UI 숨기기
        storyWidget.transform.Find("Dialog").gameObject.SetActive(false); // 대화창 숨기기
        AudioManager.Instance.PlaySFX("ButtonClick_BlueArchive");
    }

    private void OnClickSkipBtn()
    {
        // 개선 필요
        GameManager.Instance.DoFinishStory(currentStage);
    }

    private void OnClickSingleSelection()
    {
        StartCoroutine(SelectedSelection(0));
        AudioManager.Instance.PlaySFX("ButtonClick_BlueArchive");
    }

    private void OnClickSelection1()
    {
        StartCoroutine(SelectedSelection(1));
        AudioManager.Instance.PlaySFX("ButtonClick_BlueArchive");
    }

    private void OnClickSelection2()
    {
        StartCoroutine(SelectedSelection(2));
        AudioManager.Instance.PlaySFX("ButtonClick_BlueArchive");
    }
}