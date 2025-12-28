using UnityEngine;
using System.Collections.Generic;

public class OliveManager : MonoBehaviour
{
    [Header("Ayarlar")]
    public GameObject olivePrefab; // Zeytin kalıbı
    public int olivesPerBranch = 20; // Dal başına kaç zeytin
    
    // Hangi dalda hangi zeytinler var, listesini tutuyoruz
    private Dictionary<GameObject, List<GameObject>> dalZeytinListesi = new Dictionary<GameObject, List<GameObject>>();

    void Start()
    {
        ZeytinleriDiz();
    }

    void ZeytinleriDiz()
    {
        // Tag'i "Branch" olan tüm dalları bul
        GameObject[] dallar = GameObject.FindGameObjectsWithTag("Branch");

        foreach (GameObject dal in dallar)
        {
            List<GameObject> buDaldakiZeytinler = new List<GameObject>();
            
            // Dalın boyunu ve yönünü al
            CapsuleCollider dalCollider = dal.GetComponent<CapsuleCollider>();
            float boy = dalCollider.height * dal.transform.lossyScale.y;
            Vector3 merkez = dal.transform.position;
            Vector3 yukari = dal.transform.up;

            for (int i = 0; i < olivesPerBranch; i++)
            {
                // 1. POZİSYON (Sarmal Dizilim - Çakışmayı Önler)
                float t = (float)i / olivesPerBranch;
                float yükseklikAyari = Mathf.Lerp(-boy / 2.2f, boy / 2.2f, t);
                
                // Dal etrafında döndür
                Quaternion donme = Quaternion.AngleAxis(i * 45f, yukari);
                Vector3 yon = donme * dal.transform.forward;
                
                // Daldan biraz uzakta doğsun
                Vector3 dogumYeri = merkez + (yukari * yükseklikAyari) + (yon * 0.4f);

                // 2. YARATMA
                GameObject yeniZeytin = Instantiate(olivePrefab, dogumYeri, Quaternion.identity);
                
                // 3. BAĞLAMA (FixedJoint)
                FixedJoint baglanti = yeniZeytin.AddComponent<FixedJoint>();
                baglanti.connectedBody = dal.GetComponent<Rigidbody>();

                // 4. ÇOK ÖNEMLİ: Dal ve Zeytin birbirine çarpmasın (Titremeyi bitiren kod)
                Physics.IgnoreCollision(yeniZeytin.GetComponent<Collider>(), dalCollider, true);

                buDaldakiZeytinler.Add(yeniZeytin);
            }

            // Listeye kaydet
            dalZeytinListesi.Add(dal, buDaldakiZeytinler);
        }
    }

    // Sopa dala vurunca bu çalışacak
    public void DaliHasatEt(GameObject vurulanDal)
    {
        if (dalZeytinListesi.ContainsKey(vurulanDal))
        {
            List<GameObject> zeytinler = dalZeytinListesi[vurulanDal];
            
            foreach (GameObject zeytin in zeytinler)
            {
                if (zeytin != null)
                {
                    // Bağlantıyı kopar (Düşsünler)
                    Destroy(zeytin.GetComponent<FixedJoint>());

                    // Hafif salla
                    Rigidbody rb = zeytin.GetComponent<Rigidbody>();
                    if (rb) 
                    {
                        rb.WakeUp();
                        rb.AddForce(Random.insideUnitSphere * 2f, ForceMode.Impulse);
                    }
                }
            }
            // Listeyi temizle ki tekrar düşürmeye çalışmayalım
            dalZeytinListesi[vurulanDal].Clear();
        }
    }
}