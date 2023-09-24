using RotaryHeart.Lib.SerializableDictionary;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

public class Weapon : MonoBehaviour
{
    [System.Serializable]
    public class WeaponAttributes
    {
        // TODO : We can serialize this to disk for save/load purposes if needed

        public UpgradeLevelMap UpgradeLevelMap { get; set; }

        public float BulletTravelSpeed { get; set; }
    }

    [SerializeField] Transform barrelTipTransform;
    [field: SerializeField] public WeaponConfigData WeaponConfigData { get; private set; }

    public WeaponAttributes WeaponAttributesProperty { get; private set; }


    IObjectPool<Bullet> _bulletPool;

    // Weapons with different behaviors can be derived from this script
    public virtual void Initialize()
    {
        _bulletPool = new LinkedPool<Bullet>(CreateBullet, OnGetBullet, OnBulletReturnToPool, OnBulletDestroy, true, 20);

        WeaponAttributesProperty = new WeaponAttributes
        {
            BulletTravelSpeed = 10f,
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
        bullet.transform.SetParent(transform);
        bullet.gameObject.SetActive(false);

        return bullet;
    }

    void OnGetBullet(Bullet bullet)
    {
        bullet.transform.SetParent(null);
        bullet.transform.position = barrelTipTransform.position;
        bullet.transform.forward = barrelTipTransform.forward; // TODO : change this when implementing attack formations

        bullet.Initialize(WeaponAttributesProperty.BulletTravelSpeed,
            WeaponAttributesProperty.UpgradeLevelMap[WeaponUpgradeType.BulletDamage].UpgradeValue,
            WeaponAttributesProperty.UpgradeLevelMap[WeaponUpgradeType.BouncingBullet].UpgradeValue,
            _bulletPool);
    }

    void OnBulletReturnToPool(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
        bullet.transform.SetParent(transform);
    }

    void OnBulletDestroy(Bullet bullet)
    {
        Destroy(bullet);
    }
    #endregion

    public virtual void Fire()
    {
        // Take bullet from pool and send it through barrelTipTransform
        var bullet = _bulletPool.Get();
        bullet.Fire();
    }

    public void UpgradeLevel(WeaponUpgradeType type)
    {
        var nextLevel = WeaponAttributesProperty.UpgradeLevelMap[type].Level++;
        var nextLevelValueForAttribute = WeaponConfigData.ConfigMap[type][nextLevel];
        WeaponAttributesProperty.UpgradeLevelMap[type].UpgradeValue = nextLevelValueForAttribute;

        //Inject behavior change to bullet if necessarry
    }
}


[System.Serializable]
public class UpgradeLevelMap : SerializableDictionaryBase<WeaponUpgradeType, LevelUpgradeRateMap> { }

[System.Serializable]
public class LevelUpgradeRateMap
{
    [field: SerializeField] public int Level { get;  set; }
    [field: SerializeField] public int UpgradeValue { get;  set; }
}

public enum WeaponUpgradeType
{
    FireRate,
    BulletDamage,
    AttackFormation,
    BouncingBullet
}
