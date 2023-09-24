using RotaryHeart.Lib.SerializableDictionary;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(WeaponConfigData), menuName = "Config/WeaponConfigData")]
public class WeaponConfigData : ScriptableObject
{
    /// <summary>
    /// This is simply a map of upgrade paths. We give type and get Level & UpgradedValue (Fire rate, bullet damage etc.) map in return. 
    /// Id on editor represents Upgrade Level
    /// Value on editor represents Upgraded Value
    /// 
    /// 
    /// Bullets can bounce unlimited amount of time
    /// -1 will represent unlimited in this map
    /// </summary>
    [field: SerializeField] public UpgradeLevelConfigMap ConfigMap { get; private set; }

    [field: SerializeField] public WeaponPositionData WeaponPositionDataProperty { get; private set; }
    [field: SerializeField] public WeaponHolderRigData WeaponHolderRigDataProperty { get; private set; }

}

[System.Serializable]
public class UpgradeLevelConfigMap : SerializableDictionaryBase<WeaponUpgradeType, SerializableDictionaryBase<int,int>> { }

[System.Serializable]
public struct WeaponPositionData
{
    [field: SerializeField] public Vector3 Position { get; private set; }
    [field: SerializeField] public Vector3 RotationEuler { get; private set; }
}
[System.Serializable]
public struct WeaponHolderRigData
{
    [field: SerializeField] public Vector3 LeftHandIKTargetPosition { get; private set; }
    [field: SerializeField] public Vector3 LeftHandIKTargetRotationEuler { get; private set; }
    [field: SerializeField] public Vector3 RightHandIKTargetPosition { get; private set; }
    [field: SerializeField] public Vector3 RightHandIKTargetRotationEuler { get; private set; }
}