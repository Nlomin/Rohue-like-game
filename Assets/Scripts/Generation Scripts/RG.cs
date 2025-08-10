using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Room;


public class RG : MonoBehaviour
{
    public Room[] RoomPrefabs; // ������ ��� ���� ������ (1x1 � 2x2)
    public Room StartingRoom; // ������ �������
    public Room BossRoomPrefab; // ������� ������� �����
    public Room ShopRoomPrefab;
    private Room[,] spawnedRooms; // ������ � ��� ������������ ���������
    

    private List<Vector2Int> endpointRooms = new List<Vector2Int>(); // ������ �������� ������ (� ����� �������)

    public Room SpecialRoomPrefab;
    public GameObject[] ItemPrefabs;
    
    public GameObject Dungeon;
    public DungeonGenerator DungeonGenerator;

    private Vector2Int bossRoomPosition = new Vector2Int(-1, -1);

    public Transform dynamicObjectsContainer;

    private int generationAttempts = 0;
    private const int maxGenerationAttempts = 5;
    //private void Start()
    //{

    //    Application.targetFrameRate = 144;
    //    QualitySettings.vSyncCount = 1;

    //    DungeonGenerator = Dungeon.GetComponent<DungeonGenerator>();
    //    DungeonGenerator.GenerateDungeon(); // ��������� �������
    //}
    private void Awake()
    {
        Application.targetFrameRate = 144;
        QualitySettings.vSyncCount = 1;

        InitDungeon(); // �������� �����
    }
    
    
    private void InitDungeon()
    {
        
        spawnedRooms = new Room[11, 11];

        Room startRoom = Instantiate(StartingRoom).GetComponent<Room>();
        startRoom.transform.SetParent(Dungeon.transform);
        spawnedRooms[5, 5] = startRoom;

        for (int i = 0; i < 11; i++)
        {
            PlaceOneRoom();
        }

        CheckAndMergeRooms();
        ReplaceFarthestRoomWithBoss();

        if (!PlaceRoomWithItem())
        {
            Debug.Log("�� ������� ���������� ������� � ���������. ����������...");
            RestartDungeonGeneration();
            return;
        }

        if (!PlaceShopRoom())
        {
            Debug.Log("�� ������� ���������� �������. ����������...");
            RestartDungeonGeneration();
            return;
        }
        
        DungeonGenerator = Dungeon.GetComponent<DungeonGenerator>();
        
        DungeonGenerator.GenerateDungeon();
    }

    //private void Awake()
    //{

    //    Application.targetFrameRate = 144;
    //    QualitySettings.vSyncCount = 1;

    //    // ������������� ����������
    //    spawnedRooms = new Room[11, 11];

    //    // ��������� ��������� ������� (������)
    //    Room startRoom = Instantiate(StartingRoom).GetComponent<Room>();

    //    startRoom.transform.SetParent(Dungeon.transform);
    //    spawnedRooms[5, 5] = startRoom;
    //    // ��������� ������
    //    for (int i = 0; i < 11; i++)
    //    {
    //        PlaceOneRoom();
    //    }

    //    CheckAndMergeRooms();


    //    // ������� ������� � ������
    //    ReplaceFarthestRoomWithBoss();

    //    // ������� ��������� ������� � ���������
    //    if (!PlaceRoomWithItem())
    //    {
    //        // ���� �� ������� ����� ����� ��� ������� � ���������, ������������� ���������
    //        Debug.Log("�� ������� ���������� ������� � ���������. ���������� ���������...");
    //        RestartDungeonGeneration();
    //    }
    //    if (!PlaceShopRoom())
    //    {
    //        Debug.Log("�� ������� ���������� �������. ����������...");
    //        RestartDungeonGeneration();
    //    }

    //}

    private void RestartDungeonGeneration()
    {
        StartCoroutine(RestartRoutine());
    }

