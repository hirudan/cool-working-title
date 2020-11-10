using UnityEngine;

public class StarExpand : BulletPattern
{
    public int sideLength = 5;

    public override Vector3 GetInitialPosition(int bulletId)
    {
        float y = -(Mathf.Sin(72f * Mathf.PI / 180) * sideLength) / 2;
        float x = -(Mathf.Sin(18f * Mathf.PI / 180) * sideLength);
        return new Vector3(x, y, 0);
    }

    public override Vector3 GetTranslation(double time, int bulletId)
    {
        // Track which side of the star to draw
        int sideTrack = ((int) time) / sideLength;

        // Depending on the side, switch degrees
        double degrees;
        switch (sideTrack)
        {
            case 0:
                degrees = 72;
                break;
            case 1:
                degrees = -72;
                break;
            case 2:
                // deg: 180 - 72 + 32
                degrees = 140;
                break;
            case 3:
                degrees = 0;
                break;
            default:
                degrees = -140;
                break;
        }

        double degreesRad = (System.Math.PI / 180) * degrees;
        float dx = (float) System.Math.Cos(degreesRad);
        float dy = (float) System.Math.Sin(degreesRad);

        return new Vector3(dx, dy, 0);
    }
}
