using UnityEngine;

public class ForestSpawner : MonoBehaviour
{
    public GameObject treePrefab; // Senin hazırladığın Ağaç Prefabı
    public int gridWidth = 5;     // 5x5 = 25 ağaç
    public int gridDepth = 5;
    public float spacing = 5.0f;  // Ağaçlar arası mesafe

    void Start()
    {
        if (treePrefab == null) return;

        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridDepth; z++)
            {
                Vector3 pos = new Vector3(x * spacing, 0, z * spacing);
                Instantiate(treePrefab, pos, Quaternion.identity, transform);
            }
        }
    }
}