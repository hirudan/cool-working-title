using UnityEngine;

namespace BulletManagement.Patterns
{
    public class Spiral : BulletPattern
    {

        // Quantity in degrees. Controls how far apart each bullet is
        public int angleBetweenSpokes = 30;

        // Quantity in degrees. Controls how tight the spiral is.
        public int spin = 20;
        
        public override Vector3 GetInitialPosition(int bulletId)
        {
            float c = Mathf.Deg2Rad * angleBetweenSpokes * (bulletId+1);
            return new Vector3(Mathf.Cos(c), Mathf.Sin(c), 0);
        }
        
        // Roughly traces an Archimedean spiral
        public override Vector3 GetTranslation(double time, int bulletId)
        {
            var dTheta = Mathf.Deg2Rad * spin;
            var initialPosition = GetInitialPosition(bulletId);
            var dx = Mathf.Cos(dTheta * (float) time) - Mathf.Sin(dTheta * (float)time)*(dTheta*(float)time);
            var dy = Mathf.Sin(dTheta * (float) time) + Mathf.Cos(dTheta * (float)time)*(dTheta*(float)time);
            
            return new Vector3(dx, dy, 0);
        }
    }
}