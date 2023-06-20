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
        if (_strings == null || _strings.Count <= (int)language)
        {
            return "";
        }
        return _strings[(int)language];
    }
}

