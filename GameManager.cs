using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using Application = UnityEngine.Application;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;
using Slider = UnityEngine.UI.Slider;

namespace SingletonPattern
{
    public class GameManager : Singleton<GameManager>
    {
        #region Variables
        enum StartFrom
        {
            intro, story, Game1
        }

        [Header("Start Settings")]
        [SerializeField] StartFrom _startFrom;
        [SerializeField] int _storyNumber = 0;

        [Header("GameObject")]
        [SerializeField] public GameObject _canvas;
        private TMP_Text _guideText;
        private GameObject _option;
        private GameObject _selectStage;
        private GameObject _startBtn;
        private GameObject _optionBtn;
        private GameObject _gameQuitBtn;

        private Button _tempBtn;

        [Header("Select Stage Sprite")]
        [SerializeField] Sprite _stageEnterImage;
        [SerializeField] Sprite _storyReadImage;
        [SerializeField] Sprite _stageEnterImageJapanese;
        [SerializeField] Sprite _stageEnterLockedImageJapanse;

        [Header("Font")]
        [SerializeField] TMP_FontAsset _japaneseFontWithUnderlay;
        [SerializeField] TMP_FontAsset _japaneseFont;

        AudioManager _audioManager;
        StoryManager _storyManager;
        ShootingGameManager _shootingGameManager;

        private bool _isStartedStory = false;

        #endregion Variables

