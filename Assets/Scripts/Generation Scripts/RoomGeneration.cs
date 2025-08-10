using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomGeneration : MonoBehaviour
{
    //public Room[] RoomPrefabs; // ������ ��� ���� ������ (1x1 � 2x2)
    //public Room StartingRoom; // ������ �������
    //public Room[] BossRoomPrefab; // ������� ������� �����
    //private Room[,] spawnedRooms; // ������ � ��� ������������ ���������

    //private List<Vector2Int> endpointRooms = new List<Vector2Int>(); // ������ �������� ������ (� ����� �������)

    //private IEnumerator Start()
    //{
    //    spawnedRooms = new Room[11, 11]; // ������������� ������� ��� ������
    //    spawnedRooms[5, 5] = StartingRoom; // ��������� ��������� ������� � ����� (������� 5, 5)

    //    for (int i = 0; i < 12; i++)
    //    {
    //        PlaceOneRoom(); // ����������� ������� � ����
    //        yield return new WaitForSecondsRealtime(0.5f);
    //    }

    //    ReplaceFarthestRoomWithBoss(); // �������� ����� ������� ������� �� ������� �����
    //}

    //private void PlaceOneRoom()
    //{
    //    // ��������� ������ ����
    //    HashSet<Vector2Int> vacantPlaces = new HashSet<Vector2Int>();

    //    // �������� �� ���� ������� �������
    //    for (int x = 0; x < spawnedRooms.GetLength(0); x++)
    //    {
    //        for (int y = 0; y < spawnedRooms.GetLength(1); y++)
    //        {
    //            if (spawnedRooms[x, y] == null) continue;

    //            int maxX = spawnedRooms.GetLength(0) - 1;
    //            int maxY = spawnedRooms.GetLength(1) - 1;

    //            // ��� ������� 2x2 ��������� 8 �������
    //            if (spawnedRooms[x, y].Width == 2 && spawnedRooms[x, y].Height == 2)
    //            {
    //                // �������� ��� ������� 2x2: ��������� ��� �������� ������
    //                if (x > 0 && y > 0 && spawnedRooms[x - 1, y + 1] == null) vacantPlaces.Add(new Vector2Int(x - 1, y + 1)); // ����� ������� �����
    //                if (x > 0 && spawnedRooms[x - 1, y] == null) vacantPlaces.Add(new Vector2Int(x - 1, y)); // ����� �����
    //                if (x > 0 && y < maxY && spawnedRooms[x, y - 1] == null) vacantPlaces.Add(new Vector2Int(x, y - 1)); // ����� ������ �����
    //                if (y > 0 && spawnedRooms[x, y + 2] == null) vacantPlaces.Add(new Vector2Int(x, y + 2)); // ������� �����

    //                if (y < maxY && spawnedRooms[x + 1, y - 1] == null) vacantPlaces.Add(new Vector2Int(x + 1, y - 1)); // ������ �����
    //                if (x < maxX && y > 0 && spawnedRooms[x + 1, y + 2] == null) vacantPlaces.Add(new Vector2Int(x + 1, y + 2)); // ������ ������� �����
    //                if (x < maxX && spawnedRooms[x + 2, y + 1] == null) vacantPlaces.Add(new Vector2Int(x + 2, y + 1)); // ������ �����
    //                if (x < maxX && y < maxY && spawnedRooms[x + 2, y] == null) vacantPlaces.Add(new Vector2Int(x + 2, y)); // ������ ������ �����
    //            }
    //            else
    //            {
    //                // ��� ������� 1x1, ����������� �������� �������
    //                if (x > 0 && spawnedRooms[x - 1, y] == null) vacantPlaces.Add(new Vector2Int(x - 1, y));
    //                if (y > 0 && spawnedRooms[x, y - 1] == null) vacantPlaces.Add(new Vector2Int(x, y - 1));
    //                if (x < maxX && spawnedRooms[x + 1, y] == null) vacantPlaces.Add(new Vector2Int(x + 1, y));
    //                if (y < maxY && spawnedRooms[x, y + 1] == null) vacantPlaces.Add(new Vector2Int(x, y + 1));
    //            }
    //        }
    //    }



    //    if (vacantPlaces.Count == 0) return;

    //    // �������� ��������� �������
    //    Room newRoom = Instantiate(RoomPrefabs[UnityEngine.Random.Range(0, RoomPrefabs.Length)]);
    //    int limit = 500;

    //    while (limit-- > 0)
    //    {
    //        Vector2Int position = vacantPlaces.ElementAt(UnityEngine.Random.Range(0, vacantPlaces.Count));
    //        newRoom.RotateRandomly();

    //        if (newRoom.Width == 2 && newRoom.Height == 2) // ��� ������ 2x2
    //        {
    //            // ���������, ����� �� ���������� ������� 2x2
    //            if (CanPlace2x2Room(position))
    //            {
    //                if (ConnectToSomething(newRoom, position))
    //                {
    //                    newRoom.transform.position = new Vector3(position.x - 5, 0, position.y - 5) * 9;
    //                    spawnedRooms[position.x, position.y] = newRoom;
    //                    spawnedRooms[position.x + 1, position.y + 1] = newRoom;
    //                    spawnedRooms[position.x + 1, position.y] = newRoom;
    //                    spawnedRooms[position.x, position.y + 1] = newRoom;
    //                    return;
    //                }
    //            }
    //        }
    //        else // ��� ������ 1x1
    //        {
    //            if (ConnectToSomething(newRoom, position))
    //            {
    //                newRoom.transform.position = new Vector3(position.x - 5, 0, position.y - 5) * 9;
                    
    //                spawnedRooms[position.x, position.y] = newRoom;
    //                return;
    //            }
    //        }
    //    }

    //    Destroy(newRoom.gameObject); // ���� �� ������� ����������, ���������� �������
    //}

    //private bool CanPlace2x2Room(Vector2Int position)
    //{
    //    // ���������, ��� ��� 4 ������ ��� ������� 2x2 ��������
    //    for (int x = 0; x < 2; x++)
    //    {
    //        for (int y = 0; y < 2; y++)
    //        {
    //            if (spawnedRooms[position.x + x, position.y + y] != null)
    //                return false;
    //        }
    //    }

    //    // �������������� ��������: ���������, ��� ����� �� �������������
    //    // �������� ��� 2x2 ������
    //    if (spawnedRooms[position.x, position.y] != null || spawnedRooms[position.x + 1, position.y] != null ||
    //        spawnedRooms[position.x, position.y + 1] != null || spawnedRooms[position.x + 1, position.y + 1] != null)
    //    {
    //        return false;
    //    }

    //    return true;
    //}

    //private bool ConnectToSomething(Room room, Vector2Int p)
    //{
    //    int maxX = spawnedRooms.GetLength(0) - 1;
    //    int maxY = spawnedRooms.GetLength(1) - 1;

    //    List<Vector2Int> neighbours = new List<Vector2Int>();

    //    // ��� ������ 1x1 � 2x2 ��������� �������
    //    if (room.Width == 1)
    //    {
    //        for (int x = 0; x < room.Width; x++)
    //        {
    //            for (int y = 0; y < room.Height; y++)
    //            {
    //                Vector2Int currentPosition = new Vector2Int(p.x + x, p.y + y);

    //                if (room.DoorU != null && currentPosition.y < maxY && spawnedRooms[currentPosition.x, currentPosition.y + 1]?.DoorD != null)
    //                    neighbours.Add(Vector2Int.up);
    //                if (room.DoorD != null && currentPosition.y > 0 && spawnedRooms[currentPosition.x, currentPosition.y - 1]?.DoorU != null)
    //                    neighbours.Add(Vector2Int.down);
    //                if (room.DoorR != null && currentPosition.x < maxX && spawnedRooms[currentPosition.x + 1, currentPosition.y]?.DoorL != null)
    //                    neighbours.Add(Vector2Int.right);
    //                if (room.DoorL != null && currentPosition.x > 0 && spawnedRooms[currentPosition.x - 1, currentPosition.y]?.DoorR != null)
    //                    neighbours.Add(Vector2Int.left);
    //            }
    //        }
    //    }
    //    if (room.Width == 2)
    //    {
    //        for (int x = 0; x < room.Width; x++)
    //        {
    //            for (int y = 0; y < room.Height; y++)
    //            {
    //                Vector2Int currentPosition = new Vector2Int(p.x + x, p.y + y);

    //                if (room.DoorU != null && currentPosition.y < maxY && spawnedRooms[currentPosition.x, currentPosition.y + 1]?.DoorD != null)
    //                    neighbours.Add(Vector2Int.up);
    //                if (room.DoorUup != null && currentPosition.y < maxY && spawnedRooms[currentPosition.x + 1, currentPosition.y + 1]?.DoorD != null)
    //                    neighbours.Add(Vector2Int.up);

    //                if (room.DoorD != null && currentPosition.y > 0 && spawnedRooms[currentPosition.x, currentPosition.y - 1]?.DoorU != null)
    //                    neighbours.Add(Vector2Int.down);
    //                if (room.DoorR != null && currentPosition.x < maxX && spawnedRooms[currentPosition.x + 1, currentPosition.y]?.DoorL != null)
    //                    neighbours.Add(Vector2Int.right);
    //                if (room.DoorL != null && currentPosition.x > 0 && spawnedRooms[currentPosition.x - 1, currentPosition.y]?.DoorR != null)
    //                    neighbours.Add(Vector2Int.left);
    //            }
    //        }
    //    }

    //    if (neighbours.Count == 0) return false;

    //    Vector2Int selectedDirection = neighbours[UnityEngine.Random.Range(0, neighbours.Count)];

    //    // �������� ������ �� ��������� �����������
    //    if (room.Width == 1)
    //    {
    //        if (selectedDirection == Vector2Int.up)
    //        {
    //            room.DoorU.SetActive(false);
    //            spawnedRooms[p.x, p.y + 1]?.DoorD.SetActive(false);
    //        }
    //        else if (selectedDirection == Vector2Int.down)
    //        {
    //            room.DoorD.SetActive(false);
    //            spawnedRooms[p.x, p.y - 1]?.DoorU.SetActive(false);
    //        }
    //        else if (selectedDirection == Vector2Int.right)
    //        {
    //            room.DoorR.SetActive(false);
    //            spawnedRooms[p.x + 1, p.y]?.DoorL.SetActive(false);
    //        }
    //        else if (selectedDirection == Vector2Int.left)
    //        {
    //            room.DoorL.SetActive(false);
    //            spawnedRooms[p.x - 1, p.y]?.DoorR.SetActive(false);
    //        }
    //    }
    //    if (room.Width == 2)
    //    {
    //        if (selectedDirection == Vector2Int.up)
    //        {
    //            room.DoorU.SetActive(false);
    //            spawnedRooms[p.x, p.y + 1]?.DoorD.SetActive(false);
    //        }
    //        else if (selectedDirection == Vector2Int.down)
    //        {
    //            room.DoorD.SetActive(false);
    //            spawnedRooms[p.x, p.y - 1]?.DoorU.SetActive(false);
    //        }
    //        else if (selectedDirection == Vector2Int.right)
    //        {
    //            room.DoorR.SetActive(false);
    //            spawnedRooms[p.x + 1, p.y]?.DoorL.SetActive(false);
    //        }
    //        else if (selectedDirection == Vector2Int.left)
    //        {
    //            room.DoorL.SetActive(false);
    //            spawnedRooms[p.x - 1, p.y]?.DoorR.SetActive(false);
    //        }
    //    }

    //    return true;
    //}

    //private void ReplaceFarthestRoomWithBoss()
    //{
    //    Vector2Int startRoomPosition = new Vector2Int(5, 5); // ������� ��������� �������
    //    Room farthestRoom = null;
    //    Vector2Int farthestRoomPosition = new Vector2Int(-1, -1); // ��� �������� ������� ����� ������� �������
    //    float maxDistance = -1; // ������������ ����������

    //    // �������� �� ���� ��������
    //    for (int x = 0; x < spawnedRooms.GetLength(0); x++)
    //    {
    //        for (int y = 0; y < spawnedRooms.GetLength(1); y++)
    //        {
    //            if (spawnedRooms[x, y] != null && new Vector2Int(x, y) != startRoomPosition)
    //            {
    //                // ��������� ���������� �� ��������� �������
    //                float distance = Vector2Int.Distance(startRoomPosition, new Vector2Int(x, y));

    //                // ���� ��� ����� ������� �������, ���������� � � � �������
    //                if (distance > maxDistance)
    //                {
    //                    maxDistance = distance;
    //                    farthestRoom = spawnedRooms[x, y];
    //                    farthestRoomPosition = new Vector2Int(x, y);
    //                }
    //            }
    //        }
    //    }

    //    // ���� ������� ����� ������� �������, �������� � �� ������� �����
    //    if (farthestRoom != null)
    //    {
    //        Room bossRoom = Instantiate(BossRoomPrefab[UnityEngine.Random.Range(0, BossRoomPrefab.Length)], new Vector3(farthestRoomPosition.x - 5, 0, farthestRoomPosition.y - 5) * 9, Quaternion.identity);
    //        int limit = 500;
    //        while (limit-- > 0)
    //        {
    //            bossRoom.RotateRandomly();

    //            if (ConnectToSomething(bossRoom, farthestRoomPosition))
    //            {
    //                spawnedRooms[farthestRoomPosition.x, farthestRoomPosition.y] = bossRoom;
    //                Destroy(farthestRoom.gameObject);
    //                return;
    //            }
    //        }
    //    }
    //}
}


