using UnityEngine;
using System.Collections.Generic;


public class MergedRoomController : MonoBehaviour
{
    private List<RoomController> childRooms = new List<RoomController>();
    private int totalEnemies = 0;
    private int defeatedEnemies = 0;
    private bool victoryTriggered = false;

    private void Start()
    {
        // �������� ��� �������� RoomController
        childRooms.AddRange(GetComponentsInChildren<RoomController>());
    }

    public void SpawnEnemiesInAllRooms()
    {
        totalEnemies = 0;
        defeatedEnemies = 0;
        victoryTriggered = false;

        foreach (var room in childRooms)
        {
            room.SetMergedRoom(this);
            room.SpawnEnemies();
           
        }
    }

    public void OnEnemyDefeated()
    {
        defeatedEnemies++;
        Debug.Log($"MergedRoom: ���� ������� ({defeatedEnemies}/{totalEnemies})");

        if (!victoryTriggered && defeatedEnemies >= totalEnemies)
        {
            victoryTriggered = true;
            OnAllEnemiesDefeated();
        }
    }

    private void OnAllEnemiesDefeated()
    {
        Debug.Log("��� ����� � ����������� ������� ���������!");

        foreach (var room in childRooms)
        {
            room.OpenDoors(); // ������������ �������� ������ � ������ �������
        }
    }
    public void RegisterEnemyCount(int count)
    {
        totalEnemies += count;
        Debug.Log($"MergedRoom ��������������� {count} ������. ����� ������: {totalEnemies}");
    }

}

