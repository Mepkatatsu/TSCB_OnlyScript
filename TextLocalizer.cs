using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class TextLocalizer : MonoBehaviour
{
    [Serializable]
    public class FontAnchoredPosition
    {
        public LocalizeManager.Language language;
        public float anchoredPositionX;
        public float anchoredPositionY;
        public float characterSpacing;
        public float lineSpacing;
    }

    public bool doNotLocalize;
    public bool textSetByCode;
    public string koreanText;
    public string japaneseText;
    public List<FontAnchoredPosition> fontAnchoredPositions = new List<FontAnchoredPosition>();

    private RectTransform _rectTransform;
    private TMP_Text _tmpText;
    private LocalizeManager.Language _previousLanguage;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _tmpText = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        LocalizeManager.Instance.AddFontLocalizer(this);
        DoLocalize(LocalizeManager.language);
    }

    private void OnDisable()
    {
        if (LocalizeManager.Instance)
            LocalizeManager.Instance.RemoveFontLocalizer(this);
    }

    public void DoLocalize(LocalizeManager.Language language)
    {
        if (doNotLocalize)
            return;
        
        if (_previousLanguage == language)
            return;
        _previousLanguage = language;
        
        if (!_rectTransform)
            _rectTransform = GetComponent<RectTransform>();
        
        if (!_tmpText)
            _tmpText = GetComponent<TMP_Text>();
        
        for (var i = 0; i < fontAnchoredPositions.Count; i++)
        {
            if (language != fontAnchoredPositions[i].language)
                continue;

            var anchoredPositionX = fontAnchoredPositions[i].anchoredPositionX;
            var anchoredPositionY = fontAnchoredPositions[i].anchoredPositionY;
            var anchoredPosition = _rectTransform.anchoredPosition;
            anchoredPosition = new Vector2(anchoredPosition.x + anchoredPositionX, anchoredPosition.y + anchoredPositionY);
            _rectTransform.anchoredPosition = anchoredPosition;

            _tmpText.characterSpacing = fontAnchoredPositions[i].characterSpacing;
            _tmpText.lineSpacing = fontAnchoredPositions[i].lineSpacing;
        }

        _tmpText.font = LocalizeManager.Instance.GetNewFont(_tmpText.font);
        
        if (language == LocalizeManager.Language.Japanese)
        {
            if (!textSetByCode && !string.IsNullOrEmpty(japaneseText))
                _tmpText.text = japaneseText;
        }
        
        if (language == LocalizeManager.Language.Korean)
        {
            if (!textSetByCode && !string.IsNullOrEmpty(koreanText))
                _tmpText.text = koreanText;
        }
    }

    public void SetLocalizeText()
    {
        if (!_tmpText)
            _tmpText = GetComponent<TMP_Text>();
        
        if (LocalizeManager.language == LocalizeManager.Language.Japanese)
        {
            if (!string.IsNullOrEmpty(japaneseText))
                _tmpText.text = japaneseText;
        }
        
        if (LocalizeManager.language == LocalizeManager.Language.Korean)
        {
            if (!string.IsNullOrEmpty(koreanText))
                _tmpText.text = koreanText;
        }
    }
}
