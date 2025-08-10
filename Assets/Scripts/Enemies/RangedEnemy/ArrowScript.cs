using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    public float speed = 10f; // �������� ������
    public int damage = 10;   // ����, ������� ������� ������
    private Vector3 direction; // ����������� ������ ������
    private float timeOfExist = 2f;
    private void Awake()
    {
        Destroy(gameObject, timeOfExist);
    }
    public void SetDirection(Vector3 target)
    {
        direction = (target - transform.position).normalized;
        // ���� ������ � Blender/FBX "�������" ����� (Y), � �� ����� (Z):
        transform.rotation = Quaternion.LookRotation(direction);
        transform.Rotate(90f, 0, 0f); // ��������� ����� �� �����������
    }


    void Update()
    {
        // ������� ������ ������
        transform.position += direction * speed * Time.deltaTime;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // �������� ��������� �������� ������
            PlayerStats playerHealth = other.GetComponent<PlayerStats>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
        if (other.CompareTag("Obctacle"))
        {
            Destroy(gameObject);
        }
    }
}
