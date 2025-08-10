using UnityEngine;

public class MeleeEnemySword : MonoBehaviour
{
    public int damage = 10;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            // �������� ��������� �������� ������
            PlayerStats playerHealth = other.GetComponent<PlayerStats>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage); 
            }
        }
    }
}
