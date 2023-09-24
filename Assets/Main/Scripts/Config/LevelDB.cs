using RotaryHeart.Lib.SerializableDictionary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class LevelDB : ScriptableObject
{
    [field: SerializeField] public SerializableDictionaryBase<int, LevelConfig> LevelConfigMap = new();
}
