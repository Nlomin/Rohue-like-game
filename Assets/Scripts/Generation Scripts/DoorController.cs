using UnityEngine;
using System.Collections;
public class DoorController : MonoBehaviour
{
    public float moveSpeed = 6f;

    public void LowerAllDoors()
    {
        GameObject[] doors = GameObject.FindGameObjectsWithTag("Door");

        foreach (GameObject door in doors)
        {
            StartCoroutine(MoveDoor(door.transform, -4f));
        }
    }

    public void RaiseAllDoors()
    {
        GameObject[] doors = GameObject.FindGameObjectsWithTag("Door");

        foreach (GameObject door in doors)
        {
            StartCoroutine(MoveDoor(door.transform, 2.5f));
        }
    }

    private IEnumerator MoveDoor(Transform door, float targetY)
    {
        if (door == null) yield break; // сразу выход, если вдруг дверь не существует

        Vector3 target = new Vector3(door.position.x, targetY, door.position.z);

        while (door != null && Mathf.Abs(door.position.y - targetY) > 0.01f)
        {
            door.position = Vector3.MoveTowards(
                door.position,
                target,
                moveSpeed * Time.deltaTime
            );
            yield return null;
        }

        if (door != null)
        {
            door.position = target;
        }
    }

}


