using UnityEngine;

namespace BulletManagement.Patterns
{
    public class Line: BulletPattern
    {
        public float offset = 0f;

        public float spreadAngle = 0f;

        public override Vector3 GetInitialPosition(int bulletId)
        {
            var c = 1/offset * bulletId;
            return new Vector3(Mathf.Sin(spreadAngle) * bulletId, c + Mathf.Cos(spreadAngle) * bulletId, 0);
        }

        public override Vector3 GetTranslation(double time, int bulletId)
        {
            return Vector3.up;
        }
    }
}
