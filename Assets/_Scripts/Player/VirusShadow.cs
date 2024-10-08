using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class VirusShadow : MonoBehaviour {
    public IObjectPool<VirusShadow> ParentShadowPool { get; set; }

    [SerializeField] private SpriteRenderer shadowRenderer;

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void SetPosition(Vector3 newPosition)
    {
        transform.position = newPosition;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void Deactivate(float fadeTime)
    {
        StartCoroutine(DeactivateCoroutine(fadeTime));
    }

    public IEnumerator DeactivateCoroutine(float fadeTime)
    {
        yield return new WaitForSeconds(fadeTime);

        ParentShadowPool.Release(this);
    }

    public void UpdateColorAlpha(float alpha)
    {
        alpha = Mathf.Clamp01(alpha);
        Color color = new Color(1f, 1f, 1f, alpha);
        shadowRenderer.color = color;
    }
}
