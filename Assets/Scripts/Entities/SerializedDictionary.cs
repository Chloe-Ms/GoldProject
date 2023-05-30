using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SerializedDictionary<TKey, TValue>
{
    [System.Serializable]
    public class OBJ
    {
        public TKey Key;
        public TValue Value;
    }

    public List<OBJ> dictionary = new List<OBJ>();
    public void Add(TKey key, TValue value)
    {
        OBJ obj = new OBJ();
        obj.Key = key;
        obj.Value = value;
        dictionary.Add(obj);
    }

    public bool Contains(TKey key)
    {
        int i = 0;
        bool found = false;
        while (i < dictionary.Count && !found)
        {
            found = dictionary[i].Key.ToString() == key.ToString();
            i++;
        }
        return found;
    }

    public TValue Get(TKey key)
    {
        int i = 0;
        bool found = false;
        TValue value = default(TValue);
        while (i < dictionary.Count && !found)
        {
            if (dictionary[i].Key.ToString() == key.ToString())
            {
                found = true;
                value = dictionary[i].Value;
            }
            i++;
        }
        return value;
    }
}
