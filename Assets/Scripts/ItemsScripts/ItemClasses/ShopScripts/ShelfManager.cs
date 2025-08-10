using UnityEngine;
using System.Collections.Generic;

public class ShelfManager : MonoBehaviour
{
    [Header("��� ��������� 3D-������� ���������")]
    public List<GameObject> availableItems;

    [Header("����� ��� ��������� ��������� �� �����")]
    public List<Transform> spawnPoints;

    private void Start()
    {
        // �������� ������ ��� �������� �� ����������
        availableItems = ItemRegistry.Instance.GetAvailableItemsForShop();
        SpawnRandomItems();
    }

    void SpawnRandomItems()
    {
        List<GameObject> chosenPrefabs = new List<GameObject>();

        while (chosenPrefabs.Count < spawnPoints.Count)
        {
            GameObject randomPrefab = availableItems[Random.Range(0, availableItems.Count)];
            if (!chosenPrefabs.Contains(randomPrefab)) // ��� ����������
                chosenPrefabs.Add(randomPrefab);
            
        }

        for (int i = 0; i < spawnPoints.Count; i++)
        {
            GameObject randomItem= Instantiate(chosenPrefabs[i], spawnPoints[i].position, spawnPoints[i].rotation);
            randomItem.transform.SetParent(spawnPoints[i]);
        }
    }
}
