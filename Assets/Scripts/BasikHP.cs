using UnityEngine;
using UnityEngine.Events;

public class BasikHP : MonoBehaviour
{
    [Range(1, 100)]
    [SerializeField] int maxHP;

    public int currentHP;

    public UnityEvent onHealing;
    public UnityEvent onTakeDamage;
    public UnityEvent onDeath;

    private void Start()
    {
        currentHP = maxHP;
    }

    public void hpIncrease(int change)
    {
        currentHP += change;

        if (currentHP > maxHP) currentHP = maxHP;

        onHealing?.Invoke();
    }

    public void hpDecrease(int change)
    {
        if (currentHP == 0) return;

        currentHP -= change;

        if (currentHP <= 0)
        {
            currentHP = 0;
            onDeath?.Invoke();
        } else
        onTakeDamage?.Invoke();
    }

}
