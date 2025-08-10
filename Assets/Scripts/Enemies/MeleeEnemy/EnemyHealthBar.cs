using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Image hpBar;
    [SerializeField] private Image delayedBar;
    [SerializeField] private float smoothSpeed = 0.5f;

    private float targetFill = 1f;

    private MEnemyStats stats;

    private void Awake()
    {
        // Автоматически ищем компоненты на родителе
        stats = GetComponentInParent<MEnemyStats>();
        if (stats == null)
        {
            Debug.LogError("EnemyHealthBar: Не найден MEnemyStats на родителе!");
        }
    }

    private void Start()
    {
        if (stats != null)
        {
            // Подписываемся на событие получения урона
            stats.OnDamageTaken.AddListener(UpdateHealth);
            // И сразу обновим полосу при старте
            UpdateHealth(stats.currentHealth);
        }
    }

    private void UpdateHealth(int current)
    {
        if (stats == null || stats.maxHealth <= 0) return;

        float percent = Mathf.Clamp01((float)current / stats.maxHealth);
        targetFill = percent;
        hpBar.fillAmount = percent;
    }

    private void Update()
    {
        if (delayedBar.fillAmount > targetFill)
        {
            delayedBar.fillAmount = Mathf.Lerp(delayedBar.fillAmount, targetFill, Time.deltaTime * smoothSpeed);
        }

        
    }
}