    private IEnumerator RestartRoutine()
    {
        generationAttempts++;
        if (generationAttempts > maxGenerationAttempts)
        {
            Debug.LogError("������� ����� ������� ���������. ������.");
            yield break;
        }
        DungeonGenerator = Dungeon.GetComponent<DungeonGenerator>();

        // ������� NavMesh
        DungeonGenerator.navMeshSurface.RemoveData();
        yield return null; // ���� ����� �������

        // ������� ��� ������ �������
        foreach (var room in spawnedRooms)
        {
            if (room != null)
            {
                Destroy(room.gameObject);
            }
        }

        // ������� �������
        foreach (Transform items in dynamicObjectsContainer)
        {
            Destroy(items.gameObject);
        }

        bossRoomPosition = new Vector2Int(-1, -1);
        spawnedRooms = new Room[11, 11];

        // ��������� ��� ���� ����, ����� �� ���������
        yield return null;

        foreach (GameObject cell in GameObject.FindGameObjectsWithTag("Cell"))
        {
            Destroy(cell);
        }
        // ����������������
        InitDungeon();

        // ��������� ������
        yield return null;
        DungeonGenerator.navMeshSurface.BuildNavMesh();
        generationAttempts = 0;
    }

    private void PlaceOneRoom()
    {
        // ��������� ������ ����
        HashSet<Vector2Int> vacantPlaces = new HashSet<Vector2Int>();

        // �������� �� ���� ������� �������
        for (int x = 0; x < spawnedRooms.GetLength(0); x++)
        {
            for (int y = 0; y < spawnedRooms.GetLength(1); y++)
            {
                if (spawnedRooms[x, y] == null) continue;

                int maxX = spawnedRooms.GetLength(0) - 1;
                int maxY = spawnedRooms.GetLength(1) - 1;

                // ��� ������� 1x1, ����������� �������� �������
                if (x > 0 && spawnedRooms[x - 1, y] == null) vacantPlaces.Add(new Vector2Int(x - 1, y));
                if (y > 0 && spawnedRooms[x, y - 1] == null) vacantPlaces.Add(new Vector2Int(x, y - 1));
                if (x < maxX && spawnedRooms[x + 1, y] == null) vacantPlaces.Add(new Vector2Int(x + 1, y));
                if (y < maxY && spawnedRooms[x, y + 1] == null) vacantPlaces.Add(new Vector2Int(x, y + 1));
            }
        }

        if (vacantPlaces.Count == 0) return;

        // �������� ��������� �������
        Room newRoom = Instantiate(RoomPrefabs[UnityEngine.Random.Range(0, RoomPrefabs.Length)]);
        int limit = 500;

        while (limit-- > 0)
        {
            Vector2Int position = vacantPlaces.ElementAt(UnityEngine.Random.Range(0, vacantPlaces.Count));

            // ������� ������������ �������
            newRoom.RotateRandomly();
            newRoom.transform.SetParent(Dungeon.transform);
            // ����� ��������� ���������� � ������� �����
            if (TryConnectToAllNeighbors(newRoom, position))  
            {
                newRoom.transform.position = new Vector3(position.x - 11, 0, position.y - 11) * 20;
                spawnedRooms[position.x, position.y] = newRoom;
                return;
            }
        }
        
        Destroy(newRoom.gameObject); // ���� �� ������� ����������, ���������� �������
    }

    private bool TryConnectToAllNeighbors(Room room, Vector2Int pos)
    {
        int maxX = spawnedRooms.GetLength(0) - 1;
        int maxY = spawnedRooms.GetLength(1) - 1;

        bool connectedAtLeastOnce = false;

        // UP
        if (pos.y < maxY && spawnedRooms[pos.x, pos.y + 1] is Room upRoom)
        {
            if (room.DoorU != null && upRoom.DoorD != null)
            {
                room.DoorU.SetActive(true);
                room.WallU.SetActive(false);
                upRoom.DoorD.SetActive(true);
                upRoom.WallD.SetActive(false);
                connectedAtLeastOnce = true;
            }
        }

        // DOWN
        if (pos.y > 0 && spawnedRooms[pos.x, pos.y - 1] is Room downRoom)
        {
            if (room.DoorD != null && downRoom.DoorU != null)
            {
                room.DoorD.SetActive(true);
                room.WallD.SetActive(false);
                downRoom.DoorU.SetActive(true);
                downRoom.WallU.SetActive(false);
                connectedAtLeastOnce = true;
            }
        }

        // RIGHT
        if (pos.x < maxX && spawnedRooms[pos.x + 1, pos.y] is Room rightRoom)
        {
            if (room.DoorR != null && rightRoom.DoorL != null)
            {
                room.DoorR.SetActive(true);
                room.WallR.SetActive(false);
                rightRoom.DoorL.SetActive(true);
                rightRoom.WallL.SetActive(false);
                connectedAtLeastOnce = true;
            }
        }

        // LEFT
        if (pos.x > 0 && spawnedRooms[pos.x - 1, pos.y] is Room leftRoom)
        {
            if (room.DoorL != null && leftRoom.DoorR != null)
            {
                room.DoorL.SetActive(true);
                room.WallL.SetActive(false);
                leftRoom.DoorR.SetActive(true);
                leftRoom.WallR.SetActive(false);
                connectedAtLeastOnce = true;
            }
        }

        return connectedAtLeastOnce;
    }


