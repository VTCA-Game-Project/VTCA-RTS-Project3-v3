using Common.Entity;
using Manager;
using UnityEngine;
using Utils;

namespace AI
{
    public class ObstacleAvoidance
    {
        private static readonly ObstacleAvoidance instance = new ObstacleAvoidance();
        private const float BrakingForce = 0.2f;
        private float agentRadius;
        private float detectBoxLenght;
        private Obstacle closestObs = null;
        private Vector3 localPosObstacle;
        private Vector3 closestLocalPosition;

        private ObstacleAvoidance() { }
        public static ObstacleAvoidance Instance { get { return instance; } }

        public float CalculateDetectBoxLenght(AIAgent agent)
        {
            return agent.MinDetectionBoxLenght
                + (agent.AgentRigid.velocity.sqrMagnitude / (agent.MaxSpeed * agent.MaxSpeed))
                * agent.MaxSpeed;
        }
        public Vector3 GetObsAvoidanceForce(AIAgent agent, Obstacle[] obstacles)
        {
            closestObs = null;
            detectBoxLenght = agent.DetectBoxLenght;
            agentRadius = agent.Radius;
            closestLocalPosition = Vector3.negativeInfinity;

            // find losed obstacle
            localPosObstacle = Vector3.negativeInfinity;
            float distToClosest = float.MaxValue;
            float dist = 0.0f;

            for (int i = 0; i < obstacles.Length; i++)
            {
                localPosObstacle = MathUtils.ToLocalPoint(agent.transform, obstacles[i].Position);
                if (localPosObstacle.z > 0)
                {
                    float expandRadius = (obstacles[i].BoundRadius + agentRadius);
                    if (localPosObstacle.x < expandRadius)
                    {
                        float x1, x2;
                        MathUtils.CalculateQuadraticBetweenCircleAndXAxis(
                               new Vector2(closestLocalPosition.z, closestLocalPosition.x),
                               expandRadius, out x1, out x2);
                        dist = x1;
                        if (dist <= 0)
                        {
                            dist = x2;
                        }
                        if (dist < distToClosest)
                        {
                            distToClosest = dist;
                            closestObs = obstacles[i];
                            closestLocalPosition = localPosObstacle;
                        }
                    }

                }
            }
            if (closestObs)
            {
                float multiplier = 1 + (dist / detectBoxLenght);
                float xF = (closestObs.BoundRadius - closestLocalPosition.x) * multiplier;
                float zF = (closestObs.BoundRadius - closestLocalPosition.z) * BrakingForce;
                return MathUtils.ToWorldVector(agent.transform, new Vector3(xF, 0, zF));
            }
            return Vector3.zero;
        }
    }
}
