using DG.Tweening;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour {
    [SerializeField] private IntegerVariableSO killCountBeforeDashSO;

    private SpriteRenderer spriteRenderer;
    private bool wasDamagedRecently;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        wasDamagedRecently = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HandleCollideWithVirus(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        HandleCollideWithVirus(collision);
    }

    private void HandleCollideWithVirus(Collider2D collision)
    {
        if (collision.TryGetComponent(out Virus virus) && virus.IsTouchable && !wasDamagedRecently)
        {
            killCountBeforeDashSO.IncreaseValue(1);
            StartCoroutine(BeDamaged());
        }
    }

    private IEnumerator BeDamaged()
    {
        wasDamagedRecently = true;
        spriteRenderer.DOColor(Color.red, 0.1f);

        yield return new WaitForSeconds(0.1f);
        spriteRenderer.DOColor(Color.white, 2f);

        yield return new WaitForSeconds(2f);
        wasDamagedRecently = false;
    }
}