    private void CheckAndMergeRooms()
    {
        for (int x = 0; x < spawnedRooms.GetLength(0) - 1; x++)
        {
            for (int y = 0; y < spawnedRooms.GetLength(1) - 1; y++)
            {
                Room room1 = spawnedRooms[x, y];
                Room room2 = spawnedRooms[x + 1, y];
                Room room3 = spawnedRooms[x, y + 1];
                Room room4 = spawnedRooms[x + 1, y + 1];

                // ���������, ��� ��� ������ ������� ���������� � �� ���� ��������
                if (room1 != null && room2 != null && room3 != null && room4 != null &&
                    !room1.HasModifiedWalls && !room2.HasModifiedWalls &&
                    !room3.HasModifiedWalls && !room4.HasModifiedWalls)
                {
                    MergeRooms(new Vector2Int(x, y), new Vector2Int(x + 1, y), new Vector2Int(x, y + 1), new Vector2Int(x + 1, y + 1));
                }
            }
        }
    }

    private void MergeRooms(Vector2Int room1, Vector2Int room2, Vector2Int room3, Vector2Int room4)
    {

        GameObject mergedParent = new GameObject("MergedRoom");
        mergedParent.transform.SetParent(Dungeon.transform);
       

        // ��������� ��������� ���������� ����������� ��������
        mergedParent.AddComponent<MergedRoomController>();

        // ������ ��� 4 ������� ��������� ���������
        spawnedRooms[room1.x, room1.y].transform.SetParent(mergedParent.transform);
        spawnedRooms[room2.x, room2.y].transform.SetParent(mergedParent.transform);
        spawnedRooms[room3.x, room3.y].transform.SetParent(mergedParent.transform);
        spawnedRooms[room4.x, room4.y].transform.SetParent(mergedParent.transform);

        
        // ������� ���������� ����� ����� ���������
        RemoveWallBetweenRooms(room1, room2);
        RemoveWallBetweenRooms(room1, room3);
        RemoveWallBetweenRooms(room2, room4);
        RemoveWallBetweenRooms(room3, room4);
        



    }

    private void RemoveWallBetweenRooms(Vector2Int roomA, Vector2Int roomB)
    {
        Room room1 = spawnedRooms[roomA.x, roomA.y];
        Room room2 = spawnedRooms[roomB.x, roomB.y];

        if (roomA.x == roomB.x)
        {
            // ������� ��������� �� ����� ������������ ����� (������ �� Y)
            if (roomA.y < roomB.y)
            {
                
                room1.WallU.SetActive(false); 
                room2.WallD.SetActive(false);
                room1.DoorU.SetActive(false);
                room2.DoorD.SetActive(false);
            }
            else
            {
                room1.DoorD.SetActive(false);
                room2.DoorU.SetActive(false);
                room1.WallD.SetActive(false); 
                room2.WallU.SetActive(false); 
            }
        }
        else if (roomA.y == roomB.y)
        {
            // ������� ��������� �� ����� �������������� ����� (������ �� X)
            if (roomA.x < roomB.x)
            {
                room1.DoorR.SetActive(false);
                room2.DoorL.SetActive(false);
                room1.WallR.SetActive(false); 
                room2.WallL.SetActive(false);
            }
            else
            {
                room1.DoorL.SetActive(false);
                room2.DoorR.SetActive(false);
                room1.WallL.SetActive(false); 
                room2.WallR.SetActive(false); 
            }
        }

        // �������� ��� ������� ��� ����������
        room1.MarkAsModified();
        room2.MarkAsModified();
    }

