using UnityEngine;

public class Room : MonoBehaviour
{
    [Header("WallsWithDoors")]
    public GameObject DoorU;
    public GameObject DoorR;
    public GameObject DoorD;
    public GameObject DoorL;

    [Header("SolidWalls")]
    public GameObject WallU;
    public GameObject WallR;
    public GameObject WallD;
    public GameObject WallL;

    [Header("Room Settings")]
    public bool HasModifiedWalls { get; private set; } = false; // Флаг, указывающий, были ли удалены стены

    public enum RoomType
    {
        Normal,
        Item,
        Shop,
        Boss
    }

    public RoomType roomType = RoomType.Normal;

   
    public void RotateRandomly()
    {
        int count = Random.Range(0, 4);
        for (int i = 0; i < count; i++)
        {
            transform.Rotate(0, 90, 0);

            // Обновляем стены и двери при повороте
            GameObject tmpWall = WallL;
            WallL = WallD;
            WallD = WallR;
            WallR = WallU;
            WallU = tmpWall;

            GameObject tmpDoor = DoorL;
            DoorL = DoorD;
            DoorD = DoorR;
            DoorR = DoorU;
            DoorU = tmpDoor;
        }
    }

    public void MarkAsModified()
    {
        HasModifiedWalls = true; // Устанавливаем флаг, что стены были изменены
    }

}
