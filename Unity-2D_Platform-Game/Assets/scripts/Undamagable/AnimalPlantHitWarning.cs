using UnityEngine;
using TMPro;
using System.Collections;

public class AnimalPlantHitWarning : MonoBehaviour
{
    public TextMeshProUGUI warningText;
    public GameObject warningPanel;

    private string[] messages = {
        "Hayir, bunu yapamam!",
        "Baska bir yolu olmali...",
        "Baska bir sey denemeliyim...",
        "Onlar yarinimiz...",
        "Onlar bizim dostumuz...",
        "Onlari iyilestirebilirim...",
        "Onlari korumaliyim...",
    };

    public float fadeDuration = 0.5f; // Fade in/out süresi
    public float showDuration = 2f;   // Ekranda kalma süresi

    public void ShowWarning()
    {
        int randomIndex = Random.Range(0, messages.Length);
        warningText.text = messages[randomIndex];
        StopAllCoroutines();
        StartCoroutine(FadeInOut());
    }

    private IEnumerator FadeInOut()
    {
        warningPanel.SetActive(true);
        warningText.gameObject.SetActive(true);
        SetAlpha(0f);

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / fadeDuration);
            SetAlpha(alpha);
            yield return null;
        }

        yield return new WaitForSeconds(showDuration);

        elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(1 - (elapsed / fadeDuration));
            SetAlpha(alpha);
            yield return null;
        }

        warningPanel.SetActive(false);
        warningText.gameObject.SetActive(false);
    }

    private void SetAlpha(float alpha)
    {
        Color color = warningText.color;
        color.a = alpha;
        warningText.color = color;
    }
}
