using UnityEngine;

[CreateAssetMenu]
public class LevelConfig : ScriptableObject
{
    [field: SerializeField] public int LevelID { get; set; }
    [field: SerializeField] public Level LevelPrefab { get; set; }
    [field: SerializeField] public float[] UpgradeTriggerPercentages { get; set; }
}


