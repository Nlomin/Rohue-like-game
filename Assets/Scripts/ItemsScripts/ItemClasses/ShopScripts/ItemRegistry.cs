using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ItemRegistry : MonoBehaviour
{
    public static ItemRegistry Instance { get; private set; }

    [Header("Все доступные префабы предметов")]
    public List<GameObject> allItemPrefabs = new List<GameObject>();

    private GameObject reservedForRoom; // Предмет, выбранный в подземелье

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ReserveItemForDungeonRoom(GameObject itemPrefab)
    {
        reservedForRoom = itemPrefab;
    }

    public List<GameObject> GetAvailableItemsForShop()
    {
        return allItemPrefabs.Where(item => item != null && item != reservedForRoom).ToList();
    }
}

