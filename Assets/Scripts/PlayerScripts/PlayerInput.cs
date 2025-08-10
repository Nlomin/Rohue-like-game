using UnityEditor;
using UnityEngine;


public class PlayerInput : MonoBehaviour
{
    [Header("Controls")]
    [SerializeField] private KeyCode m_ForwardKey = KeyCode.W;
    [SerializeField] private KeyCode m_BackwardKey = KeyCode.S;
    [SerializeField] private KeyCode m_LeftKey = KeyCode.A;
    [SerializeField] private KeyCode m_RightKey = KeyCode.D;
    
    public bool IsRunning { get => isRunning; set => isRunning = value; }
    private bool isRunning;

    public bool IsAttacking { get => isAttacking; set => isAttacking = value; }
    private bool isAttacking;
    public StateMachine stateMachine { get; private set; }

    Animator anim;
    private Vector3 m_InputVector;

    public Vector3 InputVector => m_InputVector;
    private void Start()
    {
        anim = GetComponent<Animator>(); // Инициализируем Animator
    }
    private void Update()
    {
        HandleInput();
        
    }

    private void HandleInput()
    {
        float xInput = 0;
        float zInput = 0;
       
        if (Input.GetKey(m_ForwardKey))
        {
            
            zInput++;
        }

        if (Input.GetKey(m_BackwardKey))
        {
            
            zInput--;
        }

        if (Input.GetKey(m_LeftKey))
        {
            
            xInput--;
        }

        if (Input.GetKey(m_RightKey))
        {
            
            xInput++;
        }
        isAttacking = Input.GetButton("Fire1");
        isRunning = (xInput != 0f || zInput != 0f); // Игрок бегает, если двигается


        m_InputVector = new Vector3(xInput, 0, zInput);
        
        
    }
    public void SetAnim(string animationName, bool value)
    {
        anim.SetBool(animationName, value); // Устанавливаем параметры анимации
    }
}
