using UnityEngine;

public class BossOrbitingSphere : MonoBehaviour
{
    public int damage = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.stats.TakeDamage(damage);
                
            }
        }
    }
}
