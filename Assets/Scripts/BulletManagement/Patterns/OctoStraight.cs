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
        int spokeCount = bulletId % 8;
        float x = Mathf.Cos((Mathf.PI / 4) * spokeCount) * time;
        float y = Mathf.Sin((Mathf.PI / 4) * spokeCount) * time;
        return new Vector3(x, y, 0);
    }
}
