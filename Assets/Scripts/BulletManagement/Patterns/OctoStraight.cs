using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Emitts a pattern from the center in a straight line with 8 spokes.
 */
public class OctoStraight : BulletPattern
{
    public override Vector3 GetTranslation(float time, int bulletId)
    {
        float spokeCount = bulletId % 8;

        float c = (Mathf.PI / 4) * spokeCount;
        // Derivative functions with respect to time
        float x = Mathf.Cos(c);
        float y = Mathf.Sin(c);

        return new Vector3(x, y, 0);
    }
}
