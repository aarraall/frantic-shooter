using DG.Tweening;
using TMPro;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [field: SerializeField] public MeshRenderer Renderer { get; private set; }

    public int Health {  get; set; }

    [field: SerializeField] public Collider Collider { get; private set; }

    [field: SerializeField] public TMP_Text HPText{ get; private set; }

    private void Awake()
    {
        Health = Random.Range(20, 50);
        HPText.text = Health.ToString();
    }

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
        Renderer.material.DOKill(true);
        transform.DOKill(true);

        if (Renderer == null || transform == null)
            return;

        Renderer.material
            .DOColor(Color.yellow, .2f)
            .SetLoops(2, LoopType.Yoyo)
            .SetEase(Ease.OutBack);

        transform.DOShakeScale(.2f, .5f, 5);

        HPText.text = Health.ToString();


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
