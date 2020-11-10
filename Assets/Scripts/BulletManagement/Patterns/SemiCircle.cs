using System;
using UnityEngine;

namespace BulletManagement.Patterns
{
    /*
     * Emits concentric rings of bullets that subtend a specified angle
     */
    public class SemiCircle : BulletPattern
    {
        // The angle that subtends the bullet arc
        public float arcAngle = 150f;
        // The angle to rotate the pattern (e.g. 270 = straight down)
        public int offsetAngle = 270;
        // The number of bullets per arc
        public int density = 5;
        public override Vector3 GetTranslation(double d, int bulletId)
        {
            float c = Mathf.Deg2Rad * (arcAngle / (density + 1) * (bulletId + 1) + offsetAngle);
            var dx = Mathf.Cos(c);
            var dy = Mathf.Sin(c);
            return new Vector3(dx, dy, 0);
        }
    }
}