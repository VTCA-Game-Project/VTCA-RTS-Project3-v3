using Common.Entity;
using EnumCollection;
using InterfaceCollection;
using Manager;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Building
{
    public class Defender : Construct, IDetectEnemy, IAttackable
    {
        private bool isDetectedEnemy;
        private float attackCounter;

        public Rigidbody Arrow;
        public Transform LauncherPoint;
        public Group PlayerGroup { get; set; }
        public GameEntity TargetEntity { get; set; }

        public int Damage { get; set; }
        public float AttackRange { get; set; }
        public float DelayAttack { get; set; }
        public float ShootForce { get; set; }

        protected override void Awake()
        {
            Id = ConstructId.Defender;
            base.Awake();
        }
        protected override void Start()
        {
            base.Start();
        }
        protected override void Update()
        {
            if (IsDead) return;
            if (TargetEntity == null)
            {
                DetectEnemy();
            }
            if (isDetectedEnemy)
            {
                attackCounter += Time.deltaTime;
                if (attackCounter >= DelayAttack)
                {
                    Attack();
                    attackCounter = 0;
                }
            }
        }

        public void DetectEnemy()
        {
            // find agent inside attack range 

            if (TargetEntity != null && TargetEntity.IsDead)
            {
                TargetEntity = null;
                return;
            }
            if (TargetEntity != null) return;

            List<Player> players = UpdateGameStatus.Instance.Players;
            List<AIAgent> enemies;
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].Group != PlayerGroup)
                {
                    enemies = players[i].Agents;
                    for (int j = 0; j < enemies.Count; j++)
                    {
                        if (enemies[j] != null && !enemies[j].IsDead && Vector3.Distance(enemies[j].Position, Position) <= AttackRange)
                        {
                            TargetEntity = enemies[j];
                            isDetectedEnemy = true;
                            break;
                        }
                    }
                }
                if (TargetEntity != null) break;
            }
        }

        public void Attack()
        {
            // attack target
            // before attack check either enemy out of attack range or not
            if ((TargetEntity.Position - Position).sqrMagnitude < AttackRange * AttackRange)
            {
                // fire
                Fire();
            }
            else
            {
                TargetEntity = null;
                isDetectedEnemy = false;
            }
        }

        private void Fire()
        {
            if (TargetEntity != null && !TargetEntity.IsDead)
            {
                Rigidbody copyArrow = Instantiate(Arrow, LauncherPoint.position, transform.rotation);
                copyArrow.gameObject.SetActive(true);
                copyArrow.transform.LookAt(TargetEntity.transform);
                copyArrow.transform.Rotate(-8, 0, 0);
                copyArrow.AddForce(copyArrow.transform.forward * ShootForce);
                copyArrow.GetComponent<AIArrow>().Init(TargetEntity, Damage);
            }
        }

        protected override void InitOffset()
        {
            Damage      = Offset.Damage;
            AttackRange = Offset.AttackRadius;
            DelayAttack = Offset.DelayAttack;
            ShootForce  = Offset.AttackForce;
        }
    }
}
