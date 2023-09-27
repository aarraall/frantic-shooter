using DG.Tweening;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [field: SerializeField] public MeshRenderer Renderer { get; private set; }

    [field: SerializeField] public int Health {  get; set; }

    [field: SerializeField] public Collider Collider { get; private set; }

    private void OnTriggerEnter(Collider other)
    {
        if (other == null) return;

        if (other.CompareTag("Bullet"))
        {
            var bullet = other.attachedRigidbody.GetComponent<Bullet>();
            var bulletDamage = bullet.Damage;
            TakeDamage(bulletDamage);
            bullet.ObjectBounce(Collider.ClosestPointOnBounds(bullet.transform.position));
            return;
        }

        var player = other.attachedRigidbody.GetComponent<PlayerController>();

        player.Die();
    }

    private void TakeDamage(int damage)
    {
        Health -= damage;
        Renderer.material
            .DOColor(Color.red, .2f)
            .SetLoops(2, LoopType.Yoyo)
            .SetEase(Ease.OutBack);

        transform.DOShakeScale(.2f);

        if (Health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // TODO : add particles etc
        Destroy(gameObject);
    }

    private void Reset()
    {
        Renderer = GetComponent<MeshRenderer>();
    }
}
