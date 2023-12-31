using UnityEngine;

[CreateAssetMenu(fileName = nameof(WeaponConfigData), menuName = "Config/LevelConfig")]

public class LevelConfig : ScriptableObject
{
    [field: SerializeField] public int LevelID { get; set; }
    [field: SerializeField] public Level LevelPrefab { get; set; }
}


