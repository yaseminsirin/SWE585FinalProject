using UnityEngine;

public class SopaKontrol : MonoBehaviour
{
    // Kameranın ağaca olan uzaklığı (Genelde kamera Z=-10 ise bu 10 olmalı)
    public float derinlik = 10f; 

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // 1. Mouse'un ekrandaki (2D) yerini al
        Vector3 mousePos = Input.mousePosition;
        
        // 2. Derinlik bilgisini ekle (Z ekseni)
        mousePos.z = derinlik;

        // 3. Bunu dünya koordinatlarına (3D) çevir
        Vector3 hedefYer = Camera.main.ScreenToWorldPoint(mousePos);

        // 4. Sopayı oraya fiziksel olarak taşı (MovePosition kullanıyoruz ki çarpışma algılansın)
        rb.MovePosition(hedefYer);
    }
}