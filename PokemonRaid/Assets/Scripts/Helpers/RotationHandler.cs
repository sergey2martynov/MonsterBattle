using UnityEngine;

namespace Helpers
{
    public class RotationHandler
    {
        public static void Rotate(Transform obj, Vector3 point)
        {
            var destinationRotation = Quaternion.LookRotation(point, Vector3.up);
            obj.rotation = Quaternion.RotateTowards(obj.rotation, destinationRotation, 720 * Time.deltaTime);
        }

        /// <summary>
        /// Rotates obj to point
        /// </summary>
        /// <param name="point">Point to rotate at</param>
        /// <param name="obj">Obj that should be rotated</param>
        /// <param name="divider"> Requires 2 if rotation should be done immediately, otherwise requires 2 / deltaTime</param>

        public static void RotateProjectile(Transform point, Transform obj, float divider)
        {
            var angle = CalculateAngle(point, obj) * Mathf.PI / 180;

            if (Mathf.Abs(angle) < 0.01f)
            {
                return;
            }

            var rotation = new Quaternion(0f, Mathf.Sin(angle / divider), 0f, Mathf.Cos(angle / divider));
            obj.rotation *= rotation;
        }

        private static float CalculateAngle(Transform point, Transform obj)
        {
            if ((point.position - obj.position).magnitude >= 0.1f)
            {
                return Vector3.SignedAngle(obj.forward, point.position - obj.position, Vector3.up);
            }

            return 0;
        }
    }
}