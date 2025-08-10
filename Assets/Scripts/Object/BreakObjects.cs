using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    public GameObject fracturedPrefab;
    private bool hasDropped = false;

    [System.Serializable]
    public class DropItem
    {
        public GameObject prefab;
        [Range(0f, 100f)] public float dropChance;
    }
    public DropItem[] dropItems; // список предметов с шансами
    
    public void Break()
    {
        GameObject fractured = Instantiate(fracturedPrefab, transform.position, transform.rotation);

        foreach (Rigidbody rb in fractured.GetComponentsInChildren<Rigidbody>())
        {
            rb.AddExplosionForce(150f, transform.position, 5f);
        }

        if (hasDropped) return; // защита от двойного вызова
        hasDropped = true;

        float roll = Random.Range(0f, 100f);
        float cumulative = 0f;

        foreach (DropItem item in dropItems)
        {
            cumulative += item.dropChance;
            if (roll <= cumulative && item.prefab != null)
            {
                Vector3 spawnPos = fractured ? fractured.transform.position : transform.position;
                Instantiate(item.prefab, spawnPos, Quaternion.identity);
                break;
            }
        }

        Destroy(gameObject);
        Destroy(fractured, 5f);
    }
}

