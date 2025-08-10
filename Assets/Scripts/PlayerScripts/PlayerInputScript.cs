using UnityEngine;

public class PlayerInputScript : MonoBehaviour
{
    public bool IsInputLocked { get; set; } = false;

    public Vector2 MovementInput { get; private set; }
    public bool MouseInput { get; private set; }
    public Vector3 MousePosition { get; private set; } 

    private IKeyboardInput keyboardInput;
    private IMouseInput mouseInput;

    public bool DashInput { get; private set; }
    public void UpdateInput()
    {
        if (IsInputLocked)
        {
            MovementInput = Vector2.zero;
            MouseInput = false;
            DashInput = false;
            return;
        }
        MovementInput = keyboardInput.GetMovementInput();
        MouseInput = mouseInput.GetAttackInput();
        DashInput = keyboardInput.GetDashInput();
        MousePosition = mouseInput.GetMousePosition();
    }
    public PlayerInputScript(IKeyboardInput keyboardInput, IMouseInput mouseInput)
    {
        this.keyboardInput = keyboardInput;
        this.mouseInput = mouseInput;
    }
}

public interface IKeyboardInput
{
    Vector2 GetMovementInput();
    bool GetDashInput();
}

public class KeyboardInput : IKeyboardInput
{
    public bool GetDashInput()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }

    public Vector2 GetMovementInput()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        return new Vector2(moveX, moveZ);

    }

}

public interface IMouseInput
{
    bool GetAttackInput();
    Vector3 GetMousePosition();
}

public class MouseInput : IMouseInput
{
    public bool GetAttackInput()
    {
        return Input.GetButtonDown("Fire1");
    }

    public Vector3 GetMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        if(groundPlane.Raycast(ray, out float distance))
        {
            return ray.GetPoint(distance);
        }
        return Vector3.zero;
    }
}