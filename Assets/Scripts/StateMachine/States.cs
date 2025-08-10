using UnityEngine;
using static IdleState;

public class States : MonoBehaviour
{

}
public interface IState
{
    void Enter();
    void Execute();
    void Exit();
}

public class WalkState : IState
{
    private PlayerController player;
    private Animator animator;

    public WalkState(PlayerController player, Animator animator)
    {
        this.player = player;
        this.animator = animator;
    }

    public void Enter()
    {
        Debug.Log("Entering Walk State");
        animator.SetFloat("movementSpeed", player.PlayerMovement.GetSpeed());
    }

    public void Execute()
    {
        player.PlayerMovement.HandleMovement();
        animator.SetFloat("movementSpeed", player.PlayerMovement.GetSpeed());

        if (player.PlayerInput.MouseInput)
        {
            player.StateMachine.ChangeState(new AttackState(player, animator));
        }
        else if (player.PlayerInput.MovementInput.magnitude == 0)
        {
            player.StateMachine.ChangeState(new IdleState(player, animator));
        }
        else if (player.PlayerInput.DashInput)
        {
            player.StateMachine.ChangeState(new DashState(player, animator));
           
        }
    }

    public void Exit()
    {
        animator.SetFloat("movementSpeed", 0.1f);
        Debug.Log("Exiting Walk State");
    }
}

public class IdleState : IState
{
    private Animator animator;
    private PlayerController player;

    public IdleState(PlayerController player, Animator animator)
    {
        this.animator = animator;
        this.player = player;
    }

    public void Enter()
    {
        Debug.Log("Entering Idle State");
        animator.SetFloat("movementSpeed", 0.1f);
        
    }

    public void Execute()
    {
        if (player.PlayerInput.MouseInput)
        {
            player.StateMachine.ChangeState(new AttackState(player, animator));
        }
        else if (player.PlayerInput.MovementInput.magnitude > 0)
        {
            player.StateMachine.ChangeState(new WalkState(player, animator));
        }
        else if (player.PlayerInput.DashInput)
        {
            player.StateMachine.ChangeState(new DashState(player, animator));
          
        }
    }

    public void Exit()
    {
        Debug.Log("Exiting Idle State");
    }
}

public class AttackState : IState
{
    private PlayerController player;
    private Animator animator;
    
    private float attackDuration = 0.91f; // Пересчитанная длительность атаки
    private float comboWindow = 0.5f;
    private float elapsedTime = 0f;

    public AttackState(PlayerController player, Animator animator)
    {
        this.player = player;
        this.animator = animator;
    }

    public void Enter()
    {
        player.RotateTowardsMouse();
        Debug.Log("Entering Attack State");
        animator.SetTrigger("attackTrigger");
        animator.SetFloat("movementSpeed", 0f); 
        elapsedTime = 0f;
    }

    public void Execute()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= attackDuration - comboWindow && player.PlayerInput.MouseInput)
        {
            player.StateMachine.ChangeState(new SecondAttackState(player, animator));
        }
        else if (elapsedTime >= attackDuration)
        {
            if (player.PlayerInput.MovementInput.magnitude > 0)
                player.StateMachine.ChangeState(new WalkState(player, animator));
            else
                player.StateMachine.ChangeState(new IdleState(player, animator));
        }
        else if (player.PlayerInput.DashInput)
        {
            player.StateMachine.ChangeState(new DashState(player, animator));
            return;
        }
        

    }

    public void Exit()
    {
        Debug.Log("Exiting Attack State");
        animator.ResetTrigger("attackTrigger");
        animator.SetFloat("movementSpeed", 0.1f);
        player.DisableSwordCollider();
        player.swordTrail.DisableTrail();
    }
}

public class SecondAttackState : IState
{
    private PlayerController player;
    private Animator animator;
    //private bool comboPerformed = false;
    private float attackDuration = 1.0f;
    private float comboWindow = 0.5f;
    private float elapsedTime = 0f;

    public SecondAttackState(PlayerController player, Animator animator)
    {
        this.player = player;
        this.animator = animator;
    }

    public void Enter()
    {
        player.RotateTowardsMouse();
        Debug.Log("Entering Second Attack State");
        animator.SetTrigger("secondAttackTrigger");
        animator.SetFloat("movementSpeed", 0f); 
        elapsedTime = 0f;
        //comboPerformed = false;
    }

