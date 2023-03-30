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
        private Button _tempBtn;

        [Header("Select Stage Sprite")]
        [SerializeField] Sprite _stageEnterImage;
        [SerializeField] Sprite _storyReadImage;

        AudioManager _audioManager;
        StoryManager _storyManager;
        ShootingGameManager _shootingGameManager;

        private bool _isStartedStory = false;

        public override void Awake()
        {
            _audioManager = AudioManager.Instance;
            _storyManager = StoryManager.Instance;
            _shootingGameManager = ShootingGameManager.Instance;

            _guideText = _canvas.transform.Find("Main/GuideText").GetComponent<TMP_Text>();
            _option = _canvas.transform.Find("Option").gameObject;
            _selectStage = _canvas.transform.Find("SelectStage").gameObject;
            _tempBtn = _canvas.transform.Find("Main/TempBtn").GetComponent<Button>();

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
                _shootingGameManager.SetInitial();
                return;
            }
            StartCoroutine(DoLoading());
            SetSelectStage();
        }

        void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                _tempBtn.Select(); // 마우스를 클릭한 뒤 다른 지점으로 이동해서 뗐을 때 다른 버튼을 선택하여 애니메이션 정상화
            }
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
            _canvas.transform.Find("Main/StartBtn").gameObject.SetActive(false);
            _canvas.transform.Find("Main/OptionBtn").gameObject.SetActive(false);
            _canvas.transform.Find("Main/GameQuitBtn").gameObject.SetActive(false);
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
            // if (PlayerPrefs.HasKey("Language"));

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

        // 스토리 시작 버튼을 눌렀을 때 작동
        public void DoStartStory(int storyNum)
        {
            GameObject episodeStartText = _canvas.transform.Find("Story/EpisodeStart/Text").gameObject;

            if (storyNum == 0)
            {
                _storyManager.SetStoryNumber(storyNum); // 1화 시작 부분
                episodeStartText.transform.Find("ChapterNumber").GetComponent<TMP_Text>().text = "제1화";
                episodeStartText.transform.Find("ChapterName").GetComponent<TMP_Text>().text = "테일즈 사가 크로니클";
            }
            else if (storyNum == 1)
            {
                if (!PlayerPrefs.HasKey("Stage1Cleared"))
                {
                    _audioManager.PlaySFX("ButtonCancel");
                    return;
                }
                _storyManager.SetStoryNumber(45); // 2화 시작 부분
                episodeStartText.transform.Find("ChapterNumber").GetComponent<TMP_Text>().text = "제2화";
                episodeStartText.transform.Find("ChapterName").GetComponent<TMP_Text>().text = "모험의 시작";
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
            StartCoroutine(_storyManager.DoEpisodeStart());
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

        // 스테이지를 클리어하면 다음 스테이지를 오픈함
        public void SetSelectStage()
        {
            if (PlayerPrefs.HasKey("Stage1Cleared"))
            {
                GameObject stage = _selectStage.transform.Find("SelectStageWindow/Stage2").gameObject;
                stage.transform.Find("StageName").GetComponent<TMP_Text>().text = "모험의 시작";
                stage.transform.Find("Stage2Enter").GetComponent<Button>().image.sprite = _stageEnterImage;
                _selectStage.transform.Find("SelectStageWindow/Stage1/StageRead").GetComponent<Image>().sprite = _storyReadImage;
            }
            if (PlayerPrefs.HasKey("Stage2Cleared"))
            {
                GameObject stage = _selectStage.transform.Find("SelectStageWindow/Stage3").gameObject;
                stage.transform.Find("StageName").GetComponent<TMP_Text>().text = "스테이지3";
                stage.transform.Find("Stage3Enter").GetComponent<Button>().image.sprite = _stageEnterImage;
                _selectStage.transform.Find("SelectStageWindow/Stage2/StageRead").GetComponent<Image>().sprite = _storyReadImage;
            }
            if (PlayerPrefs.HasKey("Stage3Cleared"))
            {
                GameObject stage = _selectStage.transform.Find("SelectStageWindow/Stage4").gameObject;
                stage.transform.Find("StageName").GetComponent<TMP_Text>().text = "스테이지4";
                stage.transform.Find("Stage4Enter").GetComponent<Button>().image.sprite = _stageEnterImage;
                _selectStage.transform.Find("SelectStageWindow/Stage3/StageRead").GetComponent<Image>().sprite = _storyReadImage;
            }
            if (PlayerPrefs.HasKey("Stage4Cleared"))
            {
                _selectStage.transform.Find("SelectStageWindow/Stage4/StageRead").GetComponent<Image>().sprite = _storyReadImage;
                _selectStage.transform.Find("SelectStageWindow/StageAllClear").gameObject.SetActive(true);
            }
        }

        // 슈팅 게임을 시작함
        public void DoStartShootingGame()
        {
            _canvas.transform.Find("Game1").gameObject.SetActive(true);

            _shootingGameManager.SetInitial();
        }

        // 슈팅 게임을 종료함
        public void DoFinishShootingGame()
        {
            _canvas.transform.Find("Game1").gameObject.SetActive(false);
        }

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
