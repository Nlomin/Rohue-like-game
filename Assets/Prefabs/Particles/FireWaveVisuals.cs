using UnityEngine;

public class BurningWave : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 2f;
    public BurnEffect burnEffect;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        MEnemyStats enemyStats = other.GetComponent<MEnemyStats>();
        if (enemyStats != null && burnEffect != null)
        {
            IStatusEffectTarget target = enemyStats.GetComponent<IStatusEffectTarget>();
            if (target != null)
            {
                burnEffect.ApplyEffect(target);
            }
        }
    }
}
