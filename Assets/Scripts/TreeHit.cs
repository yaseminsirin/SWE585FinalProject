using UnityEngine;

public class TreeHit : MonoBehaviour
{
    private OliveDrop[] olives;

    void Start()
    {
        // Tree altındaki tüm zeytinleri bul
        olives = GetComponentsInChildren<OliveDrop>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Tree hit by: " + collision.gameObject.name);

        if (collision.gameObject.CompareTag("Stick"))
        {
            Debug.Log("STICK TEMAS ETTİ — ZEYTİNLER DÜŞECEK");

            foreach (var olive in olives)
            {
                olive.Drop();
            }
        }
    }
}
