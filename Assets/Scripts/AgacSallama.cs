using UnityEngine;
using System.Collections;

public class AgacSallanma : MonoBehaviour
{
    // Ağacın ne kadar sert titneyeceği
    public float sarsintiGucu = 1.5f; 
    // Ne kadar süreceği (kısa olmalı, darbe gibi)
    public float sure = 0.2f;

    private Quaternion orijinalEgim;
    private bool suanSallaniyor = false;

    void Start()
    {
        // Başlangıç duruşunu kaydet ki sonra düzelsin
        orijinalEgim = transform.localRotation;
    }

    // Bu fonksiyonu dışarıdan çağıracağız
    public void VurusYap()
    {
        if (!suanSallaniyor)
        {
            StartCoroutine(Titret());
        }
    }

    IEnumerator Titret()
    {
        suanSallaniyor = true;
        float gecenSure = 0f;

        while (gecenSure < sure)
        {
            gecenSure += Time.deltaTime;
            
            // Rastgele sağa sola, öne arkaya sars
            float z = Random.Range(-1f, 1f) * sarsintiGucu;
            float x = Random.Range(-1f, 1f) * sarsintiGucu;

            // Ağacı hafifçe döndür
            transform.localRotation = orijinalEgim * Quaternion.Euler(x, 0, z);

            yield return null; // Bir sonraki kareyi bekle
        }

        // Titreme bitince ağacı eski dik haline getir
        transform.localRotation = orijinalEgim;
        suanSallaniyor = false;
    }
}