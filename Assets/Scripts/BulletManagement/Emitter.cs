using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Emitts Bullet types. Each Emitter dictates the bullet speed.
 * Each bullet is responsible for knowing what speed it should go
 * as each GameObject must be updated by Unity so this is the most effective.
 */
public class Emitter : MonoBehaviour
{
    // We encode speed as a Vector so that some bullets are faster
    // one way than another.
    public Vector3 bulletSpeed = new Vector3(1, 1, 1);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
