using UnityEngine;

public class ProjectilesScript : MonoBehaviour
{
    private Vector3 moveDirection;
    private float speed;
    private float fixedY; // фиксированная высота

    public void SetDirection(Vector3 dir, float spd)
    {
        moveDirection = new Vector3(dir.x, 0f, dir.z).normalized; 
        speed = spd;
        fixedY = transform.position.y; 
    }

    private void Update()
    {
        Vector3 move = moveDirection * speed * Time.deltaTime;
        transform.position += move;

        // жёстко фиксируем Y-позицию
        transform.position = new Vector3(transform.position.x, fixedY, transform.position.z);
    }
}
