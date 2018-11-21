using UnityEngine;

namespace Utils
{
    public class MathUtils
    {
        private static readonly MathUtils instance = new MathUtils();
        private MathUtils() { }

        public static MathUtils Instance { get { return instance; } }
        public static Vector3 ToLocalPoint(Transform localTrans, Vector3 point)
        {
            return localTrans.InverseTransformPoint(point);
        }
        public static Vector3 ToWorldVector(Transform localTrans, Vector3 vector)
        {
            return localTrans.TransformVector(vector);
        }
        public static bool CalculateQuadraticBetweenCircleAndXAxis(Vector2 center, float R, out float x1, out float x2)
        {
            // (a,b) = Center
            // d: y = 0
            // C: (x - a)^2 + (y - b)^2 - R^2 = 0
            // => x^2 - 2ax + a^2 + b^2 - R^2 = 0
            x1 = x2 = 0;
            float delta = Mathf.Pow(2 * center.x, 2) - 4 * (Mathf.Pow(center.x, 2) + Mathf.Pow(center.y, 2) - R * R);
            if (delta < 0) return false;
            if (delta == 0)
            {
                x1 = x2 = center.x; // - b / 2a
            }
            if (delta > 0)
            {
                x1 = (2 * center.x + Mathf.Sqrt(delta)) / 2.0f;
                x2 = (2 * center.x - Mathf.Sqrt(delta)) / 2.0f;
            }
            return true;
        }
    }
}