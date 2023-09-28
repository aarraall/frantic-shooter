using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(WeaponConfigData), menuName = "Config/LevelConfig")]
public class GamewideConfigData : ScriptableObject
{
    [field: SerializeField] public UpgradeTypeImageMap UpgradeTypeImageMap { get; set; }
}

[System.Serializable]
public class UpgradeTypeImageMap : SerializableDictionaryBase<WeaponUpgradeType, Texture2D> { }
