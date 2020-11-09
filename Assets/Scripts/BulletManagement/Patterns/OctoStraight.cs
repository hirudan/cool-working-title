using UnityEngine;

namespace BulletManagement.Patterns
{
    /// <summary>
    /// Emits a pattern from the center in a straight line with 8 spokes.
    /// </summary>
    public class OctoStraight : BulletPattern
    {
        public override Vector3 GetTranslation(float time, int bulletId)
        {
            float spokeCount = bulletId % 8;

            var c = Mathf.PI / 4 * spokeCount;
            // Derivative functions with respect to time
            var dx = Mathf.Cos(c);
            var dy = Mathf.Sin(c);

            return new Vector3(dx, dy, 0);
        }
    }
}
