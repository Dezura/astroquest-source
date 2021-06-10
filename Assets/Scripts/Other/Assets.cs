using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;

public class Assets : MonoBehaviour
{
    public SerializableDictionaryBase<string, GameObject> enemies = new SerializableDictionaryBase<string, GameObject> ();

    public SerializableDictionaryBase<string, SerializableDictionaryBase<string, GameObject>> projectiles = new SerializableDictionaryBase<string, SerializableDictionaryBase<string, GameObject>> 
    {
        {"Player", new SerializableDictionaryBase<string,GameObject>()},
        {"Enemy", new SerializableDictionaryBase<string,GameObject>()}
    };

    public SerializableDictionaryBase<string, SerializableDictionaryBase<string, GameObject>> gunModels = new SerializableDictionaryBase<string, SerializableDictionaryBase<string, GameObject>> 
    {
        {"Player", new SerializableDictionaryBase<string,GameObject>()},
        {"Enemy", new SerializableDictionaryBase<string,GameObject>()}
    };
}
