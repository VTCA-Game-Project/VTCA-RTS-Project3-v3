using UnityEngine;

namespace Common.Entity
{
    public class AISeige : AIAgent
    {
        public Transform FireBall;
        public Transform LauncherPoint;
       
        protected override void Awake()
        {
            base.Awake();
        }
        protected override void Start()
        {
            base.Start();
        }
        protected override void FixedUpdate()
        {
           
            base.FixedUpdate();
        }

        public override void Attack()
        {
            if (TargetEntity != null && !TargetEntity.IsDead)
            {
                Transform copyBall = Instantiate(FireBall, LauncherPoint.position, LauncherPoint.rotation);
                copyBall.gameObject.SetActive(true);
                copyBall.GetComponent<AIFireBall>().Init(TargetEntity, Damage);
            }
            base.Attack();
        }

        public override void Dead()
        {
            base.Dead();
        }
    }

}