using UnityEngine;

public class EarthquakeShake : MonoBehaviour
{
    [Header("Targets")]
    public Rigidbody treeRb;      // Ağacın Rigidbody'si
    public Rigidbody groundRb;    // YERİN Rigidbody'si (YENİ)

    [Header("Settings")]
    public float amplitude = 0.2f;   // Sallantı mesafesi
    public float frequency = 2.0f;   // Hız

    Vector3 treeStartPos;
    Vector3 groundStartPos;

    void Start()
    {
        // Başlangıç pozisyonlarını kaydet
        if (treeRb != null) treeStartPos = treeRb.position;
        
        if (groundRb != null) 
        {
            groundStartPos = groundRb.position;
        }
        else
        {
            Debug.LogWarning("EarthquakeShake: Lütfen Ground objesine Rigidbody ekleyip buraya atayın!");
        }
    }

    void FixedUpdate()
    {
        // Sinüs dalgası ile git-gel hareketi hesapla
        float offset = Mathf.Sin(Time.time * frequency * Mathf.PI * 2f) * amplitude;
        Vector3 moveVector = new Vector3(offset, 0f, 0f);

        // Ağacı salla (Fiziksel)
        if (treeRb != null)
            treeRb.MovePosition(treeStartPos + moveVector);

        // Yeri salla (Fiziksel - MovePosition sürtünmeyi tetikler)
        if (groundRb != null)
            groundRb.MovePosition(groundStartPos + moveVector);
    }
}