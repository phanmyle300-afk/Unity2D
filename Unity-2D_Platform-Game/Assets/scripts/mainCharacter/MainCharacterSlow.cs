using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterSlow : MonoBehaviour
{
    private MainCharacterController _mainCharacterController;
    private bool isSlowed = false; // Yavaþlatma etkisinin aktif olup olmadýðýný takip eder

    private void Start()
    {
        _mainCharacterController = GetComponent<MainCharacterController>();
        if (_mainCharacterController == null)
        {
            Debug.LogError("MainCharacterController bileþeni bulunamadý!");
        }
    }

    public void ApplySlowEffect(float slowMultiplier, float duration)
    {
        if (isSlowed) return; // Eðer yavaþlatma etkisi zaten aktifse, yeni bir etki uygulanmaz

        if (_mainCharacterController != null)
        {
            isSlowed = true; // Yavaþlatma etkisini aktif olarak iþaretle
            float originalMagnitude = _mainCharacterController.runVectorMagnitude;

            // Hýzý yavaþlat
            _mainCharacterController.runVectorMagnitude *= slowMultiplier;

            // Belirli bir süre sonra hýzý eski haline döndür
            StartCoroutine(ResetRunVectorMagnitudeAfterDelay(originalMagnitude, duration));
        }
    }

    private IEnumerator ResetRunVectorMagnitudeAfterDelay(float originalMagnitude, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (_mainCharacterController != null)
        {
            _mainCharacterController.runVectorMagnitude = originalMagnitude;
        }

        isSlowed = false; // Yavaþlatma etkisini sýfýrla
    }
}
