using System.Collections;
using UnityEngine;

public class SecretArea : MonoBehaviour
{
    public float fadeDuration = 1f;
    SpriteRenderer spriteRenderer;
    Color hiddenColor;
    Coroutine currentCoroutine;
    GameObject canvas;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        hiddenColor = spriteRenderer.color;
        canvas = transform.GetChild(0).gameObject;
        // canvas.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }
            currentCoroutine = StartCoroutine(FadeSprite(true));
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }
            currentCoroutine = StartCoroutine(FadeSprite(false));
        }
    }

    IEnumerator FadeSprite(bool fadeOut)
    {
        Color startColor = spriteRenderer.color;
        Color targetColor = fadeOut ? Color.white : hiddenColor;
        float timeFading = 0f;
        // canvas.SetActive(fadeOut);
        while (timeFading < fadeDuration)
        {
            spriteRenderer.color = Color.Lerp(startColor, targetColor, timeFading / fadeDuration);
            timeFading += Time.deltaTime;
            yield return null;
        }

        spriteRenderer.color = targetColor;
        
    }
}