    private void ReplaceFarthestRoomWithBoss()
    {
        Vector2Int startRoomPosition = new Vector2Int(5, 5); // ������� ��������� �������
        Room farthestRoom = null;
        Vector2Int farthestRoomPosition = new Vector2Int(-1, -1); // ��� �������� ������� ����� ������� �������
        float maxDistance = -1; // ������������ ����������

        // �������� �� ���� ��������
        for (int x = 0; x < spawnedRooms.GetLength(0); x++)
        {
            for (int y = 0; y < spawnedRooms.GetLength(1); y++)
            {
                if (spawnedRooms[x, y] != null && new Vector2Int(x, y) != startRoomPosition)
                {
                    // ��������� ���������� �� ��������� �������
                    float distance = Vector2Int.Distance(startRoomPosition, new Vector2Int(x, y));

                    // ���� ��� ����� ������� �������, ���������� � � � �������
                    if (distance > maxDistance)
                    {
                        maxDistance = distance;
                        farthestRoom = spawnedRooms[x, y];
                        farthestRoomPosition = new Vector2Int(x, y);
                    }
                }
            }
        }

        // ���� ������� ����� ������� �������, �������� � �� ������� �����
        if (farthestRoom != null)
        {
            Room bossRoom = Instantiate(BossRoomPrefab, new Vector3(farthestRoomPosition.x - 11, 0, farthestRoomPosition.y - 11) * 20, Quaternion.identity);
            bossRoom.transform.SetParent(Dungeon.transform);
            int limit = 500;
            while (limit-- > 0)
            {
                bossRoom.RotateRandomly();

                if (TryConnectToAllNeighbors(bossRoom, farthestRoomPosition))
                {
                    Debug.Log("��������� ������� � ������");
                    spawnedRooms[farthestRoomPosition.x, farthestRoomPosition.y] = bossRoom;

                    Destroy(farthestRoom.gameObject);

                    bossRoomPosition = farthestRoomPosition;
                    bossRoom.roomType = RoomType.Boss;

                    return;
                }
            }
        }
    }

    private bool PlaceRoomWithItem()
    {
        List<Vector2Int> candidates = new List<Vector2Int>();

        for (int x = 0; x < spawnedRooms.GetLength(0); x++)
        {
            for (int y = 0; y < spawnedRooms.GetLength(1); y++)
            {
                if (spawnedRooms[x, y] == null) continue;

                int neighborCount = 0;

                Vector2Int currentPos = new Vector2Int(x, y);
                if (currentPos == bossRoomPosition) continue;

                // ��������� �������
                if (x > 0 && spawnedRooms[x - 1, y] != null) neighborCount++;
                if (x < spawnedRooms.GetLength(0) - 1 && spawnedRooms[x + 1, y] != null) neighborCount++;
                if (y > 0 && spawnedRooms[x, y - 1] != null) neighborCount++;
                if (y < spawnedRooms.GetLength(1) - 1 && spawnedRooms[x, y + 1] != null) neighborCount++;

                // ��������� ������ ���� ����� 1 �����
                if (neighborCount == 1)
                {
                    candidates.Add(new Vector2Int(x, y));
                }
            }
        }

        if (candidates.Count == 0)
        {
            Debug.LogWarning("�� ������� ���������� ������ ��� ������ �� �����������.");
            return false; // �� ������� ����� ����� ��� ������� � ���������
        }

        // ��������� ���������� �������
        Vector2Int chosen = candidates[UnityEngine.Random.Range(0, candidates.Count)];
        Room oldRoom = spawnedRooms[chosen.x, chosen.y];

        // ������ ����������� �������
        Room specialRoom = Instantiate(SpecialRoomPrefab);
        specialRoom.transform.SetParent(Dungeon.transform);

        int limit = 500;
        while (limit-- > 0)
        {
            specialRoom.RotateRandomly();
            if (TryConnectToAllNeighbors(specialRoom, chosen))
            {
                specialRoom.transform.position = new Vector3(chosen.x - 11, 0, chosen.y - 11) * 20;
                spawnedRooms[chosen.x, chosen.y] = specialRoom;

                Destroy(oldRoom.gameObject);
                specialRoom.roomType = RoomType.Item;
                Debug.Log("��������� ������� � ���������");
                SpawnItemInRoom(specialRoom);
                return true; // ������� � ��������� ������� ���������
            }
        }

        Destroy(specialRoom.gameObject);
        Debug.LogWarning("�� ������� ���������� ����������� ������� �� ���������� �������.");
        return false; // �� ������� ���������� ������� � ���������
    }


