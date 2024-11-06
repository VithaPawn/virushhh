using UnityEngine;

public class VirusVisual : MonoBehaviour {
    private SpriteRenderer spriteRenderer;
    private Color defaultColor;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultColor = spriteRenderer ? spriteRenderer.color : Color.red;
    }

    public void RestoreColor()
    {
        spriteRenderer.color = defaultColor;
    }

    public void ChangeColorWhileDashing()
    {
        spriteRenderer.color = Color.green;
    }
}
