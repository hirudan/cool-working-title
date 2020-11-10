using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BulletManagement.Patterns
{
    public class Encircle : BulletPattern
    {
        public float circleInterval = 5f;
    
        public override Vector3 GetInitialPosition(int bulletId)
        {
            return new Vector3(circleInterval, 0, 0);
        }
        public override Vector3 GetTranslation(double time, int bulletId) 
        {
            int circleNum = bulletId + 1;
            double c = System.Math.PI * circleNum * 2;
            float radius = circleNum * circleInterval;
            float dx = (float) (c * (-System.Math.Sin(time * c)) * radius);
            float dy = (float) (c * System.Math.Cos(time * c) * radius);
            return new Vector3(dx, dy, 0);
        }
    }
}
