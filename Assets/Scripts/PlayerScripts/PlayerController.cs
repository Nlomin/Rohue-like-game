using UnityEngine;


[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public PlayerMovement PlayerMovement { get; private set; }
    public Animator Animator { get; private set; }
    public PlayerInputScript PlayerInput { get; private set; }
    public StateMachine StateMachine { get; private set; }
    public SwordTrail swordTrail { get; private set; }
    public PlayerStats stats { get; private set; }   
    public Collider swordCollider; 

    private int comboIndex = 0;
    
    private float lastAttackTime;
    Animator animator;
    public int ComboIndex => comboIndex;

   

    private void Awake()
    {
        animator = GetComponent<Animator>();
        PlayerInput = new PlayerInputScript(new KeyboardInput(), new MouseInput());
        StateMachine = new StateMachine(this, animator);
        StateMachine.Initialize(new IdleState(this, animator));
        stats = GetComponent<PlayerStats>();
        swordTrail = GetComponent<SwordTrail>();
        if (swordCollider != null)
            swordCollider.enabled = false;

        
    }
    void Start()
    {
        PlayerMovement = GetComponent<PlayerMovement>();
    }
    void Update()
    {
        
        PlayerInput.UpdateInput();
        StateMachine.Execute(); 
    }

    public void EnableSwordCollider()
    {
        if (swordCollider != null)
        {
            swordCollider.enabled = true;
        }
    }
    public void RotateTowardsMouse()
    {
        Vector3 targetPosition = PlayerInput.MousePosition;
        targetPosition.y = transform.position.y; 

        Vector3 direction = (targetPosition - transform.position).normalized;

        if (direction != Vector3.zero)
        {
            transform.forward = direction; 
        }
    }
    public void DisableSwordCollider()
    {
        if (swordCollider != null)
        {
            swordCollider.enabled = false;
        }
    }
    public void AddMovement()
    {
        animator.SetFloat("movementSpeed", 0.1f);
    }
    private void DashMove()
    {
        PlayerMovement.DashMove();
        Debug.Log("MOVED===================");
    }
    public void AddDamage(float amount)
    {
        Debug.Log($"Player Gets {amount} damage");
    }
    public void SetInputLocked(bool isLocked)
    {
        PlayerInput.IsInputLocked = isLocked;   
    }

}
