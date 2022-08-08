using System;
using UnityEngine;

namespace Helpers
{
    public class CollisionHandler
    {
        public static Vector3 CheckForBounds(Transform obj, float raycastDistance, Collider[] collidersInRange,
            RaycastHit[] hit, Vector3 direction)
        {
            var boundsAmount = Physics.OverlapSphereNonAlloc(obj.position, raycastDistance, collidersInRange, 1 << 7);

            if (boundsAmount == 0)
            {
                return new Vector3(10f, 10f, 10f);
            }

            var outDirection = direction;
            direction.Normalize();

            foreach (var collider in collidersInRange)
            {
                if (collider == null)
                {
                    continue;
                }

                var position = obj.position;
                var positionDelta = collider.transform.position - position;
                var ray = new Ray(position, positionDelta.normalized);

                if (Physics.RaycastNonAlloc(ray, hit, positionDelta.magnitude, 1 << 7) > 0)
                {
                    var normal = new Vector3(
                        Mathf.Clamp(hit[0].normal.x, -Mathf.Abs(direction.x), Mathf.Abs(direction.x)),
                        Mathf.Clamp(hit[0].normal.y, -Mathf.Abs(direction.y), Mathf.Abs(direction.y)),
                        Mathf.Clamp(hit[0].normal.z, -Mathf.Abs(direction.z), Mathf.Abs(direction.z)));

                    var xSign = direction.x == 0 ? 0f : Mathf.Sign(direction.x);
                    var ySign = direction.y == 0 ? 0f : Mathf.Sign(direction.y);
                    var zSign = direction.z == 0 ? 0f : Mathf.Sign(direction.z);

                    if (Vector3.Angle(normal, direction) <= 90)
                    {
                        continue;
                    }

                    outDirection -= new Vector3(Mathf.Abs(normal.x) * xSign, Mathf.Abs(normal.y) * ySign,
                        Mathf.Abs(normal.z) * zSign);
                }
            }
            
            Array.Clear(collidersInRange, 0, collidersInRange.Length);
            Array.Clear(hit, 0, hit.Length);
            return outDirection;
        }
    }
}