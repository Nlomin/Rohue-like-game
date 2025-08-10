using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public Slider HealtBar;
    private PlayerStats stats;
    public Animator damageAnim;

    public Texture2D swordCursor;
    public Vector2 hotspot = Vector2.zero;
    public CursorMode cursorMode = CursorMode.Auto;
    private void Start()
    {
        stats = GetComponent<PlayerStats>();
        HealthBarValue();
        SetSwordCursor();
    }
    public void HealthBarValue()
    {
        HealtBar.value = stats.GetCurrentHp();
        Debug.Log("получил дамагу");
    }
    public void ShowDamageFrame()
    {
        damageAnim.SetTrigger("ShowDamage");
    }
    public void SetSwordCursor()
    {
        Vector2 hotspot = new Vector2(swordCursor.width / 2f, swordCursor.height / 2f);
        Cursor.SetCursor(swordCursor, hotspot, cursorMode);
    }

    public void ResetCursor()
    {
        Cursor.SetCursor(null, Vector2.zero, cursorMode);
    }
}
