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

            // �ʱ� PlayerPrefs ����
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
                _tempBtn.Select(); // ���콺�� Ŭ���� �� �ٸ� �������� �̵��ؼ� ���� �� �ٸ� ��ư�� �����Ͽ� �ִϸ��̼� ����ȭ
            }
        }

        // ���丮���� ������ ���� ��� �ε��� ������ ��
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

        // ������ ó������ ������ �� �������� ������
        private IEnumerator DoOpening()
        {
            Image aronaImage = _canvas.transform.Find("Main/Arona").GetComponent<Image>();

            // ������ �� �ʿ���� �͵� ����
            _canvas.transform.Find("Main/StartBtn").gameObject.SetActive(false);
            _canvas.transform.Find("Main/OptionBtn").gameObject.SetActive(false);
            _canvas.transform.Find("Main/GameQuitBtn").gameObject.SetActive(false);
            _storyManager._story.SetActive(false);
            _option.SetActive(false);

            // ������ ������ ���� �ҷ��ͼ� ����
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

        // ���丮 ���� ��ư�� ������ �� �۵�
        public void DoStartStory(int storyNum)
        {
            GameObject episodeStartText = _canvas.transform.Find("Story/EpisodeStart/Text").gameObject;

            if (storyNum == 0)
            {
                _storyManager.SetStoryNumber(storyNum); // 1ȭ ���� �κ�
                episodeStartText.transform.Find("ChapterNumber").GetComponent<TMP_Text>().text = "��1ȭ";
                episodeStartText.transform.Find("ChapterName").GetComponent<TMP_Text>().text = "������ �簡 ũ�δ�Ŭ";
            }
            else if (storyNum == 1)
            {
                if (!PlayerPrefs.HasKey("Stage1Cleared"))
                {
                    _audioManager.PlaySFX("ButtonCancel");
                    return;
                }
                _storyManager.SetStoryNumber(45); // 2ȭ ���� �κ�
                episodeStartText.transform.Find("ChapterNumber").GetComponent<TMP_Text>().text = "��2ȭ";
                episodeStartText.transform.Find("ChapterName").GetComponent<TMP_Text>().text = "������ ����";
            }
            else if (storyNum == 2)
            {
                if (!PlayerPrefs.HasKey("Stage2Cleared"))
                {
                    _audioManager.PlaySFX("ButtonCancel");
                    return;
                }
                // ���� 3ȭ �̱���
                _audioManager.PlaySFX("ButtonCancel");
                return;

                /*
                _storyManager.SetStoryNumber(0); // 3ȭ ���� �κ�
                episodeStartText.transform.Find("ChapterNumber").GetComponent<TMP_Text>().text = "��3ȭ";
                episodeStartText.transform.Find("ChapterName").GetComponent<TMP_Text>().text = "��3ȭ";
                */
            }
            else if (storyNum == 3)
            {
                if (!PlayerPrefs.HasKey("Stage3Cleared"))
                {
                    _audioManager.PlaySFX("ButtonCancel");
                    return;
                }
                // ���� 4ȭ �̱���
                _audioManager.PlaySFX("ButtonCancel");
                return;

                /*
                _storyManager.SetStoryNumber(0); // 4ȭ ���� �κ�
                episodeStartText.transform.Find("ChapterNumber").GetComponent<TMP_Text>().text = "��4ȭ";
                episodeStartText.transform.Find("ChapterName").GetComponent<TMP_Text>().text = "��4ȭ";
                */
            }
            _isStartedStory = true;
            _audioManager.PlaySFX("ButtonStart");
            StartCoroutine(_storyManager.DoEpisodeStart());
        }

        // ���丮�� ������ �� �۵�
        public void DoFinishStory(int storyNum)
        {
            _isStartedStory = false;
            _storyManager._story.gameObject.SetActive(false);
            _canvas.transform.Find("Story/Episode").gameObject.SetActive(false);
            _audioManager.PlayBGM("Restart");
            PlayerPrefs.SetInt("Stage" + (storyNum + 1) + "Cleared", 1);
            SetSelectStage();
        }

        // ���������� Ŭ�����ϸ� ���� ���������� ������
        public void SetSelectStage()
        {
            if (PlayerPrefs.HasKey("Stage1Cleared"))
            {
                GameObject stage = _selectStage.transform.Find("SelectStageWindow/Stage2").gameObject;
                stage.transform.Find("StageName").GetComponent<TMP_Text>().text = "������ ����";
                stage.transform.Find("Stage2Enter").GetComponent<Button>().image.sprite = _stageEnterImage;
                _selectStage.transform.Find("SelectStageWindow/Stage1/StageRead").GetComponent<Image>().sprite = _storyReadImage;
            }
            if (PlayerPrefs.HasKey("Stage2Cleared"))
            {
                GameObject stage = _selectStage.transform.Find("SelectStageWindow/Stage3").gameObject;
                stage.transform.Find("StageName").GetComponent<TMP_Text>().text = "��������3";
                stage.transform.Find("Stage3Enter").GetComponent<Button>().image.sprite = _stageEnterImage;
                _selectStage.transform.Find("SelectStageWindow/Stage2/StageRead").GetComponent<Image>().sprite = _storyReadImage;
            }
            if (PlayerPrefs.HasKey("Stage3Cleared"))
            {
                GameObject stage = _selectStage.transform.Find("SelectStageWindow/Stage4").gameObject;
                stage.transform.Find("StageName").GetComponent<TMP_Text>().text = "��������4";
                stage.transform.Find("Stage4Enter").GetComponent<Button>().image.sprite = _stageEnterImage;
                _selectStage.transform.Find("SelectStageWindow/Stage3/StageRead").GetComponent<Image>().sprite = _storyReadImage;
            }
            if (PlayerPrefs.HasKey("Stage4Cleared"))
            {
                _selectStage.transform.Find("SelectStageWindow/Stage4/StageRead").GetComponent<Image>().sprite = _storyReadImage;
                _selectStage.transform.Find("SelectStageWindow/StageAllClear").gameObject.SetActive(true);
            }
        }

        // ���� ������ ������
        public void DoStartShootingGame()
        {
            _canvas.transform.Find("Game1").gameObject.SetActive(true);

            _shootingGameManager.SetInitial();
        }

        // ���� ������ ������
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
