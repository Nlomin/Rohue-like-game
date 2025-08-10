using UnityEngine;

public interface IAttack
{
    void Attack();
    bool IsPlayerInAttackRange();
}
public interface IMovable
{
    void Move();
}
public interface IChase
{
    void Patrol();
    bool CanSeePlayer(out Transform playerTransform);
    void MoveTo(Vector3 target);
}
public interface IDamageable 
{
    public void Die();
    public void TakeDamage(int damage);
    public void RestoreHealth(int healing);
}

public interface IUnitStats
{
    public int Strength { get; set; }
    public int Dexterity { get; set; }
    public int Endurance { get; set; }
}

public interface IStatusEffectTarget : IDamageable
{
    void RegisterEffect(StatusEffect effect, Coroutine routine);
    void UnregisterEffect(StatusEffect effect);
    void ShowEffectIcon(Sprite icon, float duration);
    bool HasEffect(StatusEffect effect);
    Transform GetTransform();
}

public interface IFreezable
{
    void FreezeEnemy(float duration);
}





