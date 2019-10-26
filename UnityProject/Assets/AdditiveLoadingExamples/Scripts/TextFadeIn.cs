using UnityEngine;
using TMPro;

public class TextFadeIn : MonoBehaviour
{
    private TextMeshProUGUI textMeshPro;
    public float startAlpha;
    public float targetAlpha;
    public float fadeSpeed;
    private float currentAlpha;


    private void OnEnable()
    {
        currentAlpha = startAlpha;
        textMeshPro = gameObject.GetComponent<TextMeshProUGUI>() ?? gameObject.AddComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        currentAlpha = Mathf.Lerp(currentAlpha, targetAlpha, fadeSpeed * Time.deltaTime);
        textMeshPro.alpha = currentAlpha;
    }
}