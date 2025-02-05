using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class FlashingColor : MonoBehaviour
{
    public float flashInterval = 0.1f;
    public bool flashOnStart = true;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    public Color targetColor;
    private Coroutine flashCoroutine;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("FlashingColor component requires a SpriteRenderer.");
        }
        originalColor = spriteRenderer.color;
    }

    private void OnEnable()
    {
        if (flashOnStart)
        {
            StartFlashing();
        }
    }

    private void OnDisable()
    {
        StopFlashing();
    }

    private void OnDestroy()
    {
        StopFlashing();
    }

    public void StartFlashing()
    {
        if (flashCoroutine == null)
        {
            flashCoroutine = StartCoroutine(Flash());
        }
    }

    public void StopFlashing()
    {
        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
            flashCoroutine = null;
            spriteRenderer.color = originalColor;
        }
    }

    private IEnumerator Flash()
    {
        while (true)
        {
            spriteRenderer.color = targetColor;
            yield return new WaitForSeconds(flashInterval);
            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(flashInterval);
        }
    }
}