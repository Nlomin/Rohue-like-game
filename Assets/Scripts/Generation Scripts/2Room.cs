using UnityEngine;

public class RoomA : MonoBehaviour
{
    public GameObject DoorU;
    public GameObject DoorR;
    public GameObject DoorD;
    public GameObject DoorL;

    public GameObject DoorUup;
    public GameObject DoorRdown;
    public GameObject DoorDdown;
    public GameObject DoorLup;


    public int Width = 1;  // Ширина комнаты
    public int Height = 1; // Высота комнаты
    public Transform PivotPoint;
    public Room room;

    public void RotateRandomly()
    {

        int count = Random.Range(0, 4);
        for (int i = 0; i < count; i++)
        {
            PivotPoint.transform.Rotate(0, 90, 0);

            GameObject tmp = DoorL;
            DoorL = DoorD;
            DoorD = DoorR;
            DoorR = DoorU;
            DoorU = tmp;


            // Пересчитываем размеры после поворота
            int temp = Width;
            Width = Height;
            Height = temp;
        }
    }
}
