using EnumCollection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Animation
{
    public class SeigeAnimation : BaseAnimation
    {
        protected override void Attack()
        {
            anims.SetBool("IsAttack", true);
        }

        protected override void Damage() { }

        protected override void Dead() {  }

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
