
namespace Animation
{
    public class ArcherAnimation : BaseAnimation
    {
        protected override void Attack()
        {
            anims.SetBool("IsAttack", true);
        }

        protected override void Damage()
        {
        }

        protected override void Dead()
        {
            anims.SetTrigger("Dead");
        }

        protected override void Idle()
        {
            anims.SetBool("IsRunning", false);
        }

        protected override void Run()
        {
            anims.SetBool("IsRunning", true);
        }

        protected override void ResetParams()
        {
            base.ResetParams();
        }

        protected override void Update()
        {
            base.Update();
        }
    }
}