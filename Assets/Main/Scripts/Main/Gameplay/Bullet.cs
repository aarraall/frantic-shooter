using PathCreation;
using System;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    [SerializeField] Transform _bulletAssetTransform;
    public float TravelSpeed {  get; private set; }
    public int Damage {  get; private set; }
    public int BounceAmount {  get; private set; }
    public float LifeTime {  get; private set; }

    float _currentLifeTime;

    IObjectPool<Bullet> _pool;

    private bool IsDead => _currentLifeTime <= 0;
    private int _currentBounceAmount = 0;
    Vector3 _currentAttackDirection;
    Weapon _weapon;



    public void Initialize(float travelSpeed, int damage, int bounceAmount, IObjectPool<Bullet> pool, Weapon weapon, Vector3 direction, float currentLifeTime)
    {
        TravelSpeed = travelSpeed;
        Damage = damage;
        BounceAmount = bounceAmount;
        LifeTime = currentLifeTime;
        _currentLifeTime = currentLifeTime;
        SetCurrentMoveDirection(direction);
        _currentBounceAmount = bounceAmount;

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

    private void SetCurrentMoveDirection(Vector3 direction)
    {
        _currentAttackDirection = direction;
        transform.forward = direction;
    }


    private void FrustumBounce()
    {
        if (_currentBounceAmount == 0)
        {
            return;
        }

        if (!FrustumService.Instance.IsIntersecting(transform.position, _bulletAssetTransform.localScale.y / 2, out var intersectionPoint))
        {
            return;
        }

        Bounce(intersectionPoint);
    }
    public void ObjectBounce(Vector3 intersectionPos)
    {
        if (_currentBounceAmount == 0)
        {
            return;
        }

        Bounce(intersectionPos);
    }

    private void Bounce(Vector3 intersectionPos)
    {
        var reflectionVector = Vector3.Reflect(intersectionPos, transform.forward);
        SetCurrentMoveDirection(reflectionVector.normalized);
        if (BounceAmount == -1)
        {
            _currentLifeTime += LifeTime / 2;
            return;
        }

        _currentBounceAmount--;
    }

    //Control movement from weapon instead of individual updates 
    private void Update()
    {
        Fire();
        FrustumBounce();
    }

  
}
