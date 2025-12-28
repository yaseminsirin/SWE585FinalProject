using UnityEngine;

public class OliveDrop : MonoBehaviour
{
    [Header("Ayarlar")]
    // Zeytin yere düştüğünde hangi boyutta olsun?
    public Vector3 idealBoyut = new Vector3(0.3f, 0.4f, 0.3f); 
    
    private Rigidbody rb;
    private bool dropped = false; 

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Fizik açık olsun ama Joint tutsun
        rb.isKinematic = false; 
    }

    // Joint kırılınca otomatik çalışır
    void OnJointBreak(float breakForce)
    {
        Drop();
    }

    // İSMİNİ TEKRAR 'Drop' YAPTIK (Hata burada çözülüyor)
    public void Drop()
    {
        if (dropped) return; 
        dropped = true;

        // 1. Daldan Ayır
        transform.parent = null;

        // 2. Boyutu Düzelt
        transform.localScale = idealBoyut;

        // 3. Fiziği Sakinleştir (Uçmasın)
        rb.velocity = Vector3.zero;        
        rb.angularVelocity = Vector3.zero; 

        // 4. Düşüş Ayarları
        rb.drag = 1f; 
        rb.useGravity = true;
    }
}