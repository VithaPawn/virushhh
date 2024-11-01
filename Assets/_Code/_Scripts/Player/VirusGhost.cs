using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class VirusGhost : MonoBehaviour {
    public IObjectPool<VirusGhost> ParentShadowPool { get; set; }

    private SpriteRenderer ghostRenderer;
    private float fadeDuration;
    private float ghostAlpha;

    private void Awake()
    {
        ghostRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        ghostRenderer.DOFade(0, fadeDuration);
    }

    private void OnDisable()
    {
        UpdateColorAlpha(ghostAlpha);
    }

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
        Color newColor = ghostRenderer.color;
        newColor.a = alpha;
        ghostRenderer.color = newColor;
    }

    public void SetGhostAlpha (float alpha) { ghostAlpha = alpha; }

    public void UpdateVisual(Sprite sprite)
    {
        ghostRenderer.sprite = sprite;
    }

    public void UpdateScale(Vector3 scale)
    {
        transform.localScale = scale;
    }

    public void SetFadeDuration(float duration)
    {
        fadeDuration = duration;
    }
}
