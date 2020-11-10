using UnityEngine;

namespace BulletManagement.Patterns
{
    /**
 * Emits a pattern from the center in a straight line with 8 spokes.
 */
    public class OctoStraight : BulletPattern
    {
        public override Vector3 GetTranslation(double time, int bulletId)
        {
            float spokeCount = bulletId % 8;

            float c = (Mathf.PI / 4) * spokeCount;
            // Derivative functions with respect to time
            float dx = Mathf.Cos(c);
            float dy = Mathf.Sin(c);

            return new Vector3(dx, dy, 0);
        }
    }
}
