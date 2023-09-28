using RotaryHeart.Lib.SerializableDictionary;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

public class Weapon : MonoBehaviour
{
    [System.Serializable]
    public class WeaponAttributes
    {
        // TODO : We can serialize this to disk for save/load purposes if needed

        public UpgradeLevelMap UpgradeLevelMap { get; set; }

        public float BulletTravelSpeed { get; set; }

        public float AttackFormationAngleY { get; set; }
        public float BulletLifeTime { get; set; }
    }

    Transform _barrelTipTransform;
    [field: SerializeField] public WeaponConfigData WeaponConfigData { get; private set; }

    public WeaponAttributes WeaponAttributesProperty { get; private set; }

    public float FireRate => 1f / (float)WeaponAttributesProperty.UpgradeLevelMap[WeaponUpgradeType.FireRate].UpgradeValue;

    IObjectPool<Bullet> _bulletPool;
    Transform _levelParent;


    // Weapons with different behaviors can be derived from this script
    public virtual void Initialize(Transform shootingPoint, Transform levelParent)
    {
        _levelParent = levelParent;
        _bulletPool = new LinkedPool<Bullet>(CreateBullet, OnGetBullet, OnBulletReturnToPool, null, true, 20);
        _barrelTipTransform = shootingPoint;
        WeaponAttributesProperty = new WeaponAttributes
        {
            BulletTravelSpeed = WeaponConfigData.BulletTravelSpeed,
            AttackFormationAngleY = WeaponConfigData.AttackFormationAngleY,
            BulletLifeTime = WeaponConfigData.BulletLifeTime,
            UpgradeLevelMap = new UpgradeLevelMap
        {
            { WeaponUpgradeType.FireRate, new LevelUpgradeRateMap() {Level = 1, UpgradeValue =  WeaponConfigData.ConfigMap[WeaponUpgradeType.FireRate][1]} },
            { WeaponUpgradeType.BulletDamage, new LevelUpgradeRateMap() {Level = 1, UpgradeValue =  WeaponConfigData.ConfigMap[WeaponUpgradeType.BulletDamage][1]} },
            { WeaponUpgradeType.BouncingBullet, new LevelUpgradeRateMap() {Level = 1, UpgradeValue =  WeaponConfigData.ConfigMap[WeaponUpgradeType.BouncingBullet][1]} },
            { WeaponUpgradeType.AttackFormation, new LevelUpgradeRateMap() {Level = 1, UpgradeValue =  WeaponConfigData.ConfigMap[WeaponUpgradeType.AttackFormation][1]} }
        }
        };
    }

    #region BulletPool
    Bullet CreateBullet()
    {
        var bullet = Instantiate(WeaponConfigData.BulletPrefab);
        bullet.transform.SetParent(_levelParent);
        bullet.gameObject.SetActive(false);

        return bullet;
    }

    void OnGetBullet(Bullet bullet)
    {
        bullet.transform.position = _barrelTipTransform.position;
    }

    void OnBulletReturnToPool(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }

    #endregion

    public virtual void Fire()
    {
        for (int i = 0; i < WeaponAttributesProperty.UpgradeLevelMap[WeaponUpgradeType.AttackFormation].UpgradeValue; i++)
        {
            var bullet = _bulletPool.Get();

            var bulletDirection = _barrelTipTransform.forward;

            if (i > 0)
            {
                var randomAngle = Random.Range(-WeaponAttributesProperty.AttackFormationAngleY, WeaponAttributesProperty.AttackFormationAngleY);
                var eulerRandomAngle = Quaternion.Euler(0, randomAngle, 0);
                bulletDirection = eulerRandomAngle * _barrelTipTransform.forward;
            }

            bullet.Initialize(WeaponAttributesProperty.BulletTravelSpeed,
           WeaponAttributesProperty.UpgradeLevelMap[WeaponUpgradeType.BulletDamage].UpgradeValue,
           WeaponAttributesProperty.UpgradeLevelMap[WeaponUpgradeType.BouncingBullet].UpgradeValue,
           _bulletPool,
           this, bulletDirection,
           WeaponAttributesProperty.BulletLifeTime);
        }
    }

    public void UpgradeLevel(WeaponUpgradeType type)
    {
        var nextLevel = ++WeaponAttributesProperty.UpgradeLevelMap[type].Level;
        var nextLevelValueForAttribute = WeaponConfigData.ConfigMap[type][nextLevel];
        WeaponAttributesProperty.UpgradeLevelMap[type].UpgradeValue = nextLevelValueForAttribute;

        //Inject behavior change to bullet if necessarry
    }

    private void OnDestroy()
    {
        _bulletPool?.Clear();
        _bulletPool = null;
    }

    public List<WeaponUpgradeType> ReturnThreeUpgrades()
    {
        var availableUpgradeList = new List<WeaponUpgradeType>();
        foreach (var configKvP in WeaponConfigData.ConfigMap)
        {
            var currentSkillLevel = WeaponAttributesProperty.UpgradeLevelMap[configKvP.Key].Level;
            var upgradeableCount = configKvP.Value.Count - currentSkillLevel;

            for (int i = 0; i < upgradeableCount; i++)
            {
                availableUpgradeList.Add(configKvP.Key);
            }
        }

        var rnd = new System.Random();
        var randomized = availableUpgradeList.OrderBy(item => rnd.Next());
        var threeUpgrades = availableUpgradeList.Take(3).ToList();

        return threeUpgrades;
    }
}


[System.Serializable]
public class UpgradeLevelMap : SerializableDictionaryBase<WeaponUpgradeType, LevelUpgradeRateMap> { }

[System.Serializable]
public class LevelUpgradeRateMap
{
    [field: SerializeField] public int Level { get; set; }
    [field: SerializeField] public int UpgradeValue { get; set; }
}

public enum WeaponUpgradeType
{
    FireRate,
    BulletDamage,
    AttackFormation,
    BouncingBullet
}
