using System;
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
            var a = dTheta * time;
            var dx = Math.Cos(a) - Math.Sin(Math.Pow(a,2));
            var dy = Math.Sin(a) + Math.Cos(Math.Pow(a,2));
            
            return new Vector3((float)dx, (float)dy, 0);
        }
    }
}
