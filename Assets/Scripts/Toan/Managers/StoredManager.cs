using System.Collections.Generic;
using UnityEngine;
using Common.Entity;
using Common;

namespace Manager
{
    public class StoredManager
    {
        private static readonly StoredManager instance = new StoredManager();
        private static List<Obstacle> obstacles = new List<Obstacle>();

        private StoredManager() { }
        #region Properties
        public List<Obstacle> Obstacles
        {
            get { return obstacles; }
            protected set { obstacles = value; }
        }
        public static StoredManager Instance { get { return instance; } }
        #endregion


        // obstacle
        public static void AddObstacle(Obstacle obs)
        {
            obstacles.Add(obs);
        }
        public static void RemoveObstacle(Obstacle obs)
        {
            int index = obstacles.IndexOf(obs);
            if (index >= 0)
                obstacles.RemoveAt(index);
        }
        public static Obstacle[] GetObstacle(AIAgent agent)
        {
            List<Obstacle> result = new List<Obstacle>();
            for (int i = 0; i < obstacles.Count; i++)
            {
                if (Vector3.Distance(agent.Position, obstacles[i].Position) <= agent.DetectBoxLenght)
                {
                    result.Add(obstacles[i]);
                }
            }
            return result.ToArray();
        }
    }
}