    private void SpawnItemInRoom(Room room)
    {
        if (ItemPrefabs == null || ItemPrefabs.Length == 0) return;

        int index = UnityEngine.Random.Range(0, ItemPrefabs.Length);
        GameObject item = Instantiate(ItemPrefabs[index]);
        item.transform.position = room.transform.position + new Vector3(0, 1, 0);
        item.transform.SetParent(dynamicObjectsContainer);
        ItemRegistry.Instance?.ReserveItemForDungeonRoom(ItemPrefabs[index]);
    }
    private bool PlaceShopRoom()
    {
        List<Vector2Int> candidates = new List<Vector2Int>();

        for (int x = 0; x < spawnedRooms.GetLength(0); x++)
        {
            for (int y = 0; y < spawnedRooms.GetLength(1); y++)
            {
                Room room = spawnedRooms[x, y];
                if (room == null || room.roomType != RoomType.Normal) continue;

                int neighborCount = 0;
                Vector2Int currentPos = new Vector2Int(x, y);
                if (currentPos == bossRoomPosition) continue;

                if (x > 0 && spawnedRooms[x - 1, y] != null) neighborCount++;
                if (x < spawnedRooms.GetLength(0) - 1 && spawnedRooms[x + 1, y] != null) neighborCount++;
                if (y > 0 && spawnedRooms[x, y - 1] != null) neighborCount++;
                if (y < spawnedRooms.GetLength(1) - 1 && spawnedRooms[x, y + 1] != null) neighborCount++;

                if (neighborCount == 1)
                    candidates.Add(currentPos);
            }
        }

        if (candidates.Count == 0)
        {
            Debug.LogWarning("�� ������� ���������� ������ ��� ��������.");
            return false;
        }

        Vector2Int chosen = candidates[UnityEngine.Random.Range(0, candidates.Count)];
        Room oldRoom = spawnedRooms[chosen.x, chosen.y];
        if (oldRoom.roomType != RoomType.Normal) return false;

        Room shopRoom = Instantiate(ShopRoomPrefab);
        shopRoom.transform.SetParent(Dungeon.transform);

        int limit = 500;
        while (limit-- > 0)
        {
            shopRoom.RotateRandomly();
            if (TryConnectToAllNeighbors(shopRoom, chosen))
            {
                shopRoom.transform.position = new Vector3(chosen.x - 11, 0, chosen.y - 11) * 20;
                spawnedRooms[chosen.x, chosen.y] = shopRoom;
                shopRoom.roomType = RoomType.Shop;

                Destroy(oldRoom.gameObject);
                
                return true;
            }
        }

        Destroy(shopRoom.gameObject);
        return false;
    }


}

//private bool ConnectToSomething(Room room, Vector2Int p)
//{
//    int maxX = spawnedRooms.GetLength(0) - 1;
//    int maxY = spawnedRooms.GetLength(1) - 1;

//    List<Vector2Int> neighbours = new List<Vector2Int>();



//    if (room.DoorU != null && p.y < maxY && spawnedRooms[p.x, p.y + 1]?.DoorD != null)
//        neighbours.Add(Vector2Int.up);
//    if (room.DoorD != null && p.y > 0 && spawnedRooms[p.x, p.y - 1]?.DoorU != null)
//        neighbours.Add(Vector2Int.down);
//    if (room.DoorR != null && p.x < maxX && spawnedRooms[p.x + 1, p.y]?.DoorL != null)
//        neighbours.Add(Vector2Int.right);
//    if (room.DoorL != null && p.x > 0 && spawnedRooms[p.x - 1, p.y]?.DoorR != null)
//        neighbours.Add(Vector2Int.left);



