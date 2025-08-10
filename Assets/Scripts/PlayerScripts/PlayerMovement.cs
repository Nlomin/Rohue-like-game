using System.Collections;
using UnityEngine;


public class PlayerMovement : MonoBehaviour, IPlayerMovement
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public float acceleration = 2f;
    public float deceleration = 2.5f;

    public float currentSpeed = 0f;
    private float maxSpeed;

    private CharacterController characterController;
    private PlayerInputScript playerInput;

    [Header("Camera")]
    public Transform cameraTransform; // Укажи Main Camera (с Cinemachine)

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerInput = new PlayerInputScript(new KeyboardInput(), new MouseInput());

        maxSpeed = moveSpeed;

        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;
    }

    public void HandleMovement()
    {
        playerInput.UpdateInput();
        Vector2 input = playerInput.MovementInput;

        Vector3 moveDirection = new Vector3(input.x, 0f, input.y);
        moveDirection = CameraRelativeDirection(moveDirection);

        float targetSpeed = moveDirection.magnitude * moveSpeed;

        // Ускорение / замедление
        if (targetSpeed > 0.1f)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            currentSpeed = Mathf.Lerp(currentSpeed, 0f, deceleration * Time.deltaTime);
        }

        currentSpeed = Mathf.Min(currentSpeed, maxSpeed);

        if (currentSpeed > 0.1f)
        {
            characterController.Move(moveDirection.normalized * currentSpeed * Time.deltaTime);
            RotatePlayer(moveDirection);
        }
    }

    Vector3 CameraRelativeDirection(Vector3 inputDirection)
    {
        if (cameraTransform == null) return inputDirection;

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        return (forward * inputDirection.z + right * inputDirection.x);
    }

    void RotatePlayer(Vector3 direction)
    {
        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    public float GetSpeed()
    {
        return characterController.velocity.magnitude;
    }

    public void DashMove()
    {
        float dashDistance = 1f;
        float dashDuration = 0.2f;
        Vector3 dashDirection = transform.forward;

        StartCoroutine(DashCoroutine(dashDirection, dashDistance, dashDuration));
    }

    private IEnumerator DashCoroutine(Vector3 direction, float distance, float duration)
    {
        float elapsedTime = 0f;
        float startSpeed = currentSpeed;
        float dashSpeed = distance / duration;

        while (elapsedTime < duration)
        {
            float deltaSpeed = Mathf.Lerp(startSpeed, dashSpeed, elapsedTime / duration);
            characterController.Move(direction * deltaSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        currentSpeed = startSpeed;
    }
}


public interface IPlayerMovement
{
    void HandleMovement();
}

//public class PlayerMovement : MonoBehaviour, IPlayerMovement
//{

//    public float moveSpeed = 5f;
//    public float rotationSpeed = 10f;

//    public float currentSpeed = 0f;
//    public float acceleration = 2f;
//    public float deceleration = 2.5f;

//    private float maxSpeed;

//    private CharacterController characterController;
//    private PlayerInputScript playerInput;


//    void Start()
//    {
//        characterController = GetComponent<CharacterController>();
//        playerInput = new PlayerInputScript(new KeyboardInput(), new MouseInput());

//        maxSpeed = moveSpeed;
//    }

//    public void HandleMovement()
//    {
//        playerInput.UpdateInput();
//        Vector2 input = playerInput.MovementInput;
//        Vector3 moveDirection = new Vector3(input.x, 0, input.y);

//        float targetSpeed = moveDirection.magnitude * moveSpeed;

//        // Если персонаж двигается, плавно увеличиваем скорость, иначе плавно уменьшаем
//        if (targetSpeed > 0.1f)
//        {
//            currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, acceleration * Time.deltaTime);
//        }
//        else
//        {
//            currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, deceleration * Time.deltaTime);
//        }

//        // Ограничиваем максимальную скорость
//        currentSpeed = Mathf.Min(currentSpeed, maxSpeed);

//        if (currentSpeed > 0.1f) // Если скорость больше минимума, двигаем персонажа
//        {
//            characterController.Move(moveDirection.normalized * currentSpeed * Time.deltaTime);
//            RotatePlayer(moveDirection);
//        }
//    }

//    void RotatePlayer(Vector3 direction)
//    {
//        if (direction.sqrMagnitude > 0)
//        {
//            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
//            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
//        }
//    }

//    public float GetSpeed()
//    {
//        return characterController.velocity.magnitude;
//    }


//    public void DashMove()
//    {
//        float dashDistance = 1f; // Дальность рывка
//        float dashDuration = 0.2f; // Длительность рывка в секундах
//        Vector3 dashDirection = transform.forward; // Направление рывка

//        StartCoroutine(DashCoroutine(dashDirection, dashDistance, dashDuration));
//    }

//    private IEnumerator DashCoroutine(Vector3 direction, float distance, float duration)
//    {
//        float elapsedTime = 0f;
//        float startSpeed = currentSpeed;
//        float dashSpeed = distance / duration; // Скорость во время рывка

//        while (elapsedTime < duration)
//        {
//            float deltaSpeed = Mathf.Lerp(startSpeed, dashSpeed, elapsedTime / duration);
//            characterController.Move(direction * deltaSpeed * Time.deltaTime);
//            elapsedTime += Time.deltaTime;
//            yield return null;
//        }

//        currentSpeed = startSpeed; // Возвращаем скорость обратно
//    }
//}