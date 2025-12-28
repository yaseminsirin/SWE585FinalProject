using UnityEngine;

public class OliveSpawner : MonoBehaviour
{
    [Header("References")]
    public Transform branchRoot;
    public Rigidbody branchRb;
    public GameObject stemPrefab;

    [Header("Spawn Settings")]
    public int oliveCount = 50; // DENEY 1: Bunu değiştirerek test et
    public float startX = -2.0f;
    public float endX = 2.0f;
    public float yOffset = -0.22f;

    [Header("Physics Experiment Settings")]
    public float stemBreakForce = 500f; // DENEY 3: Bunu değiştir
    
    // DENEY 4 İÇİN YENİ AYAR:
    public bool useGradientBreakForce = false; // Bunu açarsan aşağısı çalışır
    public float gradientStart = 10f; // İlk zeytin 10 güce sahip olsun
    public float gradientStep = 10f;  // Her zeytinde 10 artsın (10, 20, 30...)

    [Header("Randomness")]
    public float randomX = 0.03f;
    public float randomY = 0.01f;
    public float randomZ = 0.03f;

    void Start()
    {
        SpawnOlives();
    }

    public void SpawnOlives()
    {
        if (branchRoot == null || stemPrefab == null) return;
        if (branchRb == null) branchRb = branchRoot.GetComponent<Rigidbody>();

        // Temizlik
        for (int i = transform.childCount - 1; i >= 0; i--)
            Destroy(transform.GetChild(i).gameObject);

        for (int i = 0; i < oliveCount; i++)
        {
            float t = (oliveCount <= 1) ? 0.5f : (float)i / (oliveCount - 1);
            float x = Mathf.Lerp(startX, endX, t);

            Vector3 localPos = new Vector3(
                x + Random.Range(-randomX, randomX),
                yOffset + Random.Range(-randomY, randomY),
                Random.Range(-0.03f, 0.03f) // Z random
            );

            GameObject stem = Instantiate(stemPrefab, transform);
            stem.transform.position = branchRoot.TransformPoint(localPos);
            stem.transform.rotation = Quaternion.identity;

            // === HESAPLAMA (DENEY 4) ===
            float currentBreakForce = stemBreakForce; // Varsayılan sabit
            if (useGradientBreakForce)
            {
                // i=0 ise 10, i=1 ise 20, i=2 ise 30...
                currentBreakForce = gradientStart + (i * gradientStep);
            }

            // === BAĞLANTILAR ===
            var stemRb = stem.GetComponent<Rigidbody>();
            var stemJoint = stem.GetComponent<FixedJoint>();

            if (stemRb != null && stemJoint != null)
            {
                stemRb.useGravity = false;
                stemRb.isKinematic = false;
                stemJoint.connectedBody = branchRb;
                
                // Hesaplanan gücü uygula
                stemJoint.breakForce = currentBreakForce;
                stemJoint.breakTorque = currentBreakForce;
            }

            // Zeytin Ayarı (Sonsuz Güç - Stem ile birlikte düşsün diye)
            Transform oliveTr = stem.transform.Find("Olive_1");
            if (oliveTr != null)
            {
                var oliveJoint = oliveTr.GetComponent<FixedJoint>();
                if (oliveJoint != null)
                {
                    oliveJoint.connectedBody = stemRb;
                    oliveJoint.breakForce = Mathf.Infinity;
                    oliveJoint.breakTorque = Mathf.Infinity;
                }
            }
        }
    }
}