using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField] private List<TKey> _keys = new List<TKey>();
    [SerializeField] private List<TValue> _values = new List<TValue>();
    public void OnAfterDeserialize()
    {
        this.Clear();
        if (_keys.Count != _values.Count)
        {
            Debug.LogError("Tried to deserialize a SerializableDictionary with an amount of keys ("
                + _keys.Count + ") and of values(" + _values.Count + ") different.");
        }

        for (int i = 0 ; i < _keys.Count; i++)
        {
            this.Add(_keys[i], _values[i]);
        }
    }

    public void OnBeforeSerialize()
    {
        _keys.Clear();
        _values.Clear();
        foreach(KeyValuePair<TKey, TValue> pair in this)
        {
            _keys.Add(pair.Key);
            _values.Add(pair.Value);
        }
    }
}
