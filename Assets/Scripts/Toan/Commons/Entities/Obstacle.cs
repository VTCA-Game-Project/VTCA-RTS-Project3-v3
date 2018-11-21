using Manager;
using UnityEngine;

namespace Common.Entity
{
    public class Obstacle : GameEntity
    {
        private MeshRenderer meshRenderer;

#if UNITY_EDITOR
        public bool Debug;
#endif
        public float BoundRadius { get; set; }

        public override bool IsDead
        {
            get;
            protected set;           
        }

        public int Index;
        private void Awake()
        {
            meshRenderer = GetComponent<MeshRenderer>();
            BoundRadius = meshRenderer.bounds.extents.x;
            StoredManager.AddObstacle(this);
        }

        private void OnDestroy()
        {
            StoredManager.RemoveObstacle(this);
        }
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (Debug)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, BoundRadius);
            }
        }
#endif
    }

}
