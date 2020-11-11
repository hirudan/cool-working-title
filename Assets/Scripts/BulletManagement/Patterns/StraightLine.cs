using System;
using UnityEngine;

namespace BulletManagement.Patterns
{
    public class StraightLine : BulletPattern
    {
        public float offset = 0f;

        public float acceleration = 0f;
        
        public override Vector3 GetInitialPosition(int bulletId)
        {
            var c = 1/offset * bulletId;
            return new Vector3(0, c, 0);
        }

        public override Vector3 GetTranslation(double time, int bulletId)
        {
            return new Vector3(acceleration * (1 + Time.deltaTime), acceleration * (1+Time.deltaTime), 0);
        }
    }
}
