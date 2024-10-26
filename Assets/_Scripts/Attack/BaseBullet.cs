using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class BaseBullet : MonoBehaviour {
    [SerializeField] protected BulletSO bulletSO;

    public IObjectPool<BaseBullet> ParentBulletPool { get; set; }
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Show() { gameObject.SetActive(true); }

    public void Hide() { gameObject.SetActive(false); }

    public void DeactivateAfterSeconds()
    {
        StartCoroutine(DeactivateAfterSecondsCoroutine());
    }

    public IEnumerator DeactivateAfterSecondsCoroutine()
    {
        yield return new WaitForSeconds(bulletSO.GetExistTime());
        Deactive();
    }

    private void Deactive()
    {
        rb.velocity = Vector3.zero;
        transform.position = Vector3.zero;
        ParentBulletPool.Release(this);
    }

    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    public void MoveTowardDirection(Vector3 movingDirection)
    {
        Vector2 flying2dDirection = new Vector2(movingDirection.x, movingDirection.y);
        rb.AddForce(flying2dDirection * bulletSO.GetFlyingForce(), ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Virus virus))
        {
            Deactive();
        }
    }

}
