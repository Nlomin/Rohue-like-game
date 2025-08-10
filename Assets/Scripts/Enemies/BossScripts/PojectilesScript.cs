using UnityEngine;

public class ProjectilesScript : MonoBehaviour
{
    private Vector3 moveDirection;
    private float speed;
    private float fixedY; // ������������� ������

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

        // ����� ��������� Y-�������
        transform.position = new Vector3(transform.position.x, fixedY, transform.position.z);
    }
}
