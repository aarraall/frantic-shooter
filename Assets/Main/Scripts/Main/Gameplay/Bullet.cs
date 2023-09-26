using PathCreation;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    public float TravelSpeed {  get; private set; }
    public int Damage {  get; private set; }
    public int BounceAmount {  get; private set; }

    float _currentLifeTime;

    IObjectPool<Bullet> _pool;

    private bool IsDead => _currentLifeTime <= 0;
    Vector3 _currentAttackDirection;
    Weapon _weapon;


    public void Initialize(float travelSpeed, int damage, int bounceAmount, IObjectPool<Bullet> pool, Weapon weapon, Vector3 direction, float currentLifeTime)
    {
        TravelSpeed = travelSpeed;
        Damage = damage;
        BounceAmount = bounceAmount;
        _currentLifeTime = currentLifeTime;
        _currentAttackDirection = direction;

        gameObject.SetActive(true);
        _pool = pool;
        _weapon = weapon;
    }

    public void Fire()
    {
        _currentLifeTime -= Time.deltaTime;
        if (IsDead)
        {
            ReturnToPool();
            return;
        }

        transform.position += Time.deltaTime * TravelSpeed * _currentAttackDirection;
    }

    public void OnBounce()
    {
        BounceAmount--;
        if ( BounceAmount <= 0 )
        {
            ReturnToPool();
            return;
        }

        // calculate with vector3.reflect and set new direction for the bullet

    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("InvisibleWall"))
        { return; }

        OnBounce();
    }

    private void ReturnToPool()
    {
        if (_pool == null || _weapon == null)
        {
            Destroy(gameObject);
            return;
        }

        _pool.Release(this);
        _currentLifeTime = 0;
    }

    //Control movement from weapon instead of individual updates 
    private void Update()
    {
        Fire();
    }

}
