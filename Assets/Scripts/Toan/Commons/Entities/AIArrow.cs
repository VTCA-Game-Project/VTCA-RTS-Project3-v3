using Common;
using UnityEngine;

public class AIArrow : GameEntity
{
    private Vector3 targetPos;
    private GameEntity targetEnity;

    public int Damage { get; set; }
    public GameEntity Target
    {
        get { return targetEnity; }
        set
        {
            targetPos = value.Position;
            targetEnity = value;
        }
    }
    public override bool IsDead { get; protected set; }

    private void FixedUpdate()
    {
        if (IsDead || Target == null) return;
        if ((Position - targetPos).magnitude <= 1f)
        {
            Dead();
        }
    }

    public override void Dead()
    {
        IsDead = true;
        if(Target != null)
        {
            Target.TakeDamage(Damage);
        }
        Destroy(gameObject);
    }

    public void Init(GameEntity target, int damage)
    {
        Target = target;
        Damage = damage;
    }
}