    public void Execute()
    {
        elapsedTime += Time.deltaTime;

        //if (!comboPerformed && elapsedTime >= attackDuration - comboWindow && player.PlayerInput.MouseInput)
        if (elapsedTime >= attackDuration - comboWindow && player.PlayerInput.MouseInput)
        {
            //comboPerformed = true;
            player.StateMachine.ChangeState(new ThirdAttackState(player, animator));
        }
        else if (elapsedTime >= attackDuration)
        {
            if (player.PlayerInput.MovementInput.magnitude > 0)
                player.StateMachine.ChangeState(new WalkState(player, animator));
            else
                player.StateMachine.ChangeState(new IdleState(player, animator));
        }
        else if (player.PlayerInput.DashInput)
        {
            player.StateMachine.ChangeState(new DashState(player, animator));
            return;
        }
    }

    public void Exit()
    {
        Debug.Log("Exiting Second Attack State");
        animator.ResetTrigger("secondAttackTrigger");
        animator.SetFloat("movementSpeed", 0.1f);
        player.DisableSwordCollider();
        player.swordTrail.DisableTrail();
    }
}

public class ThirdAttackState : IState
{
    private PlayerController player;
    private Animator animator;
    private float attackDuration = 1.15f;
    private float elapsedTime = 0f;
    private float comboWindow = 0.5f;
    public ThirdAttackState(PlayerController player, Animator animator)
    {
        this.player = player;
        this.animator = animator;
    }

    public void Enter()
    {
        player.RotateTowardsMouse();
        Debug.Log("Entering Third Attack State");
        animator.SetTrigger("thirdAttackTrigger");
        animator.SetFloat("movementSpeed", 0f);
        elapsedTime = 0f;
    }

    public void Execute()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= attackDuration - comboWindow && player.PlayerInput.MouseInput)
        {
            player.StateMachine.ChangeState(new AttackState(player, animator));
        }
        else if (elapsedTime >= attackDuration)
        {
            if (player.PlayerInput.MovementInput.magnitude > 0)
                player.StateMachine.ChangeState(new WalkState(player, animator));
            else
                player.StateMachine.ChangeState(new IdleState(player, animator));
        }
        else if (player.PlayerInput.DashInput)
        {
            player.StateMachine.ChangeState(new DashState(player, animator));
            return;
        }
    }

    public void Exit()
    {
        Debug.Log("Exiting Third Attack State");
        animator.ResetTrigger("thirdAttackTrigger");
        animator.SetFloat("movementSpeed", 0.1f);
        player.DisableSwordCollider();
        player.swordTrail.DisableTrail();
    }
}

public class DashState : IState
{
    private PlayerController player;
    private Animator animator;
    private float dashDuration = 0.3f; // Длительность рывка
    private float dashSpeedMultiplier = 2.5f; // Ускорение во время рывка
    private float elapsedTime = 0f;
    private Vector3 dashDirection;

    public DashState(PlayerController player, Animator animator)
    {
        this.player = player;
        this.animator = animator;
    }

    public void Enter()
    {
        player.swordTrail.DisableTrail();
        Debug.Log("Entering Dash State");
        animator.SetTrigger("dash");
        animator.SetFloat("movementSpeed", dashSpeedMultiplier);

        // Определяем направление рывка
        dashDirection = new Vector3(player.PlayerInput.MovementInput.x, 0, player.PlayerInput.MovementInput.y).normalized;
        if (dashDirection == Vector3.zero)
        {
            dashDirection = player.transform.forward; // Если нет ввода, используем текущее направление
        }

        elapsedTime = 0f;
    }

    public void Execute()
    {
        elapsedTime += Time.deltaTime;


        if (elapsedTime >= dashDuration)
        {
            // Возвращаемся в предыдущее состояние (ходьба или ожидание)
            if (player.PlayerInput.MovementInput.magnitude > 0)
                player.StateMachine.ChangeState(new WalkState(player, animator));
            else
                player.StateMachine.ChangeState(new IdleState(player, animator));
        }
    }

    public void Exit()
    {
        Debug.Log("Exiting Dash State");
        animator.ResetTrigger("dash");
        animator.SetFloat("movementSpeed", 1f); 
    }
}



