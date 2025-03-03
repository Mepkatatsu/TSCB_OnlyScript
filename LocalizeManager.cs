using System;
using System.Collections.Generic;
using SingletonPattern;
using TMPro;
using UnityEngine;

public class LocalizeManager : Singleton<LocalizeManager>
{
    public enum Language
    {
        Korean,
        Japanese
    }

    [Serializable]
    public class FontWithLanguage
    {
        public Language language;
        public TMP_FontAsset font;
    }

    public List<FontWithLanguage> normalFontList = new List<FontWithLanguage>();
    public List<FontWithLanguage> underlayFontList = new List<FontWithLanguage>();

    public static Language language = Language.Korean;

    private List<FontLocalizer> _fontLocalizerList = new List<FontLocalizer>();

    public override void Awake()
    {
        
    }

    public void ChangeLanguage(Language changeLanguage)
    {
        language = changeLanguage;
        
        if (StoryManager.Instance.ClearDialog())
            GameObject.Find("DialogManager").GetComponent<DialogManager>().InitializeDialog();

        for (var i = 0; i < _fontLocalizerList.Count; i++)
        {
            _fontLocalizerList[i].DoLocalize(language);
        }
    }

    public TMP_FontAsset GetNewFont(TMP_FontAsset fontAsset)
    {
        bool isNormalFont = false;
        
        for (var i = 0; i < normalFontList.Count; i++)
        {
            if (normalFontList[i].font == fontAsset)
            {
                isNormalFont = true;
                break;
            }
        }

        if (isNormalFont)
        {
            for (var i = 0; i < normalFontList.Count; i++)
            {
                if (normalFontList[i].language == language)
                    return normalFontList[i].font;
            }
        }
        else
        {
            for (var i = 0; i < underlayFontList.Count; i++)
            {
                if (underlayFontList[i].language == language)
                    return underlayFontList[i].font;
            }
        }

        return null;
    }

    public void AddFontLocalizer(FontLocalizer fontLocalizer)
    {
        _fontLocalizerList.Add(fontLocalizer);
    }
    
    public void RemoveFontLocalizer(FontLocalizer fontLocalizer)
    {
        _fontLocalizerList.Remove(fontLocalizer);
    }
}
