using SlipTime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBackground : MonoBehaviour, ISlipTimeAdherent
{
    public SlipTimeManager SlipTimeManager => slipTimeManager;

    [SerializeField]
    private SlipTimeManager slipTimeManager;


    /// <summary>
    /// Holds the point when the map loops and we should
    /// stop translation and jump to the top of the map instead
    /// Rather than to try to auto-guess this value, manually calculate it by hand
    /// by moving the tilemap vertically until it matches.
    /// Often than not, the number will probably work out nicely given
    /// our camera coordinates being in round numbers.
    /// </summary>
    public float loopTranslationThreshold;

    private Vector3 startLocation;

    public float speed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        startLocation = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if ((startLocation - transform.position).magnitude >= loopTranslationThreshold)
        {
            transform.position = startLocation;
        }
        else
        {
            transform.Translate(new Vector3(0, -speed * Time.deltaTime * SlipTimeManager.slipTimeCoefficient));
        }
    }
}
