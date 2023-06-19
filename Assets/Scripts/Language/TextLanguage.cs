using System;
using System.Collections.Generic;
using UnityEngine;

public enum Language
{
    FR,
    EN
}

[Serializable]
public class TextLanguage
{
    [SerializeField]
    SerializableDictionary<Language, string> _strings;

    public string GetStringInLanguage(Language language)
    {
        return _strings[language];
    }
}