using UnityEngine;

public class SopaHareket : MonoBehaviour
{
    public OliveManager yonetici; 

    void Update()
    {
        // Mouse Kontrolü (Aynı kalıyor)
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 8f; 
        Vector3 hedefNokta = Camera.main.ScreenToWorldPoint(mousePos);
        transform.position = Vector3.Lerp(transform.position, hedefNokta, Time.deltaTime * 15f);
    }

    void OnCollisionEnter(Collision carpisma)
    {
        if (carpisma.gameObject.CompareTag("Branch"))
        {
            // 1. ELMALARI DÜŞÜR (Eski kod)
            if(yonetici != null)
            {
                yonetici.DaliHasatEt(carpisma.gameObject);
            }

            // 2. YENİ KISIM: AĞACI TİTRET!
            // Çarptığımız "Gizli Dal"ın, bağlı olduğu ana ağacı bulmaya çalışıyoruz
            // Gizli Dallar ağacın içinde değilse, sahnedeki ağacı direkt bulalım:
            AgacSallanma sarsici = FindObjectOfType<AgacSallanma>();
            
            if (sarsici != null)
            {
                sarsici.VurusYap();
            }
        }
    }
}