using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

[CreateAssetMenu()]
public class GamewideConfigData : ScriptableObject
{
    [field: SerializeField] public UpgradeTypeImageMap UpgradeTypeImageMap { get; set; }
}

[System.Serializable]
public class UpgradeTypeImageMap : SerializableDictionaryBase<WeaponUpgradeType, Texture2D> { }
