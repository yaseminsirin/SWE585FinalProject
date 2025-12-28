using UnityEngine;

public class TreeShaker : MonoBehaviour
{
    public Rigidbody myTreeRb; // Ağacın kendi Rigidbody'si
    
    // Deney için bunları Inspector'dan veya CSV Logger'dan değiştirebilirsin
    public float amplitude = 0.2f; 
    public float frequency = 2.0f; 

    Vector3 startPos;
    bool isReady = false;

    void Start()
    {
        if (myTreeRb == null) 
            myTreeRb = GetComponent<Rigidbody>();

        if (myTreeRb != null)
        {
            startPos = myTreeRb.position;
            isReady = true;
        }
    }

    void FixedUpdate()
    {
        if (!isReady) return;

        // Sinüs dalgası ile sallanma
        float offset = Mathf.Sin(Time.time * frequency * Mathf.PI * 2f) * amplitude;
        Vector3 moveVector = new Vector3(offset, 0f, 0f);

        // Ağacı olduğu yerde salla (Kinematic hareket)
        myTreeRb.MovePosition(startPos + moveVector);
    }
}