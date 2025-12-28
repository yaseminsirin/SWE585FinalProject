using UnityEngine;

public class StickController : MonoBehaviour
{
    public Camera cam;
    public float distanceFromCamera = 0f; // Sopanın kameradan uzaklığı

    void Update()
    {
        Vector3 mousePos = Input.mousePosition;

        // Mouse koordinatını 3D dünyaya çevir
        mousePos.z = distanceFromCamera;

        Vector3 worldPos = cam.ScreenToWorldPoint(mousePos);

        // Stick'i tam 3D konuma taşı
        transform.position = worldPos;
    }
}
