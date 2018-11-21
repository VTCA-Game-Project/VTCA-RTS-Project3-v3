using Common.Entity;
using EnumCollection;
using UnityEngine;

namespace Animation
{
    public abstract class BaseAnimation : MonoBehaviour
    {
        protected Animator anims;
        protected Rigidbody agentRigid;
        protected AIAgent agent;

        public AnimState DefaultState   { get; set; }
        public AnimState CurrentState   { get; set; }
        public AnimState NextState      { get; set; }

        public void Play(AnimState type)
        {
            if (type == CurrentState) return;
            ResetParams();
            CurrentState = type;
            switch (type)
            {
                case AnimState.Idle:
                    Idle();
                    break;
                case AnimState.Attack:
                    Attack();
                    break;                
                case AnimState.Run:
                    Run();
                    break;
                case AnimState.Dead:
                    Dead();
                    break;
                case AnimState.Damage:
                    Damage();
                    break;
                default:
                    break;
            }
        }

        protected abstract void Idle();
        protected abstract void Run();
        protected abstract void Damage();
        protected abstract void Attack();
        protected abstract void Dead();
        protected virtual void ResetParams()
        {
            switch (CurrentState)
            {
                case AnimState.Attack:
                    anims.SetBool("IsAttack", false);
                    break;
                case AnimState.Idle:
                    anims.SetBool("IsRunning", false);
                    break;
            }

            //anims.SetBool("IsAttack", false);
            //anims.SetBool("IsRunning", false);
        }

        protected virtual void Awake()
        {
            agent = GetComponent<AIAgent>();
            anims = GetComponent<Animator>();
            agentRigid = GetComponent<Rigidbody>();

            DefaultState = AnimState.Idle;
            CurrentState = AnimState.None;
            NextState = DefaultState;
        }
        protected virtual void Update()
        {
            if(agent.IsDead)
            {
                NextState = AnimState.Dead;
                Play(NextState);
                return;
            }
            if (CurrentState == AnimState.Idle)
            {
                if (agentRigid.velocity.sqrMagnitude > 0.1f)
                {
                    NextState = AnimState.Run;
                }
                else if (agent.TargetType == TargetType.NPC)
                {
                    NextState = AnimState.Attack;
                }
            }
            else if (CurrentState == AnimState.Run)
            {
                if (agentRigid.velocity.sqrMagnitude <= 0.1f)
                {
                    NextState = AnimState.Idle;
                    agentRigid.velocity = Vector3.zero;
                }
            }
            if (CurrentState == AnimState.Attack)
            {
                if (agentRigid.velocity.sqrMagnitude > 0.1f)
                {
                    NextState = AnimState.Run;
                } else if(agent.TargetEntity == null)
                {
                    NextState = AnimState.Idle;
                }
            }

            Play(NextState);
        }

        public void ForceResetState()
        {
            CurrentState = AnimState.Attack;
            NextState = AnimState.Idle;
        }
    }
}