//    if (neighbours.Count == 0) return false;

//    Vector2Int selectedDirection = neighbours[UnityEngine.Random.Range(0, neighbours.Count)];

//    // �������� ������ �� ��������� �����������


//    if (selectedDirection == Vector2Int.up)
//    {
//        room.DoorU.SetActive(true);
//        room.WallU.SetActive(false);
//        spawnedRooms[p.x, p.y + 1]?.DoorD.SetActive(true);
//        spawnedRooms[p.x, p.y + 1]?.WallD.SetActive(false);
//    }
//    else if (selectedDirection == Vector2Int.down)
//    {
//        room.DoorD.SetActive(true);
//        room.WallD.SetActive(false);
//        spawnedRooms[p.x, p.y - 1]?.DoorU.SetActive(true);
//        spawnedRooms[p.x, p.y - 1]?.WallU.SetActive(false);
//    }
//    else if (selectedDirection == Vector2Int.right)
//    {
//        room.DoorR.SetActive(true);
//        room.WallR.SetActive(false);
//        spawnedRooms[p.x + 1, p.y]?.DoorL.SetActive(true);
//        spawnedRooms[p.x + 1, p.y]?.WallL.SetActive(false);
//    }
//    else if (selectedDirection == Vector2Int.left)
//    {
//        room.DoorL.SetActive(true);
//        room.WallL.SetActive(false);
//        spawnedRooms[p.x - 1, p.y]?.DoorR.SetActive(true);
//        spawnedRooms[p.x - 1, p.y]?.WallR.SetActive(false);
//    }

//    TryConnectBothWays(p, room);

//    return true;
//}
//// �������� �� ���� ������� � ������� �����, ���� ��� ��������
//void TryConnectBothWays(Vector2Int pos, Room room)
//{
//    int maxX = spawnedRooms.GetLength(0) - 1;
//    int maxY = spawnedRooms.GetLength(1) - 1;

//    // Up
//    if (pos.y < maxY && spawnedRooms[pos.x, pos.y + 1] != null)
//    {
//        Room neighbor = spawnedRooms[pos.x, pos.y + 1];
//        if (room.DoorU != null && neighbor.DoorD != null)
//        {
//            room.DoorU.SetActive(true);
//            room.WallU.SetActive(false);
//            neighbor.DoorD.SetActive(true);
//            neighbor.WallD.SetActive(false);
//        }
//    }
//    // Down
//    if (pos.y > 0 && spawnedRooms[pos.x, pos.y - 1] != null)
//    {
//        Room neighbor = spawnedRooms[pos.x, pos.y - 1];
//        if (room.DoorD != null && neighbor.DoorU != null)
//        {
//            room.DoorD.SetActive(true);
//            room.WallD.SetActive(false);
//            neighbor.DoorU.SetActive(true);
//            neighbor.WallU.SetActive(false);
//        }
//    }
//    // Right
//    if (pos.x < maxX && spawnedRooms[pos.x + 1, pos.y] != null)
//    {
//        Room neighbor = spawnedRooms[pos.x + 1, pos.y];
//        if (room.DoorR != null && neighbor.DoorL != null)
//        {
//            room.DoorR.SetActive(true);
//            room.WallR.SetActive(false);
//            neighbor.DoorL.SetActive(true);
//            neighbor.WallL.SetActive(false);
//        }
//    }
//    // Left
//    if (pos.x > 0 && spawnedRooms[pos.x - 1, pos.y] != null)
//    {
//        Room neighbor = spawnedRooms[pos.x - 1, pos.y];
//        if (room.DoorL != null && neighbor.DoorR != null)
//        {
//            room.DoorL.SetActive(true);
//            room.WallL.SetActive(false);
//            neighbor.DoorR.SetActive(true);
//            neighbor.WallR.SetActive(false);
//        }
//    }
//}