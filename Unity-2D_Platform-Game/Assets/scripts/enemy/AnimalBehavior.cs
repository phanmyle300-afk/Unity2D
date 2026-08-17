using UnityEngine;

public class AnimalBehavior : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool isAggressive = true; // Hayvanın saldırgan olup olmadığını kontrol eder
    private bool isVaccinated = false; // Hayvanın aşılanıp aşılanmadığını kontrol eder

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void StopBehavior()
    {
        if (!isVaccinated)
        {
            isVaccinated = true; // Hayvanın aşılandığını işaretle
            isAggressive = false; // Saldırganlığı durdur

            if (rb != null)
            {
                rb.velocity = Vector2.zero; // Hareketi durdur
                rb.isKinematic = true; // Fiziksel etkileşimleri devre dışı bırak
            }

            // İsteğe bağlı: Hayvanın görsel veya animasyon durumunu değiştir
            GetComponent<Animator>()?.SetTrigger("Vaccinated");
        }
    }

    private void Update()
    {
        if (isAggressive && !isVaccinated)
        {
            // Hayvanın hareket ve saldırı davranışlarını burada tanımlayabilirsiniz
            Patrol();
        }
    }

    private void Patrol()
    {
        // Hayvanın devriye gezme davranışı
        // Örneğin, sağa sola hareket etme
    }
}