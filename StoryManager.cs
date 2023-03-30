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

        // �̹����� 9�� �ִ� ĳ������ ���, 9���� ���� �̹����� ����־� 10~19���� ����� �� �ֵ��� ����
        [SerializeField] MomoiImage _momoiImage;
        [SerializeField] MidoriImage _midoriImage;
        [SerializeField] ArisImage _arisImage;
        [SerializeField] YuzuImage _yuzuImage;

        // ButtonImage 0: ��� ���, 1: ����� ���
        [SerializeField] ButtonImage _buttonImage;

        // 0: Background, 1: GameDevelopment, 2: TSCBackground, 3: RingOfRight, 4: Ruins, 5: GameCenter
        [SerializeField] EpisodeBackgroundImage _episodeBackgroundImage;

        private GameObject _characterName;
        private GameObject _departmentName;
        private GameObject _dialog;
        private GameObject _selection1Btn;
        private GameObject _selection1_1Btn;
        private GameObject _selection1_2Btn;

        private Image _momoiCharacterImage;
        private Image _midoriCharacterImage;
        private Image _yuzuCharacterImage;
        private Image _arisCharacterImage;
        private Image _momoiHaloImage;
        private Image _midoriHaloImage;
        private Image _yuzuHaloImage;
        private Image _arisHaloImage;

        private List<string> _characterNameList = new List<string>();    // ĳ���� �̸� ���
        private List<string> _departmentNameList = new List<string>();   // �Ҽ� �̸� ���
        private List<string> _dialogList = new List<string>();           // ��ȭ ���� ���
        private List<float> _textSpeedList = new List<float>();          // ��ȭ ���� ���

        private int _storyNum = 0;                   // ���丮�� ������ ����ƴ��� �����ϴ� ��ȣ

        private Coroutine _coroutineStoryProgress;   // ��縦 �ѱ��ھ� ����ϴ� �ڷ�ƾ, ȭ���� ��ġ�ϸ� ������Ű�� �� ���� ǥ���ϱ� ���� ���
        private Coroutine _autoStoryProgress;        // �ڵ����� ���丮�� ��������ִ� �ڷ�ƾ
        private Coroutine _coroutineWindowFadeOut;   // ȭ���� �˰� Fade Out�Ǵ� �ڷ�ƾ, ȭ���� �ٽ� Fade In �� �� �浹�� ������ �ʵ��� �����Ͽ� �ߴܽ�Ŵ

        private bool _canProgress = false;           // ���� ���丮�� ������ �� �ִ��� ����
        private bool _isStoryProgressing = false;    // ���丮���� ���� ��簡 ������ �ִ��� Ȯ��
        private bool _isAutoStoryProgress;           // ���� �ڵ����� ���丮�� ���� ������ ���θ� �Ǵ�
        private bool _isDialogOff = false;           // ��ȭâ�� �����ִ� �������� Ȯ��
        private bool _isSelectionOn = false;         // �������� �������� Ȯ��
        private bool _isPressedButton = false;       // ��ư�� �������� Ȯ��

        private int _autoStoryNum = 0;

        public override void Awake()
        {
            // GameObject ��ü ����
            _gameManager = GameManager.Instance;
            _audioManager = AudioManager.Instance;
            
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
            _dialog = _story.transform.Find("Episode/Dialog/DialogText").gameObject;

            _selection1Btn = _story.transform.Find("Episode/UI/Selection1Btn").gameObject;
            _selection1_1Btn = _story.transform.Find("Episode/UI/Selection1-1Btn").gameObject;
            _selection1_2Btn = _story.transform.Find("Episode/UI/Selection1-2Btn").gameObject;
        }

        // �ش� ĳ������ ����Ʈ �ִϸ��̼��� ����ϴ� �Լ�
        private void PlayCharacterEffectAnimation(GameObject characterName, string animationName)
        {
            characterName.transform.Find($"{animationName}Parent/{animationName}").GetComponent<Animator>().Play(animationName);
            _audioManager.PlaySFX(animationName);
        }

        // �������� ���� ���丮 ��ȣ �̵� üũ�ϰ� �̵��ϴ� �Լ�
        private void DoStoryJump()
        {
            if (_storyNum == 23)
            {
                _storyNum = 28;
            }
        }

        // ��ȭâ�� ���� Ű�� �Լ�
        private void SetDialogOn(bool isDialogOff)
        {
            if (!isDialogOff)
            {
                _canProgress = false;
                _isDialogOff = true;
                _dialog.transform.parent.gameObject.SetActive(false);
            }
            else
            {
                _canProgress = true;
                _isDialogOff = false;
                _dialog.transform.parent.gameObject.SetActive(true);
            }
        }

        // ���丮 ���� �� ȭ���� Ŭ������ �� �۵��ϴ� �Լ�
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

        // �������� ������ �� �۵��ϴ� �Լ�
        public IEnumerator SelectedSelection(int num)
        {
            if (_isPressedButton) yield break;
            _isPressedButton = true;
            yield return new WaitForSeconds(0.5f); // ��ư�� ���� �� ����ǵ��� ��� ���
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

                // �������� ���� �˸��� ��ȣ�� �̵�
                if (_storyNum == 14) // AŰ�� �Է��Ѵ�.
                {
                    _storyNum = 23;
                }
                else if (_storyNum == 30) // AŰ �Է�
                {
                    _storyNum = 30;
                }
            }
            else if (num == 2)
            {
                _selection1_1Btn.SetActive(false);
                _selection1_2Btn.SetActive(false);

                // �������� ���� �˸��� ��ȣ�� �̵�
                if (_storyNum == 14) // BŰ�� �Է��Ѵ�.
                {
                    _storyNum = 14;
                }
                else if (_storyNum == 30) // Ǫ���������� ���ɽ����� �����Ѵ�.
                {
                    _storyNum = 36;
                }
            }

            _canProgress = true;
            DoStoryProgress();
        }

        // 1��¥�� �������� �����ִ� �Լ�
        private void ShowSelection(string selectionDialog)
        {
            _isPressedButton = false;
            _isSelectionOn = true;
            _selection1Btn.SetActive(true);
            _selection1Btn.transform.Find("Text").GetComponent<TMP_Text>().text = selectionDialog;
            _selection1Btn.GetComponent<Animator>().Play("ButtonPopup");
        }

        // 2��¥�� �������� �����ִ� �Լ�
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

        // �л����� �̹����� �������ִ� �Լ�
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
                // ������� ǥ���� 0���� ���� ���� �����̴� ����� ����
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

        // ���丮���� AUTO ��ư�� ������ �� �ڵ����� ���丮�� ����ǵ��� ���ִ� �Լ�
        public void DoAutoProgress()
        {
            if (CheckAutoProgress()) return;

            _autoStoryProgress = StartCoroutine(DoAutoProgressCoroutine());
            _isAutoStoryProgress = true;
            _story.transform.Find("Episode/UI/AutoBtn").GetComponent<Image>().sprite = _buttonImage.buttonArray[1];
        }

        // �ڵ����� ���丮�� ����ǵ��� ���ִ� �ڷ�ƾ
        public IEnumerator DoAutoProgressCoroutine()
        {
            while (true)
            {
                // �ƾ��� �������� ���� ���
                while (!_canProgress)
                {
                    yield return new WaitForSeconds(1);
                }

                // ��ȭ�� ��µǰ� ���� ���� ���� ����
                if (!_isStoryProgressing)
                {
                    // ��ȭ ����� �Ϸ�� �� �ּ� 1�� ���
                    _autoStoryNum = _storyNum;
                    yield return new WaitForSeconds(1);
                    // ���丮 �ڵ� ���� �� ������ ȭ���� ��ġ�ؼ� ���� ���� �Ѿ�� ī��Ʈ�� �ٽ� ����
                    // �̰� ������ ������ 2�� ��ġ�� �ؼ� ���� ���� Ÿ�̹��� ������ �Ѿ ���, ��簡 ��ٷ� ��ŵ�Ǵ� ������ �߻���
                    // Ȯ���غ��� ��� ��ī�̺꿡���� ������ �̽��� ������.. AUTO ��ư�� ���� ä�� ��ġ�ϴ� ������ ������ �����״�...
                    if (_autoStoryNum == _storyNum)
                    {
                        DoStoryProgress();
                    }
                }
                else
                {
                    yield return new WaitForSeconds(1); // 1�ʸ��� Ȯ��
                }
            }
        }

        // GameManager���� ���丮 ���� ��ȣ�� ������ �� ����ϴ� �Լ�
        public void SetStoryNumber(int num)
        {
            _storyNum = num;
        }

        // �ܺο��� ���丮�� ������ �� �ִ� �������� ���θ� �����ϴ� �Լ�
        public void SetIsCanProgress(bool isCanProgress)
        {
            this._canProgress = isCanProgress;
        }

        // ĳ���� �̸�, �μ� �̸�, ��ȭ ����, �ؽ�Ʈ �ӵ��� �����ϰ� ���� �����ϱ� ���� �Լ�
        public void AppendDialog(string characterNameText, string departmentNameText, string dialogText, float textSpeed)
        {
            _characterNameList.Add(characterNameText);
            _departmentNameList.Add(departmentNameText);
            _dialogList.Add(dialogText);
            _textSpeedList.Add(textSpeed); // �ؽ�Ʈ �ӵ� : (textSpeed) ��
        }

        private IEnumerator DoFadeEpisodeWindowFadeOut()
        {
            _WindowFadeOut.gameObject.SetActive(true);
            DOTween.Sequence().Append(_WindowFadeOut.DOFade(1, 2f)).Append(_WindowFadeOut.DOFade(0, 2f));

            yield return new WaitForSeconds(4);

            _WindowFadeOut.gameObject.SetActive(false);
        }

        // ���丮 ����� �ؽ�Ʈ �ܿ� ĳ���� ������, ȿ���� ���� �����ϴ� ��Ʈ
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
                    _dialog.GetComponent<TMP_Text>().fontSize = 60;
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
                    _dialog.GetComponent<TMP_Text>().fontSize = 42.5f;
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
                else if (num == 8) // ������
                {
                    ShowSelection("( �ڸ��� �ɴ´�. )");
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
                    StartCoroutine(AppendTextOneByOne(_story.transform.Find("Episode/WindowText").GetComponent<TMP_Text>(), "< �ڽ��� ���� 2354��, �η��� ��ȭ�� �ұ濡 �۽ο���. >\n\n", 1));
                    yield return new WaitForSeconds(2.5f);
                    ShowSelection("��������ȭ�� ��ä?��");
                }
                else if (num == 11)
                {
                    SetDialogOn(true);
                }
                else if (num == 12)
                {
                    ShowSelection("������.��", "( ��ư�� �Է��Ѵ�. )");
                }
                else if (num == 13)
                {
                    SetDialogOn(false);
                    StartCoroutine(AppendTextOneByOne(_story.transform.Find("Episode/WindowText").GetComponent<TMP_Text>(), "< Ʃ�丮���� �����մϴ�. >\n\n", 1));
                    yield return new WaitForSeconds(2);
                    StartCoroutine(AppendTextOneByOne(_story.transform.Find("Episode/WindowText").GetComponent<TMP_Text>(), "< ���� B Ű�� ����, ������ ���⸦ �����غ�����. >\n\n", 1));
                    yield return new WaitForSeconds(2.5f);
                    ShowSelection("( A Ű�� �Է��Ѵ�. )", "( B Ű�� �Է��Ѵ�. )");
                }
                // B ��ư ��Ʈ ����
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
                    ShowSelection("��???��");
                }
                else if (num == 16)
                {
                    SetDialogOn(false);
                    // ���� ����
                    _story.transform.Find("Episode/WindowText").GetComponent<TMP_Text>().text = "";
                    _audioManager.PlaySFX("GameOver");
                    _story.transform.Find("Episode/WindowText_GameOver").GetComponent<TMP_Text>().DOFade(1, 2f);
                    yield return new WaitForSeconds(4);
                    ShowSelection("��?!��");
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
                    _story.transform.Find("Episode/WindowText").GetComponent<TMP_Text>().text = "";

                    //GameOver �۾� �����ϰ� ����
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
                    ShowSelection("������.��");
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
                    ShowSelection("( �ٽ� �õ��Ѵ�. )");
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
                    StartCoroutine(AppendTextOneByOne(_story.transform.Find("Episode/WindowText").GetComponent<TMP_Text>(), "< ���� ������ �����߽��ϴ�. >\n\n", 1));
                    _audioManager.PlaySFX("Message");
                    yield return new WaitForSeconds(2f);
                    SetDialogOn(true);
                }
                // B ��ư ��Ʈ ����

                // A��ư ��Ʈ
                else if (num == 23)
                {
                    _story.transform.Find("Episode/WindowText").GetComponent<TMP_Text>().text = "";
                    _audioManager.StopBGM();
                    StartCoroutine(AppendTextOneByOne(_story.transform.Find("Episode/WindowText").GetComponent<TMP_Text>(), "< ���� ������ �����߽��ϴ�. >\n\n", 1));
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
                // A��ư ��Ʈ ����
                else if (num == 28)
                {
                    SetDialogOn(false);
                    _audioManager.PlayBGM("MechanicalJungle");
                    _story.transform.Find("Episode/WindowText").GetComponent<TMP_Text>().text += "<#FF656B>";
                    StartCoroutine(AppendTextOneByOne(_story.transform.Find("Episode/WindowText").GetComponent<TMP_Text>(), "< ���� �߻�! ���� �߻�! >\n\n", 1));
                    yield return new WaitForSeconds(2);
                    _story.transform.Find("Episode/WindowText").GetComponent<TMP_Text>().text += "</color>";
                    StartCoroutine(AppendTextOneByOne(_story.transform.Find("Episode/WindowText").GetComponent<TMP_Text>(), "< �߻��� Ǫ�������� ��Ÿ����! >\n\n", 1));
                    yield return new WaitForSeconds(2);
                    SetDialogOn(true);
                }
                else if (num == 29)
                {
                    ShowSelection("( A��ư �Է�, < ��� ���ٸް�����, �� ���� ���� 2ȸ �����Ѵ�. > )", "( Ǫ���������� ���ɽ����� �����Ѵ�. )");
                }
                // A��ư�� ������ ��Ʈ ����
                else if (num == 30)
                {
                    SetDialogOn(false);
                    _audioManager.StopBGM();
                    StartCoroutine(AppendTextOneByOne(_story.transform.Find("Episode/WindowText").GetComponent<TMP_Text>(), "< ��!! >\n\n", 1));
                    _audioManager.PlaySFX("8bitBang");
                    yield return new WaitForSeconds(2);
                    StartCoroutine(AppendTextOneByOne(_story.transform.Find("Episode/WindowText").GetComponent<TMP_Text>(), "< ����, ����� ĳ���Ͱ� ����ߴ�. >\n\n", 1));
                    _audioManager.PlaySFX("KnockDown");
                    yield return new WaitForSeconds(2);

                    // ���� ����
                    _story.transform.Find("Episode/WindowText").GetComponent<TMP_Text>().text = "";
                    _audioManager.PlaySFX("GameOver");
                    _story.transform.Find("Episode/WindowText_GameOver").GetComponent<TMP_Text>().DOFade(1, 2);
                    yield return new WaitForSeconds(4);

                    ShowSelection("��??!!��");
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
                    _story.transform.Find("Episode/WindowText").GetComponent<TMP_Text>().text = "";

                    //GameOver �۾� �����ϰ� ����
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
                    ShowSelection("( �ٽ� �õ��Ѵ�. )");
                }
                else if (num == 35) // Window Fade Out(���� �κ�����)
                {
                    StartCoroutine(DoFadeEpisodeWindowFadeOut());
                    StartCoroutine(_audioManager.FadeOutMusic());
                    _audioManager.PlaySFX("TSCStart");
                    yield return new WaitForSeconds(1);
                    yield return new WaitForSeconds(1);
                    SetDialogOn(false);
                    _momoi.transform.gameObject.SetActive(false);
                    _aris.transform.gameObject.SetActive(false);
                    _story.transform.Find("Episode/EpisodeBackground").GetComponent<Image>().sprite = _episodeBackgroundImage.backgroundArray[2];
                    yield return new WaitForSeconds(2);

                    StartCoroutine(AppendTextOneByOne(_story.transform.Find("Episode/WindowText").GetComponent<TMP_Text>(), "< ���� ������ �����߽��ϴ�. >\n\n", 1));
                    _audioManager.PlaySFX("Message");
                    yield return new WaitForSeconds(2f);

                    _audioManager.PlayBGM("MechanicalJungle");
                    _story.transform.Find("Episode/WindowText").GetComponent<TMP_Text>().text += "<#FF656B>";
                    StartCoroutine(AppendTextOneByOne(_story.transform.Find("Episode/WindowText").GetComponent<TMP_Text>(), "< ���� �߻�! ���� �߻�! >\n\n", 1));
                    yield return new WaitForSeconds(2);
                    _story.transform.Find("Episode/WindowText").GetComponent<TMP_Text>().text += "</color>";
                    StartCoroutine(AppendTextOneByOne(_story.transform.Find("Episode/WindowText").GetComponent<TMP_Text>(), "< �߻��� Ǫ�������� ��Ÿ����! >\n\n", 1));
                    yield return new WaitForSeconds(2);

                    ShowSelection("( Ǫ���������� ���ɽ����� �����Ѵ�. )");
                }
                // Ǫ���������� ���ɽ����� �����Ѵ�. (���� ��Ʈ)
                else if (num == 36)
                {
                    SetDialogOn(false);
                    yield return new WaitForSeconds(2);
                    StartCoroutine(AppendTextOneByOne(_story.transform.Find("Episode/WindowText").GetComponent<TMP_Text>(), "< ���� �߻�! ���� �߻�! >\n\n", 1));
                    _audioManager.StopBGM();
                    _audioManager.PlaySFX("RobotTalk");
                    yield return new WaitForSeconds(2);
                    StartCoroutine(AppendTextOneByOne(_story.transform.Find("Episode/WindowText").GetComponent<TMP_Text>(), "< �ɰ��� ���� �߻����� �ý����� ��� �����մϴ�. >\n\n", 1));
                    _audioManager.PlaySFX("RobotTalk");
                    yield return new WaitForSeconds(2);
                    ShowSelection("��!?��");
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
                    _story.transform.Find("Episode/WindowText").GetComponent<TMP_Text>().text = "";

                    _story.transform.Find("Episode/EpisodeBackground").GetComponent<Image>().sprite = _episodeBackgroundImage.backgroundArray[1];
                    yield return new WaitForSeconds(2);
                    // ����� ����?

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
                    ShowSelection("( ������ ������١��� )");
                }
                else if (num == 43)
                {
                    SetDialogOn(false);
                    _WindowFadeOut.gameObject.SetActive(true);
                    // ȭ�� �����̴� ����, DOTween�� ����ϸ� �߰��߰� ����� ������ �� ���� ������ �Լ� ���

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


                    // ��ȭâ�� ������ WindowFadeOut ���߰�, ����� ���������� ����
                    _story.transform.Find("Episode/EpisodeBackground").GetComponent<Image>().sprite = _episodeBackgroundImage.backgroundArray[0];
                    _story.transform.Find("Episode/EpisodeBackground").GetComponent<Image>().color = Color.black;
                    StartCoroutine(_gameManager.FadeOutImage(4f, _WindowFadeOut));
                    _yuzu.SetActive(false);
                    _aris.SetActive(false);
                    _audioManager.PlaySFX("KnockDown"); // �������� �Ҹ�
                    _audioManager.StopBGM();
                    yield return new WaitForSeconds(1);

                    _WindowFadeOut.gameObject.SetActive(false);
                    SetDialogOn(true);
                    _dialog.GetComponent<TMP_Text>().fontSize = 60;
                    _audioManager.PlaySFX("Confuse");
                }
                else if (num == 44) // ����ȭ������ �̵�
                {
                    _gameManager.DoFinishStory(0);
                }
                // 1�� ����

                // 2�� ����
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
                    ShowSelection("( ����. )");
                }
                else if (num == 52)
                {
                    ShowSelection("( ����. )");
                }
                else if (num == 53)
                {
                    _story.transform.Find("Episode/EpisodeMovingBackground").GetComponent<Image>().DOFade(0, 2);
                    yield return new WaitForSeconds(2);

                    _midori.SetActive(false);
                }
                else if (num == 54)
                {
                    ShowSelection("( ���� ���. )");
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
                    ShowSelection("( �̵���? )", "( �����ƴ�, ���Ϸΰ� ���� �� ���� �츮 �л��� �ƴϱ�. )");
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
                    ShowSelection("�������������� ���� �ƴϾ���?��");
                }
                else if (num == 63)
                {
                    SetCharacterImage(_midori, 14);
                    PlayCharacterEffectAnimation(_midori, "Mess");
                    _midori.GetComponent<Animator>().Play("Shiver");
                }
                else if (num == 64)
                {
                    ShowSelection("���׷� ���� ����.��");
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
                    ShowSelection("�������� ���� �ο��� �� ���ϴ� ��.��");
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
                    ShowSelection("������ ������ �ȴٸ�� �󸶵���.��");
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
                    ShowSelection("����, ��ø�!��");
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
                    ShowSelection("( �����̶�⺸�ٴ� ������ó�� ���̴µ����� )");
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
                    ShowSelection("( ���ӱ⸦ ���캻��. )");
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
                    ShowSelection("���ֱ� ��¥�� �ü��� ������ ������ ��������?��");
                }
                else if (num == 90)
                {
                    ShowSelection("( �̵����� �÷��̸� ���Ѻ���. )");
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
                    ShowSelection("�������� ���� ���� ���߽��״µ��� ��ŷ ���Կ� �����ϴٴϡ�����");
                }
                else if (num == 95)
                {
                    //PlayCharacterEffectAnimation(_midori, "Silence");
                }
                else if (num == 96)
                {
                    ShowSelection("( �׷��ٸ� Ȥ�á��� )", "������ �÷����� ���� ��������?��");
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
                    ShowSelection("( ������ �����Ѵ�. )");
                }
                else if (num == 100)
                {
                    StartCoroutine(DoFadeEpisodeWindowFadeOut());
                    StartCoroutine(_audioManager.FadeOutMusic());

                    yield return new WaitForSeconds(2);

                    SetDialogOn(false);
                    _gameManager._canvas.transform.Find("Story/Episode/UI/Menu").gameObject.SetActive(false); // ���� �޴�â �����
                    _gameManager._canvas.transform.Find("Story/Episode/UI").gameObject.SetActive(false); // UI �����
                    _gameManager._canvas.transform.Find("Story/Episode/Dialog").gameObject.SetActive(false); // ��ȭâ �����
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
                    ShowSelection("( �ٽ� �õ��Ѵ�. )");
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
                    ShowSelection("( �ٽ� �õ��Ѵ�. )");
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
                    ShowSelection("���ƴϾ�, �ᱹ�� �̵������� ��ġë�� �Ŷ�� ������.��", "�����ϰ� ���� ������ �ּ��� ���ϴ� ���� �ֺ��� �Ű��� ������ ���̴ϱ�.��");
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
                    ShowSelection("�����?��");
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
                    ShowSelection("�������� �����ε�, ��ϴ� �����̶�ϡ�����");
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
                    ShowSelection("���̵������� ��϶� ���̰� ���� ���� �ž�?��");
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
                    ShowSelection("���ƴϾ�. �����ڸų��� ������ ���� ���� �ִ� ������.��");
                }
                else if (num == 133)
                {
                    ShowSelection("���Ƹ� ���ڱ⸸ �� ���� �ƴϾ��� �ž�.��", "�������� �����ϴ� ���� ���Ѻ��� �͵� ������ݾ�.��");
                }
                else if (num == 134)
                {
                    SetCharacterImage(_midori, 12);
                }
                else if (num == 135)
                {
                    ShowSelection("���׸��� �ٸ� ����� �÷��̸� ���鼭 ���ݰ� �Ǵ� ���� ����.��", "������ �̷��� �÷����ϴµ�, �� ����� �̷��� �ϴ±���, �ϰ�.��");
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
                    ShowSelection("���̵������� �̸��� ����Ѵ�.��");
                }
                else if (num == 142)
                {
                    ShowSelection("����¿ �� ���� �̵��������� �Է��Ѵ�.��");
                }
                else if (num == 144)
                {
                    ShowSelection("���̵������� �ܼ��� �˷����� �ʾҴ���顱", "������ ���� ���ھ �޼��� �� ������ �״ϱ�.��");
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
                    ShowSelection("����, �̵�����!?��");
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
                else if (num == 153) // ����ȭ������ �̵�
                {
                    _gameManager.DoFinishStory(1);
                }
            }

            if (_isSelectionOn) yield break;  // �������� ������ �� ��ȭâ �������� �ʵ��� üũ

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

        // ��ȭ�� �� ���ھ� ������ִ� �Լ�(�⺻)
        private IEnumerator SetTextBoxCoroutine(int num)
        {
            if (_characterNameList.Count <= num) yield break;
            _story.transform.Find("Episode/Dialog/DialogEnd").gameObject.SetActive(false);

            // �̸� ���̿� ���� �Ҽ� ǥ�� ��ġ ����
            if (_characterName.GetComponent<TMP_Text>().text.Length != _characterNameList[num].Length) // �̸� ���̰� ����Ǿ��� ���� ����
            {
                _departmentName.GetComponent<RectTransform>().anchoredPosition = new Vector2(-457 + ((_characterNameList[num].Length - 2) * 52), _departmentName.GetComponent<RectTransform>().anchoredPosition.y);
            }

            // ��� ���� �л��� �̹����� ���, ����ϰ� ���� ���� �л��� �̹����� ��Ӱ� ����
            if (_characterNameList[num] == "�����")
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
            else if (_characterNameList[num] == "�̵���")
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
            else if (_characterNameList[num] == "�Ƹ���")
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
            else if (_characterNameList[num] == "����")
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
            _dialog.GetComponent<TMP_Text>().text = "";
            _isStoryProgressing = true;
            float textSpeed = _textSpeedList[num];
            for (int i = 0; i < _dialogList[num].Length; i++)
            {
                _dialog.GetComponent<TMP_Text>().text += _dialogList[num][i];
                yield return new WaitForSeconds(0.05f / textSpeed);
            }
            _storyNum++;
            DoStoryJump(); // �������� ���� ���丮 ��ȣ �̵� üũ

            _isStoryProgressing = false;
            _story.transform.Find("Episode/Dialog/DialogEnd").gameObject.SetActive(true);
        }

        // ��ȭ ������ ���� �� Ŭ���ϸ� �� ���� ��µǵ��� ���ִ� �Լ�
        private void SetTextBox(int num)
        {
            if (_characterNameList.Count <= num) return;
            _characterName.GetComponent<TMP_Text>().text = _characterNameList[num];
            _departmentName.GetComponent<TMP_Text>().text = _departmentNameList[num];
            _dialog.GetComponent<TMP_Text>().text = _dialogList[num];
            _storyNum++;
            DoStoryJump(); // �������� ���� ���丮 ��ȣ �̵� üũ

            _isStoryProgressing = false;
            _story.transform.Find("Episode/Dialog/DialogEnd").gameObject.SetActive(true);
        }

        // ���丮�� �����Ű�� �Լ�
        public void DoStoryProgress()
        {
            if (!_canProgress)
            {
                return;
            }
            StartCoroutine(DoStoryAction(_storyNum));
        }

        // ���丮���� �ؽ�Ʈ�� ��Ÿ���� �Լ�
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

        // ���丮�� ó�� �������� �� �۵��ϴ� �Լ�
        public IEnumerator DoEpisodeStart()
        {
            _story.SetActive(true);
            _episodeStartWindowFadeOut.color = new Color(1, 1, 1, 0);
            _story.transform.Find("EpisodeStart").gameObject.SetActive(true);
            _story.transform.Find("Episode").gameObject.SetActive(false);
            StartCoroutine(_audioManager.FadeOutMusic());
            yield return new WaitForSeconds(6.5f);
            _episodeStartWindowFadeOut.DOFade(1, 2);

            // ������� ���丮 �ʱ� ����

            _dialog.GetComponent<TMP_Text>().fontSize = 42.5f;
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

            // ������� ���丮 �ʱ� ����

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