
namespace Animation
{
    public class MagicAnimation : BaseAnimation
    {

        protected override void Attack()
        {
            anims.SetBool("IsAttack", true);
        }

        protected override void Damage()
        {
            anims.SetTrigger("Damage");
        }

        protected override void Dead()
        {
            anims.SetTrigger("Dead");
        }

        protected override void Idle()
        {
            anims.SetBool("IsRunning", false);
        }

        protected override void ResetParams()
        {
            base.ResetParams();
        }

        protected override void Run()
        {
            anims.SetBool("IsRunning", true);
        }

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Update()
        {
            base.Update();
        }

    }
}
