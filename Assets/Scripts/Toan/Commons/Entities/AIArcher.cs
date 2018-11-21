using Common.Entity;
using UnityEngine;

namespace Common.Entity
{
    public class AIArcher : AIAgent
    {
        public Rigidbody Arrow;
        public Transform LauncherPoint;
        public float ShootForce;
      
     
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
            SoundManager.instanece.PlayEffect(5);
            base.Attack();
            if (TargetEntity == null || TargetEntity.IsDead) return;
            Vector3 direction = (TargetEntity.Position - Position).normalized;
            Rigidbody copyArrow = Instantiate(Arrow, LauncherPoint.position,transform.rotation);
            copyArrow.gameObject.SetActive(true);
            copyArrow.transform.forward = direction;
            copyArrow.AddForce(direction * ShootForce);
            copyArrow.GetComponent<AIArrow>().Init(TargetEntity,Damage);
                       
        }
    }
}