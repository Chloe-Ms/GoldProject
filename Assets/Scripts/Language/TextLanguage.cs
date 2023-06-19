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
    [SerializeField] List<string> _strings;

    public string GetStringInLanguage(Language language)
    {
        return _strings[(int)language];
    }
}

