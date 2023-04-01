using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SingletonPattern
{
    public class StoryManager : Singleton<StoryManager>
    {
        GameManager _gameManager;
        AudioManager _audioManager;

        public GameObject _story;
        public Image _WindowFadeOut;
        public Image _episodeStartWindowFadeOut;

        [HideInInspector] public GameObject _yuzu;
        [HideInInspector] public GameObject _aris;
        [HideInInspector] public GameObject _midori;
        [HideInInspector] public GameObject _momoi;

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
        public class EpisodeBackgroundImage
        {
            public Sprite[] backgroundArray;
        }

        // 이미지가 9개 있는 캐릭터의 경우, 9번에 더미 이미지를 집어넣어 10~19까지 사용할 수 있도록 설정
        [SerializeField] MomoiImage _momoiImage;
        [SerializeField] MidoriImage _midoriImage;
        [SerializeField] ArisImage _arisImage;
        [SerializeField] YuzuImage _yuzuImage;

        // ButtonImage 0: 흰색 배경, 1: 노란색 배경
        [SerializeField] ButtonImage _buttonImage;

        // 0: Background, 1: GameDevelopment, 2: TSCBackground, 3: RingOfRight, 4: Ruins, 5: GameCenter
        [SerializeField] EpisodeBackgroundImage _episodeBackgroundImage;

        [HideInInspector] public GameObject _characterName;
        [HideInInspector] public GameObject _departmentName;
        [HideInInspector] public GameObject _dialogText;
        [HideInInspector] public GameObject _selection1Btn;
        [HideInInspector] public GameObject _selection1_1Btn;
        [HideInInspector] public GameObject _selection1_2Btn;
        [HideInInspector] public TMP_Text _windowText;
        [HideInInspector] public TMP_Text _gameOverText;
        [HideInInspector] public TMP_Text _gameOver2Text;

        private Image _momoiCharacterImage;
        private Image _midoriCharacterImage;
        private Image _yuzuCharacterImage;
        private Image _arisCharacterImage;
        private Image _momoiHaloImage;
        private Image _midoriHaloImage;
        private Image _yuzuHaloImage;
        private Image _arisHaloImage;

        private List<string> _characterNameList = new List<string>();    // 캐릭터 이름 목록
        private List<string> _departmentNameList = new List<string>();   // 소속 이름 목록
        private List<string> _dialogList = new List<string>();           // 대화 내용 목록
        private List<float> _textSpeedList = new List<float>();          // 대화 내용 목록

        private int _storyNum = 0;                   // 스토리가 어디까지 진행됐는지 저장하는 번호

        private Coroutine _coroutineStoryProgress;   // 대사를 한글자씩 출력하는 코루틴, 화면을 터치하면 중지시키고 한 번에 표시하기 위해 사용
        private Coroutine _autoStoryProgress;        // 자동으로 스토리를 진행시켜주는 코루틴
        private Coroutine _coroutineWindowFadeOut;   // 화면이 검게 Fade Out되는 코루틴, 화면을 다시 Fade In 할 때 충돌이 생기지 않도록 저장하여 중단시킴

        private bool _canProgress = false;           // 현재 스토리를 진행할 수 있는지 여부
        private bool _isStoryProgressing = false;    // 스토리에서 현재 대사가 나오고 있는지 확인
        private bool _isAutoStoryProgress;           // 현재 자동으로 스토리가 진행 중인지 여부를 판단
        private bool _isDialogOff = false;           // 대화창이 꺼져있는 상태인지 확인
        private bool _isSelectionOn = false;         // 선택지가 켜졌는지 확인
        private bool _isPressedButton = false;       // 버튼이 눌렀는지 확인

        private int _autoStoryNum = 0;

        public override void Awake()
        {
            // GameObject 객체 연결
            if (_gameManager == null) _gameManager = GameManager.Instance;
            if (_audioManager == null) _audioManager = AudioManager.Instance;
            
            _momoi = _story.transform.Find("Episode/Character/MomoiParent/Momoi").gameObject;
            _midori = _story.transform.Find("Episode/Character/MidoriParent/Midori").gameObject;
            _yuzu = _story.transform.Find("Episode/Character/YuzuParent/Yuzu").gameObject;
            _aris = _story.transform.Find("Episode/Character/ArisParent/Aris").gameObject;

            _momoiCharacterImage = _momoi.transform.Find("CharacterImage").GetComponent<Image>();
            _midoriCharacterImage = _midori.transform.Find("CharacterImage").GetComponent<Image>();
            _yuzuCharacterImage = _yuzu.transform.Find("CharacterImage").GetComponent<Image>();
            _arisCharacterImage = _aris.transform.Find("CharacterImage").GetComponent<Image>();

            _momoiHaloImage = _momoi.transform.Find("HaloParent/Halo").GetComponent<Image>();
            _midoriHaloImage = _midori.transform.Find("HaloParent/Halo").GetComponent<Image>();
            _yuzuHaloImage = _yuzu.transform.Find("HaloParent/Halo").GetComponent<Image>();
            _arisHaloImage = _aris.transform.Find("HaloParent/Halo").GetComponent<Image>();

            _characterName = _story.transform.Find("Episode/Dialog/CharacterName").gameObject;
            _departmentName = _story.transform.Find("Episode/Dialog/DepartmentName").gameObject;
            _dialogText = _story.transform.Find("Episode/Dialog/DialogText").gameObject;

            _selection1Btn = _story.transform.Find("Episode/UI/Selection1Btn").gameObject;
            _selection1_1Btn = _story.transform.Find("Episode/UI/Selection1-1Btn").gameObject;
            _selection1_2Btn = _story.transform.Find("Episode/UI/Selection1-2Btn").gameObject;

            _windowText = _story.transform.Find("Episode/WindowText").GetComponent<TMP_Text>();
            _gameOverText = _story.transform.Find("Episode/WindowText_GameOver").GetComponent<TMP_Text>();
            _gameOver2Text = _story.transform.Find("Episode/WindowText_GameOver2").GetComponent<TMP_Text>();
        }

        // 해당 캐릭터의 이펙트 애니메이션을 재생하는 함수
        private void PlayCharacterEffectAnimation(GameObject characterName, string animationName)
        {
            characterName.transform.Find($"{animationName}Parent/{animationName}").GetComponent<Animator>().Play(animationName);
            _audioManager.PlaySFX(animationName);
        }

        // 선택지로 갈린 스토리 번호 이동 체크하고 이동하는 함수
        private void DoStoryJump()
        {
            if (_storyNum == 23)
            {
                _storyNum = 28;
            }
        }

        // 대화창을 끄고 키는 함수
        private void SetDialogOn(bool isDialogOff)
        {
            if (!isDialogOff)
            {
                _canProgress = false;
                _isDialogOff = true;
                _dialogText.transform.parent.gameObject.SetActive(false);
            }
            else
            {
                _canProgress = true;
                _isDialogOff = false;
                _dialogText.transform.parent.gameObject.SetActive(true);
            }
        }

        // 스토리 진행 중 화면을 클릭했을 때 작동하는 함수
        public void EpisodeBackgroundClick()
        {
            if (_story.transform.Find("Episode/UI/Menu").gameObject.activeSelf) _story.transform.Find("Episode/UI/Menu").gameObject.SetActive(false);
            if (_story.transform.Find("Episode/UI").gameObject.activeSelf == false)
            {
                _story.transform.Find("Episode/UI").gameObject.SetActive(true);
                if (!_isDialogOff) _story.transform.Find("Episode/Dialog").gameObject.SetActive(true);
            }
            else
            {
                DoStoryProgress();
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
                _selection1Btn.SetActive(false);
            }
            else if (num == 1)
            {
                _selection1_1Btn.SetActive(false);
                _selection1_2Btn.SetActive(false);

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
                _selection1_1Btn.SetActive(false);
                _selection1_2Btn.SetActive(false);

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

            _canProgress = true;
            DoStoryProgress();
        }

        // 1개짜리 선택지를 보여주는 함수
        private void ShowSelection(string selectionDialog)
        {
            _isPressedButton = false;
            _isSelectionOn = true;
            _selection1Btn.SetActive(true);
            _selection1Btn.transform.Find("Text").GetComponent<TMP_Text>().text = selectionDialog;
            _selection1Btn.GetComponent<Animator>().Play("ButtonPopup");
        }

        // 2개짜리 선택지를 보여주는 함수
        private void ShowSelection(string selectionDialog1, string selectionDialog2)
        {
            _isPressedButton = false;
            _isSelectionOn = true;
            _selection1_1Btn.SetActive(true);
            _selection1_2Btn.SetActive(true);
            _selection1_1Btn.transform.Find("Text").GetComponent<TMP_Text>().text = selectionDialog1;
            _selection1_2Btn.transform.Find("Text").GetComponent<TMP_Text>().text = selectionDialog2;
            _selection1_1Btn.GetComponent<Animator>().Play("ButtonPopup");
            _selection1_2Btn.GetComponent<Animator>().Play("ButtonPopup");
        }

        public IEnumerator ShowSadMidori()
        {
            if (_midori.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Shiver")) yield break;

            SetCharacterImage(_midori, 14);
            _midori.GetComponent<Animator>().Play("Shiver");

            yield return new WaitForSeconds(2f);

            SetCharacterImage(_midori, 10);
        }

        // 학생들의 이미지를 변경해주는 함수
        public IEnumerator ShowHappyMidori()
        {
            if (_midori.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Jumping")) yield break;

            SetCharacterImage(_midori, 13);
            _midori.GetComponent<Animator>().Play("Jumping");

            yield return new WaitForSeconds(1f);

            SetCharacterImage(_midori, 10);
        }

        public void SetCharacterImage(GameObject characterName, int num)
        {
            if (characterName == _momoi)
            {
                _momoiCharacterImage.sprite = _momoiImage.imageArray[num];
                // 모모이의 표정이 0번일 때는 눈을 깜빡이는 모션이 있음
                if (num == 0)
                {
                    _momoiCharacterImage.GetComponent<Animator>().enabled = true;
                }
                else
                {
                    _momoiCharacterImage.GetComponent<Animator>().enabled = false;
                }
            }
            else if (characterName == _midori)
            {
                _midoriCharacterImage.sprite = _midoriImage.imageArray[num];
            }
            else if (characterName == _aris)
            {
                _arisCharacterImage.sprite = _arisImage.imageArray[num];
            }
            else if (characterName == _yuzu)
            {
                _yuzuCharacterImage.sprite = _yuzuImage.imageArray[num];
            }
        }

        public bool CheckAutoProgress()
        {
            if (_isAutoStoryProgress)
            {
                StopCoroutine(_autoStoryProgress);
                _isAutoStoryProgress = false;
                _story.transform.Find("Episode/UI/AutoBtn").GetComponent<Image>().sprite = _buttonImage.buttonArray[0];
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
            _story.transform.Find("Episode/UI/AutoBtn").GetComponent<Image>().sprite = _buttonImage.buttonArray[1];
        }

        // 자동으로 스토리가 진행되도록 해주는 코루틴
        public IEnumerator DoAutoProgressCoroutine()
        {
            while (true)
            {
                // 컷씬이 진행중일 때는 대기
                while (!_canProgress)
                {
                    yield return new WaitForSeconds(1);
                }

                // 대화가 출력되고 있지 않을 때만 진행
                if (!_isStoryProgressing)
                {
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
                }
                else
                {
                    yield return new WaitForSeconds(1); // 1초마다 확인
                }
            }
        }

        // GameManager에서 스토리 시작 번호를 지정할 때 사용하는 함수
        public void SetStoryNumber(int num)
        {
            _storyNum = num;
        }

        // 외부에서 스토리를 진행할 수 있는 상태인지 여부를 변경하는 함수
        public void SetIsCanProgress(bool isCanProgress)
        {
            this._canProgress = isCanProgress;
        }

        // 캐릭터 이름, 부서 이름, 대화 내용, 텍스트 속도를 간편하게 쓰고 저장하기 위한 함수
        public void AppendDialog(string characterNameText, string departmentNameText, string dialogText, float textSpeed)
        {
            _characterNameList.Add(characterNameText);
            _departmentNameList.Add(departmentNameText);
            _dialogList.Add(dialogText);
            _textSpeedList.Add(textSpeed); // 텍스트 속도 : (textSpeed) 배
        }

        private IEnumerator DoFadeEpisodeWindowFadeOut()
        {
            _WindowFadeOut.gameObject.SetActive(true);
            DOTween.Sequence().Append(_WindowFadeOut.DOFade(1, 2f)).Append(_WindowFadeOut.DOFade(0, 2f));

            yield return new WaitForSeconds(4);

            _WindowFadeOut.gameObject.SetActive(false);
        }

        // 스토리 진행시 텍스트 외에 캐릭터 움직임, 효과음 등을 관리하는 파트
        private IEnumerator DoStoryAction(int num)
        {
            if (!_isStoryProgressing)
            {
                _canProgress = false;
                if (num == 0)
                {
                    _audioManager.PlaySFX("Silence");
                }
                else if (num == 1)
                {
                    //audioManager.PlaySFX("Silence");
                }
                else if (num == 2)
                {
                    _dialogText.GetComponent<TMP_Text>().fontSize = 60;
                    _audioManager.PlaySFX("Start");
                }
                else if (num == 3)
                {
                    _momoi.SetActive(true);
                    _momoi.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);

                    _story.transform.Find("Episode/EpisodeBackground").GetComponent<Image>().sprite = _episodeBackgroundImage.backgroundArray[1];
                    _story.transform.Find("Episode/EpisodeBackground").GetComponent<Image>().DOFade(1, 2);
                    _momoiCharacterImage.DOFade(1, 2);
                    _momoiHaloImage.DOFade(1, 2);

                    _audioManager.PlayBGM("PixelTime");
                    yield return new WaitForSeconds(1.5f);
                    _dialogText.GetComponent<TMP_Text>().fontSize = 42.5f;
                    PlayCharacterEffectAnimation(_momoi, "Talking");
                    _momoi.GetComponent<Animator>().Play("Jumping");
                }
                else if (num == 4)
                {
                    _midori.SetActive(true);
                    _midori.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(600, 0); // midori right side : 625
                    _midoriCharacterImage.color = Color.white;
                    _midoriHaloImage.color = Color.white;

                    _momoi.transform.parent.GetComponent<RectTransform>().DOLocalMoveX(-700, 0.5f); // momoi left side : -700
                }
                else if (num == 5)
                {
                    _midori.gameObject.SetActive(false);
                    _momoi.gameObject.SetActive(false);
                    _aris.gameObject.SetActive(true);
                    _yuzu.gameObject.SetActive(true);

                    SetCharacterImage(_yuzu, 8);
                    SetCharacterImage(_aris, 0);
                    _arisCharacterImage.color = Color.white;
                    _arisHaloImage.color = Color.white;
                    _aris.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(600, 0); // aris right side : 580
                    _yuzuCharacterImage.color = Color.white;
                    _yuzuHaloImage.color = Color.white;
                    _yuzu.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(-700, 0); // left side : -700
                    _yuzu.GetComponent<Animator>().Play("Shiver");
                    PlayCharacterEffectAnimation(_yuzu, "Mess");
                }
                else if (num == 6)
                {
                    SetCharacterImage(_aris, 7);
                    PlayCharacterEffectAnimation(_aris, "Shine");
                }
                else if (num == 7)
                {
                    SetCharacterImage(_momoi, 3);
                    _momoi.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
                    _momoi.gameObject.SetActive(true);
                    _aris.gameObject.SetActive(false);
                    _yuzu.gameObject.SetActive(false);

                    _momoi.GetComponent<Animator>().Play("Jumping");
                    PlayCharacterEffectAnimation(_momoi, "Talking");
                }
                else if (num == 8) // 선택지
                {
                    if (PlayerPrefs.GetString("Language").Equals("Korean")) ShowSelection("( 자리에 앉는다. )");
                    else if (PlayerPrefs.GetString("Language").Equals("Japanese")) ShowSelection("（席に座る）");
                }
                else if (num == 9) // Window Fade Out
                {
                    StartCoroutine(DoFadeEpisodeWindowFadeOut());
                    _audioManager.PlaySFX("TSCStart");

                    StartCoroutine(_audioManager.FadeOutMusic());
                    yield return new WaitForSeconds(2f);
                    SetDialogOn(false);
                    _momoi.transform.gameObject.SetActive(false);
                    _story.transform.Find("Episode/EpisodeBackground").GetComponent<Image>().sprite = _episodeBackgroundImage.backgroundArray[2];

                    yield return new WaitForSeconds(2f);

                    SetDialogOn(true);
                }
                else if (num == 10)
                {
                    SetDialogOn(false);
                    yield return new WaitForSeconds(0.5f);
                    _audioManager.PlayBGM("Theme101");

                    if (PlayerPrefs.GetString("Language").Equals("Korean")) 
                        StartCoroutine(AppendTextOneByOne(_windowText, "< 코스모스 세기 2354년, 인류는 업화의 불길에 휩싸였다. >\n\n", 1));
                    else if (PlayerPrefs.GetString("Language").Equals("Japanese")) 
                        StartCoroutine(AppendTextOneByOne(_windowText, "コスモス世紀2354年、人類は劫火の炎につつまれた……\n\n", 1));

                    yield return new WaitForSeconds(2.5f);
                    if (PlayerPrefs.GetString("Language").Equals("Korean")) ShowSelection("“……동화적 색채?”");
                    else if (PlayerPrefs.GetString("Language").Equals("Japanese")) ShowSelection("“……童話テイストで、色彩豊か？”");
                }
                else if (num == 11)
                {
                    SetDialogOn(true);
                }
                else if (num == 12)
                {
                    if (PlayerPrefs.GetString("Language").Equals("Korean")) ShowSelection("“……。”", "( 버튼을 입력한다. )");
                    else if (PlayerPrefs.GetString("Language").Equals("Japanese")) ShowSelection("“…….”", "（ボタンを押す）");
                }
                else if (num == 13)
                {
                    SetDialogOn(false);
                    if (PlayerPrefs.GetString("Language").Equals("Korean"))
                        StartCoroutine(AppendTextOneByOne(_windowText, "< 튜토리얼을 시작합니다. >\n\n", 1));
                    else if (PlayerPrefs.GetString("Language").Equals("Japanese"))
                        StartCoroutine(AppendTextOneByOne(_windowText, "チュートリアルを開始します。\n\n", 1));
                    
                    yield return new WaitForSeconds(2);

                    if (PlayerPrefs.GetString("Language").Equals("Korean"))
                        StartCoroutine(AppendTextOneByOne(_windowText, "< 먼저 B 키를 눌러, 눈앞의 무기를 장착해보세요. >\n\n", 1));
                    else if (PlayerPrefs.GetString("Language").Equals("Japanese"))
                        StartCoroutine(AppendTextOneByOne(_windowText, "まずはBボタンを押して、目の前の武器を装着してみてください。\n\n", 1));

                    yield return new WaitForSeconds(2.5f);

                    if (PlayerPrefs.GetString("Language").Equals("Korean")) ShowSelection("( A 키를 입력한다. )", "( B 키를 입력한다. )");
                    else if (PlayerPrefs.GetString("Language").Equals("Japanese")) ShowSelection("（Aボタンを押す）", "（Bボタンを押す）");
                }
                // B 버튼 루트 시작
                else if (num == 14)
                {
                    _audioManager.StopBGM();
                    _audioManager.PlaySFX("SelectButton");
                    yield return new WaitForSeconds(1);
                    _audioManager.PlaySFX("8bitBomb");
                    SetDialogOn(true);
                }
                else if (num == 15)
                {
                    if (PlayerPrefs.GetString("Language").Equals("Korean")) ShowSelection("“???”");
                    else if (PlayerPrefs.GetString("Language").Equals("Japanese")) ShowSelection("“？？？”");
                }
                else if (num == 16)
                {
                    SetDialogOn(false);
                    // 게임 오버
                    _windowText.text = "";
                    _audioManager.PlaySFX("GameOver");
                    _story.transform.Find("Episode/WindowText_GameOver").GetComponent<TMP_Text>().DOFade(1, 2f);
                    yield return new WaitForSeconds(4);
                    if (PlayerPrefs.GetString("Language").Equals("Korean")) ShowSelection("“?!”");
                    else if (PlayerPrefs.GetString("Language").Equals("Japanese")) ShowSelection("“？！”");
                }
                else if (num == 17)
                {
                    SetDialogOn(true);
                    _audioManager.PlaySFX("Laughing");
                }
                else if (num == 18) // WindowFadeOut
                {
                    SetCharacterImage(_momoi, 3);

                    StartCoroutine(DoFadeEpisodeWindowFadeOut());
                    yield return new WaitForSeconds(2f);
                    _momoi.transform.gameObject.SetActive(true);
                    _windowText.text = "";

                    //GameOver 글씨 투명하게 변경
                    ColorUtility.TryParseHtmlString("#FF656B00", out Color color);
                    _story.transform.Find("Episode/WindowText_GameOver").GetComponent<TMP_Text>().color = color;

                    _story.transform.Find("Episode/EpisodeBackground").GetComponent<Image>().sprite = _episodeBackgroundImage.backgroundArray[1];
                    yield return new WaitForSeconds(2f);


                    _momoi.GetComponent<Animator>().Play("Jumping");
                    PlayCharacterEffectAnimation(_momoi, "Laughing");
                    _audioManager.PlayBGM("MischievousStep");
                }
                else if (num == 19)
                {
                    if (PlayerPrefs.GetString("Language").Equals("Korean")) ShowSelection("“…….”");
                    else if (PlayerPrefs.GetString("Language").Equals("Japanese")) ShowSelection("“……。”");
                }
                else if (num == 20)
                {
                    _aris.SetActive(true);
                    SetCharacterImage(_aris, 9);

                    _momoi.transform.parent.GetComponent<RectTransform>().DOLocalMoveX(-700, 0.5f); // momoi left side : -700
                    PlayCharacterEffectAnimation(_aris, "Sweat");
                }
                else if (num == 21)
                {
                    if (PlayerPrefs.GetString("Language").Equals("Korean")) ShowSelection("( 다시 시도한다. )");
                    else if (PlayerPrefs.GetString("Language").Equals("Japanese")) ShowSelection("（もう一回やる）");
                }
                else if (num == 22)
                {
                    StartCoroutine(DoFadeEpisodeWindowFadeOut());
                    StartCoroutine(_audioManager.FadeOutMusic());
                    _audioManager.PlaySFX("TSCStart");
                    yield return new WaitForSeconds(2f);
                    SetDialogOn(false);
                    _momoi.transform.gameObject.SetActive(false);
                    _aris.transform.gameObject.SetActive(false);
                    _story.transform.Find("Episode/EpisodeBackground").GetComponent<Image>().sprite = _episodeBackgroundImage.backgroundArray[2];
                    yield return new WaitForSeconds(2f);
                    if (PlayerPrefs.GetString("Language").Equals("Korean"))
                        StartCoroutine(AppendTextOneByOne(_windowText, "< 무기 장착에 성공했습니다. >\n\n", 1));
                    else if (PlayerPrefs.GetString("Language").Equals("Japanese"))
                        StartCoroutine(AppendTextOneByOne(_windowText, "武器を装着しました。\n\n", 1));
                    _audioManager.PlaySFX("Message");
                    yield return new WaitForSeconds(2f);
                    SetDialogOn(true);
                }
                // B 버튼 루트 종료

                // A버튼 루트
                else if (num == 23)
                {
                    _windowText.text = "";
                    _audioManager.StopBGM();
                    if (PlayerPrefs.GetString("Language").Equals("Korean"))
                        StartCoroutine(AppendTextOneByOne(_windowText, "< 무기 장착에 성공했습니다. >\n\n", 1));
                    else if (PlayerPrefs.GetString("Language").Equals("Japanese"))
                        StartCoroutine(AppendTextOneByOne(_windowText, "武器を装着しました。\n\n", 1));
                    _audioManager.PlaySFX("Message");
                    yield return new WaitForSeconds(2f);
                    SetDialogOn(true);
                    _audioManager.PlaySFX("Talking");
                }
                else if (num == 24)
                {
                    _audioManager.PlaySFX("Sweat");
                }
                else if (num == 25)
                {
                    _audioManager.PlaySFX("Shine");
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
                    _audioManager.PlayBGM("MechanicalJungle");
                    _windowText.text += "<#FF656B>";
                    if (PlayerPrefs.GetString("Language").Equals("Korean"))
                        StartCoroutine(AppendTextOneByOne(_windowText, "< 전투 발생! 전투 발생! >\n\n", 1));
                    else if (PlayerPrefs.GetString("Language").Equals("Japanese"))
                        StartCoroutine(AppendTextOneByOne(_windowText, "エンカウントが発生しました！\n\n", 1));

                    yield return new WaitForSeconds(2);

                    _windowText.text += "</color>";
                    if (PlayerPrefs.GetString("Language").Equals("Korean"))
                        StartCoroutine(AppendTextOneByOne(_windowText, "< 야생의 푸니젤리가 나타났다! >\n\n", 1));
                    else if (PlayerPrefs.GetString("Language").Equals("Japanese"))
                        StartCoroutine(AppendTextOneByOne(_windowText, "野生のプニプニが現れた！\n\n", 1));
                    yield return new WaitForSeconds(2);
                    SetDialogOn(true);
                }
                else if (num == 29)
                {
                    if (PlayerPrefs.GetString("Language").Equals("Korean")) ShowSelection("( A버튼 입력, < 비검 츠바메가에시, 한 번에 적을 2회 공격한다. > )", "( 푸니젤리에게 조심스럽게 접근한다. )");
                    else if (PlayerPrefs.GetString("Language").Equals("Japanese")) ShowSelection("（Aボタン「秘剣つばめが返し：敵に対して２回攻撃をする」）", "（プニプニに用心深く接近する）");
                }
                // A버튼을 누른다 루트 시작
                else if (num == 30)
                {
                    SetDialogOn(false);
                    _audioManager.StopBGM();
                    if (PlayerPrefs.GetString("Language").Equals("Korean"))
                        StartCoroutine(AppendTextOneByOne(_windowText, "< 탕!! >\n\n", 1));
                    else if (PlayerPrefs.GetString("Language").Equals("Japanese"))
                        StartCoroutine(AppendTextOneByOne(_windowText, "ッダーン！\n\n", 1));
                    _audioManager.PlaySFX("8bitBang");

                    yield return new WaitForSeconds(2);

                    if (PlayerPrefs.GetString("Language").Equals("Korean"))
                        StartCoroutine(AppendTextOneByOne(_windowText, "< 명중, 당신의 캐릭터가 즉사했다. >\n\n", 1));
                    else if (PlayerPrefs.GetString("Language").Equals("Japanese"))
                        StartCoroutine(AppendTextOneByOne(_windowText, "攻撃が命中、即死しました\n\n", 1));
                    _audioManager.PlaySFX("KnockDown");

                    yield return new WaitForSeconds(2);

                    // 게임 오버
                    _windowText.text = "";
                    _audioManager.PlaySFX("GameOver");
                    _story.transform.Find("Episode/WindowText_GameOver").GetComponent<TMP_Text>().DOFade(1, 2);
                    yield return new WaitForSeconds(4);

                    if (PlayerPrefs.GetString("Language").Equals("Korean")) ShowSelection("“??!!”");
                    else if (PlayerPrefs.GetString("Language").Equals("Japanese")) ShowSelection("“？？！！”");
                }
                else if (num == 31)
                {
                    _story.transform.Find("Episode/WindowText_GameOver2").GetComponent<TMP_Text>().DOFade(1, 2);
                    _audioManager.PlayBGM("FadeOut");
                    _audioManager.PlaySFX("RobotTalk");
                    yield return new WaitForSeconds(4);

                    SetCharacterImage(_momoi, 7);

                    _momoi.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(-700, 0);
                    StartCoroutine(DoFadeEpisodeWindowFadeOut());
                    yield return new WaitForSeconds(2);
                    _momoi.transform.gameObject.SetActive(true);
                    _windowText.text = "";

                    //GameOver 글씨 투명하게 변경
                    ColorUtility.TryParseHtmlString("#FF656B00", out Color color);
                    _story.transform.Find("Episode/WindowText_GameOver").GetComponent<TMP_Text>().color = color;
                    ColorUtility.TryParseHtmlString("#5DC48100", out color);
                    _story.transform.Find("Episode/WindowText_GameOver2").GetComponent<TMP_Text>().color = color;
                    _story.transform.Find("Episode/EpisodeBackground").GetComponent<Image>().sprite = _episodeBackgroundImage.backgroundArray[1];
                    yield return new WaitForSeconds(2);

                    PlayCharacterEffectAnimation(_momoi, "Silence");
                    SetDialogOn(true);
                }
                else if (num == 32)
                {
                    _midori.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(600, 0);
                    _midori.transform.gameObject.SetActive(true);
                    SetCharacterImage(_midori, 7);

                    PlayCharacterEffectAnimation(_midori, "Sweat");
                }
                else if (num == 33)
                {
                    _momoi.SetActive(false);
                    _midori.SetActive(false);
                    _aris.SetActive(true);
                    SetCharacterImage(_aris, 10);

                    _aris.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
                    _aris.GetComponent<Animator>().Play("Jumping");
                    PlayCharacterEffectAnimation(_aris, "Mess");
                }
                else if (num == 34)
                {
                    if (PlayerPrefs.GetString("Language").Equals("Korean")) ShowSelection("( 다시 시도한다. )");
                    else if (PlayerPrefs.GetString("Language").Equals("Japanese")) ShowSelection("（もう一回やる）");
                }
                else if (num == 35) // Window Fade Out(게임 부분으로)
                {
                    StartCoroutine(DoFadeEpisodeWindowFadeOut());
                    StartCoroutine(_audioManager.FadeOutMusic());
                    _audioManager.PlaySFX("TSCStart");
                    yield return new WaitForSeconds(2);
                    SetDialogOn(false);
                    _momoi.transform.gameObject.SetActive(false);
                    _aris.transform.gameObject.SetActive(false);
                    _story.transform.Find("Episode/EpisodeBackground").GetComponent<Image>().sprite = _episodeBackgroundImage.backgroundArray[2];

                    yield return new WaitForSeconds(2);

                    if (PlayerPrefs.GetString("Language").Equals("Korean"))
                        StartCoroutine(AppendTextOneByOne(_windowText, "< 무기 장착에 성공했습니다. >\n\n", 1));
                    else if (PlayerPrefs.GetString("Language").Equals("Japanese"))
                        StartCoroutine(AppendTextOneByOne(_windowText, "武器を装着しました。\n\n", 1));
                    _audioManager.PlaySFX("Message");

                    yield return new WaitForSeconds(2f);

                    _audioManager.PlayBGM("MechanicalJungle");
                    _windowText.text += "<#FF656B>";
                    if (PlayerPrefs.GetString("Language").Equals("Korean"))
                        StartCoroutine(AppendTextOneByOne(_windowText, "< 전투 발생! 전투 발생! >\n\n", 1));
                    else if (PlayerPrefs.GetString("Language").Equals("Japanese"))
                        StartCoroutine(AppendTextOneByOne(_windowText, "エンカウントが発生しました！\n\n", 1));

                    yield return new WaitForSeconds(2);

                    _windowText.text += "</color>";
                    if (PlayerPrefs.GetString("Language").Equals("Korean"))
                        StartCoroutine(AppendTextOneByOne(_windowText, "< 야생의 푸니젤리가 나타났다! >\n\n", 1));
                    else if (PlayerPrefs.GetString("Language").Equals("Japanese"))
                        StartCoroutine(AppendTextOneByOne(_windowText, "野生のプニプニが現れた！\n\n", 1));

                    yield return new WaitForSeconds(2);

                    if (PlayerPrefs.GetString("Language").Equals("Korean")) ShowSelection("( 푸니젤리에게 조심스럽게 접근한다. )");
                    else if (PlayerPrefs.GetString("Language").Equals("Japanese")) ShowSelection("（プニプニに用心深く接近する）");
                }
                // 푸니젤리에게 조심스럽게 접근한다. (공통 루트)
                else if (num == 36)
                {
                    SetDialogOn(false);
                    yield return new WaitForSeconds(2);

                    if (PlayerPrefs.GetString("Language").Equals("Korean"))
                        StartCoroutine(AppendTextOneByOne(_windowText, "< 오류 발생! 오류 발생! >\n\n", 1));
                    else if (PlayerPrefs.GetString("Language").Equals("Japanese"))
                        StartCoroutine(AppendTextOneByOne(_windowText, "エラー！エラー！\n\n", 1));

                    _audioManager.StopBGM();
                    _audioManager.PlaySFX("RobotTalk");
                    yield return new WaitForSeconds(2);

                    if (PlayerPrefs.GetString("Language").Equals("Korean"))
                        StartCoroutine(AppendTextOneByOne(_windowText, "< 심각한 오류 발생으로 시스템을 긴급 정지합니다. >\n\n", 1.5f));
                    else if (PlayerPrefs.GetString("Language").Equals("Japanese"))
                        StartCoroutine(AppendTextOneByOne(_windowText, "深刻なエラーが発生してシステムを緊急停止します。\n\n", 1));

                    _audioManager.PlaySFX("RobotTalk");
                    yield return new WaitForSeconds(2);

                    if (PlayerPrefs.GetString("Language").Equals("Korean")) ShowSelection("“!?”");
                    else if (PlayerPrefs.GetString("Language").Equals("Japanese")) ShowSelection("“！？”");
                }
                else if (num == 37)
                {
                    SetDialogOn(true);
                    _audioManager.PlaySFX("Explode");
                }
                else if (num == 38)
                {
                    SetCharacterImage(_momoi, 4);
                    _momoi.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(-700, 0);

                    StartCoroutine(DoFadeEpisodeWindowFadeOut());
                    yield return new WaitForSeconds(2);
                    SetDialogOn(false);
                    _momoi.SetActive(true);
                    _windowText.text = "";

                    _story.transform.Find("Episode/EpisodeBackground").GetComponent<Image>().sprite = _episodeBackgroundImage.backgroundArray[1];
                    yield return new WaitForSeconds(2);
                    // 흙먼지 연출?

                    SetDialogOn(true);
                    _momoi.GetComponent<Animator>().Play("Shiver");
                    _momoi.transform.Find("MessParent/Mess").GetComponent<Animator>().Play("Mess");
                    _audioManager.PlaySFX("Mess");

                    _audioManager.PlayBGM("MischievousStep");
                }
                else if (num == 39)
                {
                    SetCharacterImage(_midori, 7);
                    _midori.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(600, 0);

                    _midori.SetActive(true);
                    _midori.GetComponent<Animator>().Play("Jumping");
                    PlayCharacterEffectAnimation(_midori, "Talking");
                }
                else if (num == 40)
                {
                    SetCharacterImage(_yuzu, 8);
                    SetCharacterImage(_aris, 4);
                    _momoi.SetActive(false);
                    _midori.SetActive(false);
                    _aris.SetActive(true);
                    _yuzu.SetActive(true);
                    _yuzu.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(-700, 0);
                    _aris.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(600, 0);

                    _yuzu.GetComponent<Animator>().Play("Shiver");
                    PlayCharacterEffectAnimation(_yuzu, "Mess");
                }
                else if (num == 41)
                {
                    SetCharacterImage(_aris, 4);
                    _aris.GetComponent<Animator>().Play("Shiver");
                    PlayCharacterEffectAnimation(_aris, "Mess");
                }
                else if (num == 42)
                {
                    if (PlayerPrefs.GetString("Language").Equals("Korean")) ShowSelection("( 정신이 흐려진다…… )");
                    else if (PlayerPrefs.GetString("Language").Equals("Japanese")) ShowSelection("“視界がぼやける……”");
                }
                else if (num == 43)
                {
                    SetDialogOn(false);
                    _WindowFadeOut.gameObject.SetActive(true);
                    // 화면 깜빡이는 연출, DOTween을 사용하면 중간중간 끊기는 느낌이 들어서 따로 구현한 함수 사용

                    _coroutineWindowFadeOut = StartCoroutine(_gameManager.FadeInImage(0.5f, _WindowFadeOut));
                    yield return new WaitForSeconds(0.5f);

                    StopCoroutine(_coroutineWindowFadeOut);
                    _coroutineWindowFadeOut = StartCoroutine(_gameManager.FadeOutImage(0.3f, _WindowFadeOut));
                    yield return new WaitForSeconds(0.5f);

                    StopCoroutine(_coroutineWindowFadeOut);
                    _coroutineWindowFadeOut = StartCoroutine(_gameManager.FadeOutImage(0.01f, _WindowFadeOut));
                    yield return new WaitForSeconds(0.2f);

                    StopCoroutine(_coroutineWindowFadeOut);
                    _coroutineWindowFadeOut = StartCoroutine(_gameManager.FadeInImage(0.5f, _WindowFadeOut));
                    yield return new WaitForSeconds(0.5f);

                    StopCoroutine(_coroutineWindowFadeOut);
                    _coroutineWindowFadeOut = StartCoroutine(_gameManager.FadeOutImage(0.3f, _WindowFadeOut));
                    yield return new WaitForSeconds(0.5f);

                    StopCoroutine(_coroutineWindowFadeOut);
                    _coroutineWindowFadeOut = StartCoroutine(_gameManager.FadeOutImage(0.01f, _WindowFadeOut));
                    yield return new WaitForSeconds(0.2f);

                    StopCoroutine(_coroutineWindowFadeOut);
                    _coroutineWindowFadeOut = StartCoroutine(_gameManager.FadeInImage(0.5f, _WindowFadeOut));
                    yield return new WaitForSeconds(2);


                    // 대화창을 가리는 WindowFadeOut 감추고, 배경을 검은색으로 변경
                    _story.transform.Find("Episode/EpisodeBackground").GetComponent<Image>().sprite = _episodeBackgroundImage.backgroundArray[0];
                    _story.transform.Find("Episode/EpisodeBackground").GetComponent<Image>().color = Color.black;
                    StartCoroutine(_gameManager.FadeOutImage(4f, _WindowFadeOut));
                    _yuzu.SetActive(false);
                    _aris.SetActive(false);
                    _audioManager.PlaySFX("KnockDown"); // 쓰러지는 소리
                    _audioManager.StopBGM();
                    yield return new WaitForSeconds(1);

                    _WindowFadeOut.gameObject.SetActive(false);
                    SetDialogOn(true);
                    _dialogText.GetComponent<TMP_Text>().fontSize = 60;
                    _audioManager.PlaySFX("Confuse");
                }
                else if (num == 44) // 메인화면으로 이동
                {
                    CheckAutoProgress();

                    _gameManager.DoFinishStory(0);

                    yield break;

                }
                // 1장 종료

                // 2장 시작
                else if (num == 45)
                {
                    SetDialogOn(false);

                    _audioManager.PlayBGM("MoroseDreamer");

                    _story.transform.Find("Episode/EpisodeMovingBackground").GetComponent<Image>().sprite = _episodeBackgroundImage.backgroundArray[3];
                    _story.transform.Find("Episode/EpisodeMovingBackground").GetComponent<Image>().DOFade(1, 2);
                    yield return new WaitForSeconds(2);
                    _story.transform.Find("Episode/EpisodeMovingBackground").DOLocalMoveY(-100, 3).SetEase(Ease.Linear);
                    yield return new WaitForSeconds(4);

                    _canProgress = true;
                    _storyNum++;
                    DoStoryProgress();
                    yield break;
                }
                else if (num == 46)
                {
                    SetDialogOn(true);

                    _midori.SetActive(true);
                    SetCharacterImage(_midori, 10);
                    _midoriCharacterImage.color = Color.black;
                    _midoriCharacterImage.DOFade(1, 2);
                    _midori.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(-500, 0);
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
                    _story.transform.Find("Episode/EpisodeMovingBackground").GetComponent<Image>().DOFade(0, 2);
                    yield return new WaitForSeconds(2);

                    _midori.SetActive(false);
                }
                else if (num == 54)
                {
                    ShowSelection("( 눈을 뜬다. )");
                }
                else if (num == 55)
                {

                    SetDialogOn(false);
                    _story.transform.Find("Episode/EpisodeBackground").GetComponent<Image>().sprite = _episodeBackgroundImage.backgroundArray[4];
                    _story.transform.Find("Episode/EpisodeBackground").GetComponent<Image>().DOFade(1, 3);

                    _midori.SetActive(true);
                    _midori.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
                    _midoriCharacterImage.color = new Color(1, 1, 1, 0);
                    _midoriCharacterImage.DOFade(1, 3);
                    StartCoroutine(_audioManager.FadeOutMusic());

                    yield return new WaitForSeconds(3);

                    SetDialogOn(true);
                    SetCharacterImage(_midori, 12);
                    PlayCharacterEffectAnimation(_midori, "Talking");
                    _midori.GetComponent<Animator>().Play("Jumping");

                    _audioManager.PlayBGM("Morning");
                }
                else if (num == 56)
                {
                    ShowSelection("( 미도리? )", "( ……아니, 헤일로가 없는 걸 보니 우리 학생이 아니군. )");
                }
                else if (num == 57)
                {
                    SetCharacterImage(_midori, 14);
                    PlayCharacterEffectAnimation(_midori, "Sweat");
                }
                else if (num == 58)
                {
                    SetCharacterImage(_midori, 18);
                }
                else if (num == 59)
                {
                    SetCharacterImage(_midori, 15);
                }
                else if (num == 62)
                {
                    ShowSelection("“……미증유의 위기 아니었어?”");
                }
                else if (num == 63)
                {
                    SetCharacterImage(_midori, 14);
                    PlayCharacterEffectAnimation(_midori, "Mess");
                    _midori.GetComponent<Animator>().Play("Shiver");
                }
                else if (num == 64)
                {
                    ShowSelection("“그럴 수도 있지.”");
                }
                else if (num == 65)
                {
                    SetCharacterImage(_midori, 18);
                    _midori.GetComponent<Animator>().Play("Jumping");
                }
                else if (num == 66)
                {
                    SetCharacterImage(_midori, 15);
                }
                else if (num == 67)
                {
                    ShowSelection("“하지만 나는 싸움을 잘 못하는 걸.”");
                }
                else if (num == 68)
                {
                    SetCharacterImage(_midori, 10);
                    PlayCharacterEffectAnimation(_midori, "Talking");
                    _midori.GetComponent<Animator>().Play("Jumping");
                }
                else if (num == 69)
                {
                    SetCharacterImage(_midori, 12);
                    PlayCharacterEffectAnimation(_midori, "Question");
                }
                else if (num == 70)
                {
                    SetCharacterImage(_midori, 13);
                    PlayCharacterEffectAnimation(_midori, "Shine");
                }
                else if (num == 71)
                {
                    SetCharacterImage(_midori, 17);
                    PlayCharacterEffectAnimation(_midori, "Silence");
                }
                else if (num == 72)
                {
                    SetCharacterImage(_midori, 10);
                }
                else if (num == 73)
                {
                    SetCharacterImage(_midori, 17);
                }
                else if (num == 74)
                {
                    ShowSelection("“내가 도움이 된다면야 얼마든지.”");
                }
                else if (num == 75)
                {
                    SetCharacterImage(_midori, 13);
                    PlayCharacterEffectAnimation(_midori, "Talking");
                    _midori.GetComponent<Animator>().Play("Jumping");
                }
                else if (num == 76)
                {
                    _canProgress = true;
                    ShowStoryText();
                    _canProgress = false;

                    yield return new WaitForSeconds(1.5f);

                    _audioManager.PlaySFX("Run");
                    _midori.transform.parent.GetComponent<RectTransform>().DOLocalMoveX(2000, 1f);
                    _canProgress = true;

                    yield break;
                }
                else if (num == 77)
                {
                    ShowSelection("“자, 잠시만!”");
                }
                else if (num == 78)
                {
                    SetDialogOn(false);
                    StartCoroutine(DoFadeEpisodeWindowFadeOut());
                    StartCoroutine(_audioManager.FadeOutMusic());

                    yield return new WaitForSeconds(2);

                    _audioManager.PlayBGM("RoundAndRound");
                    _story.transform.Find("Episode/EpisodeBackground").GetComponent<Image>().sprite = _episodeBackgroundImage.backgroundArray[5];

                    SetCharacterImage(_midori, 10);
                    _midori.transform.parent.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                    yield return new WaitForSeconds(2);

                    SetDialogOn(true);

                    PlayCharacterEffectAnimation(_midori, "Talking");
                    _midori.GetComponent<Animator>().Play("Jumping");
                }
                else if (num == 79)
                {
                    ShowSelection("( 신전이라기보다는 오락실처럼 보이는데…… )");
                }
                else if (num == 80)
                {
                    SetCharacterImage(_midori, 15);
                }
                else if (num == 81)
                {
                    SetCharacterImage(_midori, 14);
                    PlayCharacterEffectAnimation(_midori, "Sweat");
                }
                else if (num == 82)
                {
                    ShowSelection("( 게임기를 살펴본다. )");
                }
                else if (num == 83)
                {
                    StartCoroutine(DoFadeEpisodeWindowFadeOut());

                    yield return new WaitForSeconds(2);

                    _midori.SetActive(false);
                    SetDialogOn(false);
                    _story.transform.Find("Episode/EpisodeBackground").GetComponent<Image>().color = Color.black;
                    _story.transform.Find("Episode/WindowText_Ranking").gameObject.SetActive(true);

                    yield return new WaitForSeconds(2);
                    _audioManager.PlaySFX("Mess");
                    SetDialogOn(true);
                }
                else if (num == 85)
                {
                    _audioManager.PlaySFX("Sweat");
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
                    StartCoroutine(DoFadeEpisodeWindowFadeOut());
                    StartCoroutine(_audioManager.FadeOutMusic());

                    yield return new WaitForSeconds(2);

                    _audioManager.PlayBGM("KurameNoMyojo");
                    SetDialogOn(false);
                    SetCharacterImage(_midori, 15);
                    _midori.SetActive(true);
                    //_audioManager.PlayBGM("RoundAndRound");
                    _story.transform.Find("Episode/EpisodeBackground").GetComponent<Image>().sprite = _episodeBackgroundImage.backgroundArray[5];
                    _story.transform.Find("Episode/EpisodeBackground").GetComponent<Image>().color = Color.white;
                    _story.transform.Find("Episode/WindowText_Ranking").gameObject.SetActive(false);

                    yield return new WaitForSeconds(2);

                    _midori.GetComponent<Animator>().Play("Jumping");
                    _audioManager.PlaySFX("Shoot");

                    yield return new WaitForSeconds(0.3f);

                    _audioManager.PlaySFX("Shoot");

                    yield return new WaitForSeconds(0.3f);

                    _audioManager.PlaySFX("UseSkill");

                    yield return new WaitForSeconds(0.3f);

                    _audioManager.PlaySFX("EnemyDestroy");

                    yield return new WaitForSeconds(0.3f);

                    _midori.GetComponent<Animator>().Play("Jumping");
                    _audioManager.PlaySFX("Shoot");

                    yield return new WaitForSeconds(0.3f);

                    _audioManager.PlaySFX("Shoot");

                    yield return new WaitForSeconds(0.3f);

                    _audioManager.PlaySFX("EnemyDestroy");

                    yield return new WaitForSeconds(1f);

                    _audioManager.PlaySFX("Win");
                    SetDialogOn(true);

                    _audioManager.PlayBGM("RoundAndRound");
                }
                else if (num == 93)
                {
                    SetCharacterImage(_midori, 17);
                    PlayCharacterEffectAnimation(_midori, "Silence");
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
                    SetCharacterImage(_midori, 10);
                    _midori.GetComponent<Animator>().Play("Jumping");
                    PlayCharacterEffectAnimation(_midori, "Talking");

                    _canProgress = true;
                    ShowStoryText();
                    _canProgress = false;

                    yield return new WaitForSeconds(1);

                    DOTween.Sequence().Append(_midori.transform.parent.GetComponent<RectTransform>().DOLocalMoveX(-200, 0.5f))
                        .AppendInterval(0.5f).Append(_midori.transform.parent.GetComponent<RectTransform>().DOLocalMoveX(-400, 0.5f));
                    _audioManager.PlaySFX("Step");

                    yield return new WaitForSeconds(1);

                    _audioManager.PlaySFX("Step");
                    _canProgress = true;

                    yield break;
                }
                else if (num == 99)
                {
                    ShowSelection("( 게임을 시작한다. )");
                }
                else if (num == 100)
                {
                    StartCoroutine(DoFadeEpisodeWindowFadeOut());
                    StartCoroutine(_audioManager.FadeOutMusic());

                    yield return new WaitForSeconds(2);

                    SetDialogOn(false);
                    _gameManager._canvas.transform.Find("Story/Episode/UI/Menu").gameObject.SetActive(false); // 열린 메뉴창 숨기기
                    _gameManager._canvas.transform.Find("Story/Episode/UI").gameObject.SetActive(false); // UI 숨기기
                    _gameManager._canvas.transform.Find("Story/Episode/Dialog").gameObject.SetActive(false); // 대화창 숨기기
                    _gameManager.DoStartShootingGame();

                    _canProgress = false;

                    yield break;
                }
                else if (num == 102)
                {
                    SetCharacterImage(_midori, 13);
                    PlayCharacterEffectAnimation(_midori, "Talking");
                    _midori.GetComponent<Animator>().Play("Jumping");
                }
                else if (num == 103)
                {
                    ShowSelection("( 다시 시도한다. )");
                    _storyNum = 99;
                }
                else if (num == 104)
                {
                    SetCharacterImage(_midori, 13);
                    PlayCharacterEffectAnimation(_midori, "Talking");
                    _midori.GetComponent<Animator>().Play("Jumping");
                }
                else if (num == 105)
                {
                    SetCharacterImage(_midori, 17);
                }
                else if (num == 106)
                {
                    SetCharacterImage(_midori, 18);
                    PlayCharacterEffectAnimation(_midori, "Silence");
                }
                else if (num == 107)
                {
                    SetCharacterImage(_midori, 13);
                    PlayCharacterEffectAnimation(_midori, "Talking");
                    _midori.GetComponent<Animator>().Play("Jumping");
                }
                else if (num == 108)
                {
                    ShowSelection("( 다시 시도한다. )");
                    _storyNum = 99;
                }
                else if (num == 109)
                {
                    _audioManager.PlaySFX("Win");
                }
                else if (num == 110)
                {
                    SetCharacterImage(_midori, 13);
                    PlayCharacterEffectAnimation(_midori, "Talking");
                    _midori.GetComponent<Animator>().Play("Jumping");
                }
                else if (num == 111)
                {
                    SetCharacterImage(_midori, 18);
                }
                else if (num == 112)
                {
                    PlayCharacterEffectAnimation(_midori, "Silence");
                }
                else if (num == 113)
                {
                    SetCharacterImage(_midori, 13);
                    PlayCharacterEffectAnimation(_midori, "Shine");
                }
                else if (num == 114)
                {
                    ShowSelection("“아니야, 결국엔 미도리엘도 눈치챘을 거라고 생각해.”", "“잘하고 싶은 마음에 최선을 다하다 보니 주변에 신경을 못썼을 뿐이니까.”");
                }
                else if (num == 115)
                {
                    SetCharacterImage(_midori, 12);
                    PlayCharacterEffectAnimation(_midori, "Silence");
                }
                else if (num == 116)
                {
                    SetCharacterImage(_midori, 18);
                }
                else if (num == 117)
                {
                    SetCharacterImage(_midori, 17);
                }
                else if (num == 118)
                {
                    ShowSelection("“언니?”");
                }
                else if (num == 119)
                {
                    SetCharacterImage(_midori, 14);
                    PlayCharacterEffectAnimation(_midori, "Sweat");
                }
                else if (num == 120)
                {
                    SetCharacterImage(_midori, 17);
                    PlayCharacterEffectAnimation(_midori, "Silence");
                }
                else if (num == 123)
                {
                    ShowSelection("“동생은 여신인데, 언니는 마왕이라니……”");
                }
                else if (num == 124)
                {
                    SetCharacterImage(_midori, 14);
                    PlayCharacterEffectAnimation(_midori, "Sweat");
                }
                else if (num == 125)
                {
                    SetCharacterImage(_midori, 17);
                }
                else if (num == 126)
                {
                    ShowSelection("“미도리엘은 언니랑 사이가 좋지 않은 거야?”");
                }
                else if (num == 127)
                {
                    SetCharacterImage(_midori, 12);
                }
                else if (num == 128)
                {
                    SetCharacterImage(_midori, 17);
                }
                else if (num == 131)
                {
                    SetCharacterImage(_midori, 14);
                    PlayCharacterEffectAnimation(_midori, "Sweat");
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
                    SetCharacterImage(_midori, 12);
                }
                else if (num == 135)
                {
                    ShowSelection("“그리고 다른 사람의 플레이를 보면서 깨닫게 되는 점도 있지.”", "“나는 이렇게 플레이하는데, 이 사람은 이렇게 하는구나, 하고.”");
                }
                else if (num == 136)
                {
                    SetCharacterImage(_midori, 18);
                    PlayCharacterEffectAnimation(_midori, "Silence");
                }
                else if (num == 137)
                {
                    SetCharacterImage(_midori, 10);
                }
                else if (num == 138)
                {
                    _audioManager.PlaySFX("RankingWarning");
                }
                else if (num == 139)
                {
                    SetCharacterImage(_midori, 12);
                    PlayCharacterEffectAnimation(_midori, "Talking");
                    _midori.GetComponent<Animator>().Play("Jumping");
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
                    PlayCharacterEffectAnimation(_midori, "Silence");
                }
                else if (num == 146)
                {
                    SetCharacterImage(_midori, 14);
                    PlayCharacterEffectAnimation(_midori, "Talking");
                    _midori.GetComponent<Animator>().Play("Jumping");
                }
                else if (num == 147)
                {
                    ShowSelection("“미, 미도리엘!?”");
                }
                else if (num == 150)
                {
                    SetCharacterImage(_midori, 13);
                    PlayCharacterEffectAnimation(_midori, "Shine");
                }
                else if (num == 151)
                {
                    PlayCharacterEffectAnimation(_midori, "Talking");
                    _midori.GetComponent<Animator>().Play("Jumping");

                    _canProgress = true;
                    ShowStoryText();
                    _canProgress = false;

                    yield return new WaitForSeconds(1.5f);

                    _audioManager.PlaySFX("Run");
                    _midori.transform.parent.GetComponent<RectTransform>().DOLocalMoveX(2000, 1f);
                    _canProgress = true;

                    yield break;
                }
                else if (num == 152)
                {
                    _story.transform.Find("Episode/EpisodeBackground").GetComponent<Image>().DOColor(Color.black, 2);
                }
                else if (num == 153) // 메인화면으로 이동
                {
                    CheckAutoProgress();

                    _gameManager.DoFinishStory(0);

                    yield break;
                }
            }

            if (_isSelectionOn) yield break;  // 선택지를 보여줄 때 대화창 갱신하지 않도록 체크

            _canProgress = true;

            ShowStoryText();
        }

        public void DoShootingGameEnd(string endType)
        {
            StartCoroutine(DoShootingGameEndCoroutine(endType));
        }

        public IEnumerator DoShootingGameEndCoroutine(string endType)
        {
            yield return new WaitForSeconds(2);

            StartCoroutine(DoFadeEpisodeWindowFadeOut());

            yield return new WaitForSeconds(2);

            _audioManager.PlayBGM("RoundAndRound");

            _gameManager.DoFinishShootingGame();

            _midori.SetActive(true);
            _midori.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(-400, 0);
            _midori.transform.parent.GetComponent<RectTransform>().localScale = new Vector2(1, 1);

            if (endType.Equals("GameOver")) SetCharacterImage(_midori, 17);
            else SetCharacterImage(_midori, 13);

            yield return new WaitForSeconds(2);

            SetDialogOn(true);
            _gameManager._canvas.transform.Find("Story/Episode/UI").gameObject.SetActive(true);
            _canProgress = true;

            if (endType.Equals("GameOver")) _storyNum = 101;
            else if (endType.Equals("GameWin")) _storyNum = 104;
            else _storyNum = 109;

            DoStoryProgress();
        }

        private IEnumerator AppendTextOneByOne(TMP_Text textBox, string text, float textSpeed)
        {
            for (int i = 0; i < text.Length; i++)
            {
                textBox.text += text[i];
                yield return new WaitForSeconds(0.05f / textSpeed);
            }
        }

        // 대화를 한 글자씩 출력해주는 함수(기본)
        private IEnumerator SetTextBoxCoroutine(int num)
        {
            if (_characterNameList.Count <= num) yield break;
            _story.transform.Find("Episode/Dialog/DialogEnd").gameObject.SetActive(false);

            // 이름 길이에 따라 소속 표시 위치 변경
            if (_characterName.GetComponent<TMP_Text>().text.Length != _characterNameList[num].Length) // 이름 길이가 변경되었을 때만 동작
            {
                _departmentName.GetComponent<RectTransform>().anchoredPosition = new Vector2(-457 + ((_characterNameList[num].Length - 2) * 52), _departmentName.GetComponent<RectTransform>().anchoredPosition.y);
            }

            // 얘기 중인 학생의 이미지를 밝게, 얘기하고 있지 않은 학생의 이미지를 어둡게 변경
            if (_characterNameList[num] == "모모이")
            {
                _momoiCharacterImage.color = new Color(1, 1, 1, _momoiCharacterImage.color.a);
                _momoiHaloImage.color = new Color(1, 1, 1, _momoiHaloImage.color.a);

                _midoriCharacterImage.color = new Color(0.5f, 0.5f, 0.5f, _midoriCharacterImage.color.a);
                _midoriHaloImage.color = new Color(0.5f, 0.5f, 0.5f, _midoriHaloImage.color.a);

                _arisCharacterImage.color = new Color(0.5f, 0.5f, 0.5f, _arisCharacterImage.color.a);
                _arisHaloImage.color = new Color(0.5f, 0.5f, 0.5f, _arisHaloImage.color.a);

                _yuzuCharacterImage.color = new Color(0.5f, 0.5f, 0.5f, _yuzuCharacterImage.color.a);
                _yuzuHaloImage.color = new Color(0.5f, 0.5f, 0.5f, _yuzuHaloImage.color.a);
            }
            else if (_characterNameList[num] == "미도리")
            {
                _momoiCharacterImage.color = new Color(0.5f, 0.5f, 0.5f, _momoiCharacterImage.color.a);
                _momoiHaloImage.color = new Color(0.5f, 0.5f, 0.5f, _momoiHaloImage.color.a);

                _midoriCharacterImage.color = new Color(1, 1, 1, _midoriCharacterImage.color.a);
                _midoriHaloImage.color = new Color(1, 1, 1, _midoriHaloImage.color.a);

                _arisCharacterImage.color = new Color(0.5f, 0.5f, 0.5f, _arisCharacterImage.color.a);
                _arisHaloImage.color = new Color(0.5f, 0.5f, 0.5f, _arisHaloImage.color.a);

                _yuzuCharacterImage.color = new Color(0.5f, 0.5f, 0.5f, _yuzuCharacterImage.color.a);
                _yuzuHaloImage.color = new Color(0.5f, 0.5f, 0.5f, _yuzuHaloImage.color.a);
            }
            else if (_characterNameList[num] == "아리스")
            {
                _momoiCharacterImage.color = new Color(0.5f, 0.5f, 0.5f, _momoiCharacterImage.color.a);
                _momoiHaloImage.color = new Color(0.5f, 0.5f, 0.5f, _momoiHaloImage.color.a);

                _midoriCharacterImage.color = new Color(0.5f, 0.5f, 0.5f, _midoriCharacterImage.color.a);
                _midoriHaloImage.color = new Color(0.5f, 0.5f, 0.5f, _midoriHaloImage.color.a);

                _arisCharacterImage.color = new Color(1, 1, 1, _arisCharacterImage.color.a);
                _arisHaloImage.color = new Color(1, 1, 1, _arisHaloImage.color.a);

                _yuzuCharacterImage.color = new Color(0.5f, 0.5f, 0.5f, _yuzuCharacterImage.color.a);
                _yuzuHaloImage.color = new Color(0.5f, 0.5f, 0.5f, _yuzuHaloImage.color.a);
            }
            else if (_characterNameList[num] == "유즈")
            {
                _momoiCharacterImage.color = new Color(0.5f, 0.5f, 0.5f, _momoiCharacterImage.color.a);
                _momoiHaloImage.color = new Color(0.5f, 0.5f, 0.5f, _momoiHaloImage.color.a);

                _midoriCharacterImage.color = new Color(0.5f, 0.5f, 0.5f, _midoriCharacterImage.color.a);
                _midoriHaloImage.color = new Color(0.5f, 0.5f, 0.5f, _midoriHaloImage.color.a);

                _arisCharacterImage.color = new Color(0.5f, 0.5f, 0.5f, _arisCharacterImage.color.a);
                _arisHaloImage.color = new Color(0.5f, 0.5f, 0.5f, _arisHaloImage.color.a);

                _yuzuCharacterImage.color = new Color(1, 1, 1, _yuzuCharacterImage.color.a);
                _yuzuHaloImage.color = new Color(1, 1, 1, _yuzuHaloImage.color.a);
            }


            _characterName.GetComponent<TMP_Text>().text = _characterNameList[num];
            _departmentName.GetComponent<TMP_Text>().text = _departmentNameList[num];
            _dialogText.GetComponent<TMP_Text>().text = "";
            _isStoryProgressing = true;
            float textSpeed = _textSpeedList[num];
            for (int i = 0; i < _dialogList[num].Length; i++)
            {
                _dialogText.GetComponent<TMP_Text>().text += _dialogList[num][i];
                yield return new WaitForSeconds(0.05f / textSpeed);
            }
            _storyNum++;
            DoStoryJump(); // 선택지로 갈린 스토리 번호 이동 체크

            _isStoryProgressing = false;
            _story.transform.Find("Episode/Dialog/DialogEnd").gameObject.SetActive(true);
        }

        // 대화 나오고 있을 때 클릭하면 한 번에 출력되도록 해주는 함수
        private void SetTextBox(int num)
        {
            if (_characterNameList.Count <= num) return;
            _characterName.GetComponent<TMP_Text>().text = _characterNameList[num];
            _departmentName.GetComponent<TMP_Text>().text = _departmentNameList[num];
            _dialogText.GetComponent<TMP_Text>().text = _dialogList[num];
            _storyNum++;
            DoStoryJump(); // 선택지로 갈린 스토리 번호 이동 체크

            _isStoryProgressing = false;
            _story.transform.Find("Episode/Dialog/DialogEnd").gameObject.SetActive(true);
        }

        // 스토리를 진행시키는 함수
        public void DoStoryProgress()
        {
            if (!_canProgress)
            {
                return;
            }
            StartCoroutine(DoStoryAction(_storyNum));
        }

        // 스토리에서 텍스트를 나타내는 함수
        private void ShowStoryText()
        {
            if (_isStoryProgressing)
            {
                StopCoroutine(_coroutineStoryProgress);
                SetTextBox(_storyNum);
                return;
            }
            _coroutineStoryProgress = StartCoroutine(SetTextBoxCoroutine(_storyNum));
        }

        // 스토리에 처음 진입했을 때 작동하는 함수
        public IEnumerator DoEpisodeStart()
        {
            _story.SetActive(true);
            _episodeStartWindowFadeOut.color = new Color(1, 1, 1, 0);
            _story.transform.Find("EpisodeStart").gameObject.SetActive(true);
            _story.transform.Find("Episode").gameObject.SetActive(false);
            StartCoroutine(_audioManager.FadeOutMusic());
            yield return new WaitForSeconds(6.5f);
            _episodeStartWindowFadeOut.DOFade(1, 2);

            // 여기부터 스토리 초기 세팅

            _dialogText.GetComponent<TMP_Text>().fontSize = 42.5f;
            SetCharacterImage(_momoi, 0);
            SetCharacterImage(_midori, 0);
            SetCharacterImage(_aris, 0);
            SetCharacterImage(_yuzu, 0);
            _story.transform.Find("Episode/EpisodeBackground").GetComponent<Image>().color = new Color(1, 1, 1, 0);
            _momoi.SetActive(false);
            _midori.SetActive(false);
            _aris.SetActive(false);
            _yuzu.SetActive(false);

            _momoiCharacterImage.color = new Color(1, 1, 1, 0);
            _momoiHaloImage.color = new Color(1, 1, 1, 0);
            _midoriCharacterImage.color = new Color(1, 1, 1, 0);
            _midoriHaloImage.color = new Color(1, 1, 1, 0);
            _arisCharacterImage.color = new Color(1, 1, 1, 0);
            _arisHaloImage.GetComponent<Image>().color = new Color(1, 1, 1, 0);
            _yuzuCharacterImage.color = new Color(1, 1, 1, 0);
            _yuzuHaloImage.color = new Color(1, 1, 1, 0);

            _momoi.SetActive(false);
            _midori.SetActive(false);
            _aris.SetActive(false);
            _yuzu.SetActive(false);

            // 여기까지 스토리 초기 세팅

            yield return new WaitForSeconds(2.5f);
            _story.transform.Find("Episode").gameObject.SetActive(true);
            _story.transform.Find("Episode/UI").gameObject.SetActive(true);
            _story.transform.Find("Episode/Dialog").gameObject.SetActive(true);
            _story.transform.Find("Episode/EpisodeProgressBtn").gameObject.SetActive(true);
            _canProgress = true;
            DoStoryProgress();
        }
    }
}