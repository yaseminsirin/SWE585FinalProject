using UnityEngine;

public class CupManager : MonoBehaviour
{
    [Header("Sürükle Bırak")]
    public GameObject kahvePrefab; // Hazırladığın küçük top
    public GameObject kirikPrefab; // İndirdiğin kırık obje
    
    [Header("Ayarlar")]
    public int kahveSayisi = 100; // Kaç tane dolsun?
    public float kirilmaHizi = 3f; // Ne kadar sert çarparsa kırılsın?

    private Rigidbody rb;
    private bool tutuyorMuyum = false;
    private Vector3 mouseOffset;
    private float mZCoord;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        IciniDoldur();
    }

    void IciniDoldur()
    {
        // Kupanın azıcık üzerinden içine doğru yağdırıyoruz
        Vector3 baslangicYeri = transform.position + Vector3.up * 1.5f; 
        
        for (int i = 0; i < kahveSayisi; i++)
        {
            Vector3 rastgeleKonum = baslangicYeri + new Vector3(Random.Range(-0.1f, 0.1f), i * 0.05f, Random.Range(-0.1f, 0.1f));
            Instantiate(kahvePrefab, rastgeleKonum, Quaternion.identity);
        }
    }

    // --- MOUSE İLE TUTMA ---
    void OnMouseDown()
    {
        mZCoord = Camera.main.WorldToScreenPoint(transform.position).z;
        mouseOffset = transform.position - GetMouseWorldPos();
        tutuyorMuyum = true;
        rb.isKinematic = true; // Fiziği dondur, ele yapışsın
    }

   // --- BU KISMI GÜNCELLE ---
    void OnMouseDrag()
    {
        // 1. Mouse'un gösterdiği hedef noktayı al
        Vector3 hedefNokta = GetMouseWorldPos() + mouseOffset;

        // 2. YÜKSEKLİK SINIRI (LIMIT): 
        // Bardağın Y pozisyonu asla "0.5f" değerinin altına inemesin.
        // NOT: Kendi masanın yüksekliğine göre bu 0.5 sayısını değiştirebilirsin.
        if (hedefNokta.y < 0.2f) 
        {
            hedefNokta.y = 0.2f; 
        }

        // 3. Pozisyonu güncelle
        transform.position = hedefNokta;
    }

    void OnMouseUp()
    {
        tutuyorMuyum = false;
        rb.isKinematic = false; // Fiziği aç, düşsün
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = mZCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    // --- KIRILMA ---
   void OnCollisionEnter(Collision carpisma)
    {
        // --- BU KISIM KONSOLA RAPOR VERİR ---
        // 1. Neye çarptık?
        string carpanSey = carpisma.gameObject.name;
        // 2. Hızımız kaç?
        float hiz = carpisma.relativeVelocity.magnitude;
        
        Debug.Log("ÇARPIŞMA VAR! --> Çarpan: " + carpanSey + " | Hız: " + hiz + " | Tutuyor muyum?: " + tutuyorMuyum);
        // ------------------------------------

        // KONTROL: Tutmuyorum VE Hız limiti geçti VE Çarpan şey kahve değil
        if (!tutuyorMuyum && hiz > kirilmaHizi && !carpisma.gameObject.name.Contains("Kahve"))
        {
            Debug.Log("TÜM ŞARTLAR TAMAM! KIRILIYORUM...");
            Kiril();
        }
        else
        {
             Debug.Log("KIRILMADIM ÇÜNKÜ BİR ŞART TUTMADI.");
        }
    }

    void Kiril()
    {
        // Eğer Kırık Prefab atanmadıysa hata vermesin, uyarsın
        if (kirikPrefab == null) 
        {
            Debug.LogError("HATA: Inspector'da 'Kirik Prefab' kutusu boş! Oraya prefabı sürükle.");
            return;
        }

        GameObject kirik = Instantiate(kirikPrefab, transform.position, transform.rotation);
        
        Rigidbody[] parcalar = kirik.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody r in parcalar)
        {
            r.AddExplosionForce(150f, transform.position, 1f);
        }

        Destroy(gameObject);
    }
}