        void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                _tempBtn.Select(); // 마우스를 클릭한 뒤 다른 지점으로 이동해서 뗐을 때 다른 버튼을 선택하여 애니메이션 정상화
            }
        }

        #region Initialize Game

        public override void Awake()
        {
            if(_audioManager == null) _audioManager = AudioManager.Instance;
            if(_storyManager == null) _storyManager = StoryManager.Instance;
            if(_shootingGameManager == null) _shootingGameManager = ShootingGameManager.Instance;

            _guideText = _canvas.transform.Find("Main/GuideText").GetComponent<TMP_Text>();
            _option = _canvas.transform.Find("Option").gameObject;
            _selectStage = _canvas.transform.Find("SelectStage").gameObject;
            _tempBtn = _canvas.transform.Find("Main/TempBtn").GetComponent<Button>();

            _startBtn = _canvas.transform.Find("Main/StartBtn").gameObject;
            _optionBtn = _canvas.transform.Find("Main/OptionBtn").gameObject;
            _gameQuitBtn = _canvas.transform.Find("Main/GameQuitBtn").gameObject;

            // 초기 PlayerPrefs 설정
            if (!PlayerPrefs.HasKey("Language")) PlayerPrefs.SetString("Language", "Korean");
            if (!PlayerPrefs.HasKey("BGM")) PlayerPrefs.SetFloat("BGM", 1);
            if (!PlayerPrefs.HasKey("SFX")) PlayerPrefs.SetFloat("SFX", 1);
            if (!PlayerPrefs.HasKey("FPS")) PlayerPrefs.SetInt("FPS", 60);
        }

        void Start()
        {
            if (_startFrom == StartFrom.Game1)
            {
                _canvas.transform.Find("Game1").gameObject.SetActive(true);
                _shootingGameManager.InitializeShootingGame();
                return;
            }
            StartCoroutine(DoLoading());
            SetSelectStage();
        }

        // 스토리부터 시작할 때는 잠깐 로딩을 갖도록 함
        private IEnumerator DoLoading()
        {
            if (_startFrom == StartFrom.intro) StartCoroutine(DoOpening());
            else if (_startFrom == StartFrom.story)
            {
                yield return new WaitForSeconds(1);
                _storyManager._story.SetActive(true);
                _storyManager.SetStoryNumber(_storyNumber);
                _storyManager.SetIsCanProgress(true);
                _storyManager.DoStoryProgress();
                SetSelectStage();
            }
        }

        // 게임이 처음부터 시작할 때 오프닝을 보여줌
        private IEnumerator DoOpening()
        {
            Image aronaImage = _canvas.transform.Find("Main/Arona").GetComponent<Image>();

            // 시작할 때 필요없는 것들 해제
            _startBtn.SetActive(false);
            _optionBtn.SetActive(false);
            _gameQuitBtn.SetActive(false);
            _storyManager._story.SetActive(false);
            _option.SetActive(false);

            // 유저가 설정한 정보 불러와서 적용
            if (PlayerPrefs.HasKey("BGM"))
            {
                float bgm = PlayerPrefs.GetFloat("BGM");
                _audioManager.SetBGMVolume(bgm);
                _option.transform.Find("OptionWindow/BGMSlider").GetComponent<Slider>().value = bgm;
            }
            if (PlayerPrefs.HasKey("SFX"))
            {
                float sfx = PlayerPrefs.GetFloat("SFX");
                _audioManager.SetSFXVolume(sfx);
                _option.transform.Find("OptionWindow/SFXSlider").GetComponent<Slider>().value = sfx;
            }
            if (PlayerPrefs.HasKey("FPS"))
            {
                int fps = PlayerPrefs.GetInt("FPS");
                Application.targetFrameRate = fps;
                if (fps == 30)
                {
                    _option.transform.Find("OptionWindow/FPS/FPS30Btn/Selected").gameObject.SetActive(true);
                    _option.transform.Find("OptionWindow/FPS/FPS60Btn/Selected").gameObject.SetActive(false);
                }
                else
                {
                    _option.transform.Find("OptionWindow/FPS/FPS30Btn/Selected").gameObject.SetActive(false);
                    _option.transform.Find("OptionWindow/FPS/FPS60Btn/Selected").gameObject.SetActive(true);
                }
            }
            if (PlayerPrefs.HasKey("Language"))
            {
                if (PlayerPrefs.GetString("Language").Equals("Korean"))
                {
                    _canvas.transform.Find("Option/OptionWindow/Language/KoreanBtn/Selected").gameObject.SetActive(true);
                    _canvas.transform.Find("Option/OptionWindow/Language/JapaneseBtn/Selected").gameObject.SetActive(false);

                    _canvas.transform.Find("Story/Episode/Dialog/CharacterName").GetComponent<RectTransform>().anchoredPosition = new Vector2(-622, -266);
                    _canvas.transform.Find("Story/Episode/Dialog/DepartmentName").GetComponent<RectTransform>().anchoredPosition = new Vector2(-405, -279);
                    _canvas.transform.Find("Story/Episode/Dialog/DialogText").GetComponent<RectTransform>().anchoredPosition = new Vector2(-14, -433);
                }
                else if (PlayerPrefs.GetString("Language").Equals("Japanese"))
                {
                    _canvas.transform.Find("Option/OptionWindow/Language/KoreanBtn/Selected").gameObject.SetActive(false);
                    _canvas.transform.Find("Option/OptionWindow/Language/JapaneseBtn/Selected").gameObject.SetActive(true);

                    GameObject selection1Btn = _storyManager._selection1Btn.transform.GetChild(0).gameObject;
                    GameObject selection1_1Btn = _storyManager._selection1_1Btn.transform.GetChild(0).gameObject;
                    GameObject selection1_2Btn = _storyManager._selection1_2Btn.transform.GetChild(0).gameObject;

                    // 스토리 로컬라이징
                    DoLocalizing(_storyManager._characterName.GetComponent<TMP_Text>(), _japaneseFontWithUnderlay, "", -24);
                    _storyManager._characterName.GetComponent<TMP_Text>().characterSpacing = -4.5f;

                    DoLocalizing(_storyManager._departmentName.GetComponent<TMP_Text>(), _japaneseFontWithUnderlay, "", -19);
                    _storyManager._departmentName.GetComponent<TMP_Text>().characterSpacing = -4.5f;

                    DoLocalizing(_storyManager._dialogText.GetComponent<TMP_Text>(), _japaneseFontWithUnderlay, "", -17);
                    _storyManager._dialogText.GetComponent<TMP_Text>().characterSpacing = -4.5f;
                    _storyManager._dialogText.GetComponent<TMP_Text>().lineSpacing = 0;

                    DoLocalizing(selection1Btn.GetComponent<TMP_Text>(), _japaneseFont);
                    DoLocalizing(selection1_1Btn.GetComponent<TMP_Text>(), _japaneseFont);
                    DoLocalizing(selection1_2Btn.GetComponent<TMP_Text>(), _japaneseFont);

                    DoLocalizing(_storyManager._windowText, _japaneseFont);
                    DoLocalizing(_storyManager._gameOver2Text, _japaneseFont, "プニプニ：どれだけ剣術を鍛えたところで、我が銃の前では無力……ふっ。");

                    // 인트로 ~ 메인화면 로컬라이징

                    DoLocalizing(_guideText, _japaneseFont, "このゲームはNexon Gamesで開発したブルーアーカイブの\nファンメイドゲームで、公式コンテンツではありません。");

                    DoLocalizing(_startBtn.transform.GetChild(0).GetComponent<TMP_Text>(), _japaneseFontWithUnderlay, "スタート");
                    DoLocalizing(_optionBtn.transform.GetChild(0).GetComponent<TMP_Text>(), _japaneseFontWithUnderlay, "オプション");
                    DoLocalizing(_gameQuitBtn.transform.GetChild(0).GetComponent<TMP_Text>(), _japaneseFontWithUnderlay, "ゲーム終了");

                    DoLocalizing(_option.transform.Find("OptionWindow/Language/Text").GetComponent<TMP_Text>(), _japaneseFont, "言語", -13);
                    DoLocalizing(_option.transform.Find("OptionWindow/OptionText").GetComponent<TMP_Text>(), _japaneseFont, "オプション", -20);
                    DoLocalizing(_option.transform.Find("OptionWindow/SFXSlider/Text").GetComponent<TMP_Text>(), _japaneseFont, "効果音", -13);

                    DoLocalizing(_selectStage.transform.Find("SelectStageWindow/ChapterName").GetComponent<TMP_Text>(), _japaneseFont, "レトロチック・ロマン");
                    DoLocalizing(_selectStage.transform.Find("SelectStageWindow/ChapterContents").GetComponent<TMP_Text>(), _japaneseFont,
                        "ゲーム開発部で開発したテイルズ・サガ・クロニクルをプレイすることになりました。「今年のクソゲーランキング１位」のゲームを無事にクリアできましょうか？");
                    DoLocalizing(_selectStage.transform.Find("SelectStageWindow/ChapterNumber").GetComponent<TMP_Text>(), _japaneseFont, "1章");
                    DoLocalizing(_selectStage.transform.Find("SelectStageWindow/EpisodeList").GetComponent<TMP_Text>(), _japaneseFont, "エピソードリスト");
                    DoLocalizing(_selectStage.transform.Find("SelectStageWindow/StageAllClear").GetComponent<TMP_Text>(), _japaneseFont, "すべてのエピソードをクリアしました。");
                    _selectStage.transform.Find("SelectStageWindow/ChapterContents").GetComponent<TMP_Text>().lineSpacing = 0;
                    _selectStage.transform.Find("SelectStageWindow/ChapterContents").GetComponent<TMP_Text>().characterSpacing = -5;

                    DoLocalizing(_selectStage.transform.Find("SelectStageWindow/Stage1/StageName").GetComponent<TMP_Text>(), _japaneseFont, "テイルズ・サガ・クロニクル");
                    DoLocalizing(_selectStage.transform.Find("SelectStageWindow/Stage2/StageName").GetComponent<TMP_Text>(), _japaneseFont, "？？？");
                    DoLocalizing(_selectStage.transform.Find("SelectStageWindow/Stage3/StageName").GetComponent<TMP_Text>(), _japaneseFont, "？？？");
                    DoLocalizing(_selectStage.transform.Find("SelectStageWindow/Stage4/StageName").GetComponent<TMP_Text>(), _japaneseFont, "？？？");
                    _selectStage.transform.Find("SelectStageWindow/Stage1/StageName").GetComponent<TMP_Text>().characterSpacing = -13;

                    _selectStage.transform.Find("SelectStageWindow/Stage1/Stage1Enter").GetComponent<Button>().image.sprite = _stageEnterImageJapanese;
                    _selectStage.transform.Find("SelectStageWindow/Stage2/Stage2Enter").GetComponent<Button>().image.sprite = _stageEnterLockedImageJapanse;
                    _selectStage.transform.Find("SelectStageWindow/Stage3/Stage3Enter").GetComponent<Button>().image.sprite = _stageEnterLockedImageJapanse;
                    _selectStage.transform.Find("SelectStageWindow/Stage4/Stage4Enter").GetComponent<Button>().image.sprite = _stageEnterLockedImageJapanse;

                    DoLocalizing(_canvas.transform.Find("Story/EpisodeStart/Text/ChapterNumber").GetComponent<TMP_Text>(), _japaneseFont, "第1話");
                    DoLocalizing(_canvas.transform.Find("Story/EpisodeStart/Text/ChapterName").GetComponent<TMP_Text>(), _japaneseFont, "テイルズ・サガ・クロニクル");
                }
            }

            aronaImage.gameObject.SetActive(true);
            aronaImage.color = new Color(1, 1, 1, 0);
            _guideText.color = new Color(1, 1, 1, 0);

            DOTween.Sequence().Append(aronaImage.DOFade(1.0f, 3).SetEase(Ease.Linear)).Join(_guideText.DOFade(1.0f, 3).SetEase(Ease.Linear)).AppendInterval(2)
                .Append(aronaImage.DOFade(0f, 2).SetEase(Ease.Linear)).Join(_guideText.DOFade(0f, 2).SetEase(Ease.Linear));

            yield return new WaitForSeconds(7);
            aronaImage.gameObject.SetActive(false);
            _audioManager.PlayBGM("Restart");

            _canvas.transform.Find("Main/MainBackground").GetComponent<Image>().DOFade(1, 2);
            yield return new WaitForSeconds(1);
            _canvas.transform.Find("Main/MainCharacter").GetComponent<Image>().DOFade(1, 2);
            yield return new WaitForSeconds(1);
            _canvas.transform.Find("Main/Title1").GetComponent<Image>().DOFade(1, 2);
            yield return new WaitForSeconds(1);
            _audioManager.PlaySFX("8bitBomb");
            _canvas.transform.Find("Main/Title2").GetComponent<Image>().DOFade(1, 2);
            _canvas.transform.Find("Main/Title2").GetComponent<Animator>().Play("Blast");
            yield return new WaitForSeconds(2);
            _canvas.transform.Find("Main/StartBtn").gameObject.SetActive(true);
            _canvas.transform.Find("Main/StartBtn/Text").GetComponent<TMP_Text>().DOFade(1, 1);
            yield return new WaitForSeconds(0.5f);
            _canvas.transform.Find("Main/OptionBtn").gameObject.SetActive(true);
            _canvas.transform.Find("Main/OptionBtn/Text").GetComponent<TMP_Text>().DOFade(1, 1);
            yield return new WaitForSeconds(0.5f);
            _canvas.transform.Find("Main/GameQuitBtn").gameObject.SetActive(true);
            _canvas.transform.Find("Main/GameQuitBtn/Text").GetComponent<TMP_Text>().DOFade(1, 1);
        }

        private void DoLocalizing(TMP_Text textObject, TMP_FontAsset font, string text = "", float yPosition = -10)
        {
            textObject.font = font;
            textObject.text = text;

            textObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(textObject.GetComponent<RectTransform>().anchoredPosition.x, textObject.GetComponent<RectTransform>().anchoredPosition.y + yPosition);
        }

        #endregion Initialize Game

        #region For StoryManager.cs
        // 스테이지를 클리어하면 다음 스테이지를 오픈
        public void SetSelectStage()
        {
            if (PlayerPrefs.HasKey("Stage1Cleared"))
            {
                GameObject stage = _selectStage.transform.Find("SelectStageWindow/Stage2").gameObject;
                if (PlayerPrefs.GetString("Language").Equals("Korean"))
                {
                    stage.transform.Find("StageName").GetComponent<TMP_Text>().text = "모험의 시작";
                    stage.transform.Find("Stage2Enter").GetComponent<Button>().image.sprite = _stageEnterImage;
                }
                else if (PlayerPrefs.GetString("Language").Equals("Japanese"))
                {
                    stage.transform.Find("StageName").GetComponent<TMP_Text>().text = "冒険の始まり";
                    stage.transform.Find("Stage2Enter").GetComponent<Button>().image.sprite = _stageEnterImageJapanese;
                }
                _selectStage.transform.Find("SelectStageWindow/Stage1/StageRead").GetComponent<Image>().sprite = _storyReadImage;
            }
            if (PlayerPrefs.HasKey("Stage2Cleared"))
            {
                GameObject stage = _selectStage.transform.Find("SelectStageWindow/Stage3").gameObject;
                if (PlayerPrefs.GetString("Language").Equals("Korean"))
                {
                    stage.transform.Find("StageName").GetComponent<TMP_Text>().text = "스테이지3";
                    stage.transform.Find("Stage3Enter").GetComponent<Button>().image.sprite = _stageEnterImage;
                }
                else if (PlayerPrefs.GetString("Language").Equals("Japanese"))
                {
                    stage.transform.Find("StageName").GetComponent<TMP_Text>().text = "ステージ３";
                    stage.transform.Find("Stage3Enter").GetComponent<Button>().image.sprite = _stageEnterImageJapanese;
                }
                _selectStage.transform.Find("SelectStageWindow/Stage2/StageRead").GetComponent<Image>().sprite = _storyReadImage;
            }
            if (PlayerPrefs.HasKey("Stage3Cleared"))
            {
                GameObject stage = _selectStage.transform.Find("SelectStageWindow/Stage4").gameObject;
                if (PlayerPrefs.GetString("Language").Equals("Korean"))
                {
                    stage.transform.Find("StageName").GetComponent<TMP_Text>().text = "스테이지4";
                    stage.transform.Find("Stage4Enter").GetComponent<Button>().image.sprite = _stageEnterImage;
                }
                else if (PlayerPrefs.GetString("Language").Equals("Japanese"))
                {
                    stage.transform.Find("StageName").GetComponent<TMP_Text>().text = "ステージ４";
                    stage.transform.Find("Stage4Enter").GetComponent<Button>().image.sprite = _stageEnterImageJapanese;
                }
                _selectStage.transform.Find("SelectStageWindow/Stage3/StageRead").GetComponent<Image>().sprite = _storyReadImage;
            }
            if (PlayerPrefs.HasKey("Stage4Cleared"))
            {
                _selectStage.transform.Find("SelectStageWindow/Stage4/StageRead").GetComponent<Image>().sprite = _storyReadImage;
                _selectStage.transform.Find("SelectStageWindow/StageAllClear").gameObject.SetActive(true);
            }
        }

        // 스토리 시작 버튼을 눌렀을 때 작동
        public void DoStartStory(int storyNum)
        {
            GameObject episodeStartText = _canvas.transform.Find("Story/EpisodeStart/Text").gameObject;

            if (storyNum == 0)
            {
                _storyManager.SetStoryNumber(storyNum); // 1화 시작 부분
                
                if(PlayerPrefs.GetString("Language").Equals("Korean"))
                {
                    episodeStartText.transform.Find("ChapterNumber").GetComponent<TMP_Text>().text = "제1화";
                    episodeStartText.transform.Find("ChapterName").GetComponent<TMP_Text>().text = "테일즈 사가 크로니클";
                }
                else if (PlayerPrefs.GetString("Language").Equals("Japanese"))
                {
                    episodeStartText.transform.Find("ChapterNumber").GetComponent<TMP_Text>().text = "第1話";
                    episodeStartText.transform.Find("ChapterName").GetComponent<TMP_Text>().text = "テイルズ・サガ・クロニクル";
                }
            }
            else if (storyNum == 1)
            {
                if (!PlayerPrefs.HasKey("Stage1Cleared"))
                {
                    _audioManager.PlaySFX("ButtonCancel");
                    return;
                }
                // 현재 일본어 1장까지만 번역되어있음
                if (PlayerPrefs.GetString("Language").Equals("Japanese"))
                {
                    _audioManager.PlaySFX("ButtonCancel");
                    return;
                }
                    _storyManager.SetStoryNumber(45); // 2화 시작 부분

                if (PlayerPrefs.GetString("Language").Equals("Korean"))
                {
                    episodeStartText.transform.Find("ChapterNumber").GetComponent<TMP_Text>().text = "제2화";
                    episodeStartText.transform.Find("ChapterName").GetComponent<TMP_Text>().text = "모험의 시작";
                }
                else if (PlayerPrefs.GetString("Language").Equals("Japanese"))
                {
                    episodeStartText.transform.Find("ChapterNumber").GetComponent<TMP_Text>().text = "第2話";
                    episodeStartText.transform.Find("ChapterName").GetComponent<TMP_Text>().text = "冒険のはじまり";
                }
            }
            else if (storyNum == 2)
            {
                if (!PlayerPrefs.HasKey("Stage2Cleared"))
                {
                    _audioManager.PlaySFX("ButtonCancel");
                    return;
                }
                // 현재 3화 미구현
                _audioManager.PlaySFX("ButtonCancel");
                return;

                /*
                _storyManager.SetStoryNumber(0); // 3화 시작 부분
                episodeStartText.transform.Find("ChapterNumber").GetComponent<TMP_Text>().text = "제3화";
                episodeStartText.transform.Find("ChapterName").GetComponent<TMP_Text>().text = "제3화";
                */
            }
            else if (storyNum == 3)
            {
                if (!PlayerPrefs.HasKey("Stage3Cleared"))
                {
                    _audioManager.PlaySFX("ButtonCancel");
                    return;
                }
                // 현재 4화 미구현
                _audioManager.PlaySFX("ButtonCancel");
                return;

                /*
                _storyManager.SetStoryNumber(0); // 4화 시작 부분
                episodeStartText.transform.Find("ChapterNumber").GetComponent<TMP_Text>().text = "제4화";
                episodeStartText.transform.Find("ChapterName").GetComponent<TMP_Text>().text = "제4화";
                */
            }
            _isStartedStory = true;
            _audioManager.PlaySFX("ButtonStart");
            StartCoroutine(_storyManager.InitializeStory());
        }

        // 스토리가 끝났을 때 작동
        public void DoFinishStory(int storyNum)
        {
            _isStartedStory = false;
            _storyManager._story.gameObject.SetActive(false);
            _canvas.transform.Find("Story/Episode").gameObject.SetActive(false);
            _audioManager.PlayBGM("Restart");
            PlayerPrefs.SetInt("Stage" + (storyNum + 1) + "Cleared", 1);
            SetSelectStage();
        }
        #endregion For StoryManager.cs

        #region For ShootingGameManager.cs
        // 슈팅 게임을 시작함
        public void DoStartShootingGame()
        {
            _canvas.transform.Find("Game1").gameObject.SetActive(true);

            _shootingGameManager.InitializeShootingGame();
        }

        // 슈팅 게임을 종료함
        public void DoFinishShootingGame()
        {
            _canvas.transform.Find("Game1").gameObject.SetActive(false);
        }
        #endregion For ShootingGameManager.cs

        public IEnumerator FadeInImage(float timeSpeed, Image image)
        {
            while (image.color.a < 1.0f)
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a + (Time.deltaTime * timeSpeed));
                yield return null;
            }
        }
        public IEnumerator FadeOutImage(float timeSpeed, Image image)
        {
            while (image.color.a > 0.0f)
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a - (Time.deltaTime * timeSpeed));
                yield return null;
            }
        }

        public bool GetIsStartedStory()
        {
            return _isStartedStory;
        }
    }
}
