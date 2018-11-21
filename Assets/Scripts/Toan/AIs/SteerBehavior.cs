using UnityEngine;
using Common.Entity;
using EnumCollection;

namespace AI
{
    public class SteerBehavior
    {
        private static readonly SteerBehavior instance = new SteerBehavior();

        private const float DecelerationTweaker = 0.3f;

        public float SafeDist { get; set; }
        public Deceleration Deceleration { get; set; }

        private SteerBehavior()
        {
            Deceleration = Deceleration.Normal;
        }

        public Vector3 Seek(AIAgent agent, Vector3 target)
        {
            Vector3 desireDir = target - agent.Position;
            float maxSpeed = agent.MaxSpeed;
            if (desireDir.sqrMagnitude > 0.1f)
            {
                Vector3 result = desireDir - agent.Velocity;
                return result;
            }
            return Vector3.zero;
        }

        public Vector3 Flee(AIAgent agent, Vector3 target)
        {
            float distToTarget = Vector3.SqrMagnitude(target - agent.Position);
            float panicDist = Mathf.Pow(agent.Radius + SafeDist, 2);

            if (distToTarget < panicDist)
            {
                Vector3 desireVel = -Seek(agent, target);
                // scale the force inversely proportional to the agent's target
                desireVel /= Mathf.Sqrt(distToTarget);
                return desireVel;
            }
            return Vector3.zero;
        }

        public Vector3 Arrive(AIAgent agent, Vector3 target)
        {
            Vector3 toTarget = target - agent.Position;
            float dist = toTarget.sqrMagnitude;
            if (dist > 0)
            {
                float speed = dist / ((float)Deceleration * DecelerationTweaker);
                speed = Mathf.Min(speed, agent.MaxSpeed);
                toTarget = toTarget * speed / Mathf.Sqrt(dist);

                Vector3 desireVel = toTarget - agent.Velocity;
                return desireVel;
            }
            return Vector3.zero;
        }

        public static SteerBehavior Instance { get { return instance; } }
    }
}
