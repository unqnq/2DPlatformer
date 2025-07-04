using System.Collections;
using UnityEngine;
using TMPro;

public class SecretText : MonoBehaviour
{
    public float fadeDuration = 1f;
    TextMeshProUGUI tmpText;
    Color hiddenColor;
    Coroutine currentCoroutine;
    void Start()
    {
        tmpText = GetComponent<TextMeshProUGUI>();
        tmpText.overrideColorTags = true;
        hiddenColor = tmpText.color;
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
        Color startColor = tmpText.color;
        Color targetColor = fadeOut ?  new Color(hiddenColor.r, hiddenColor.g, hiddenColor.b, 100f): hiddenColor;
        float timeFading = 0f;

        while (timeFading < fadeDuration)
        {
            tmpText.color = Color.Lerp(startColor, targetColor, timeFading / fadeDuration);
            timeFading += Time.deltaTime;
            yield return null;
        }

        tmpText.color = targetColor;
    }

    void OnDestroy()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
    }
}
