using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    public float speed = 10f; // Скорость полета
    public int damage = 10;   // Урон, который наносит стрела
    private Vector3 direction; // Направление полета стрелы
    private float timeOfExist = 2f;
    private void Awake()
    {
        Destroy(gameObject, timeOfExist);
    }
    public void SetDirection(Vector3 target)
    {
        direction = (target - transform.position).normalized;
        // Если стрела в Blender/FBX "смотрит" вверх (Y), а не вперёд (Z):
        transform.rotation = Quaternion.LookRotation(direction);
        transform.Rotate(90f, 0, 0f); // Повернуть носом по направлению
    }


    void Update()
    {
        // Двигаем стрелу вперед
        transform.position += direction * speed * Time.deltaTime;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Получаем компонент здоровья игрока
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
