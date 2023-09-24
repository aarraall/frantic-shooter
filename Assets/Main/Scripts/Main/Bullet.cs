using PathCreation;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    [SerializeField] float initialLifeTime = 3.0f;
    public float TravelSpeed {  get; private set; }
    public int Damage {  get; private set; }
    public int BounceAmount {  get; private set; }

    float _currentLifeTime;

    IObjectPool<Bullet> _pool;

    private bool IsDead => _currentLifeTime <= 0;
    Vector3 _currentAttackDirection;


    public void Initialize(float travelSpeed, int damage, int bounceAmount, IObjectPool<Bullet> pool)
    {
        TravelSpeed = travelSpeed;
        Damage = damage;
        BounceAmount = bounceAmount;
        _currentLifeTime = initialLifeTime;

        gameObject.SetActive(true);
        _pool = pool;
        _currentAttackDirection = transform.forward;
    }

    public void Fire()
    {
        _currentLifeTime -= Time.deltaTime;
        if (IsDead)
        {
            // return to pool
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
        _pool.Release(this);
        _currentLifeTime = 0;
    }

    //Control movement from weapon instead of individual updates 
    private void Update()
    {
        Fire();
    }

}
