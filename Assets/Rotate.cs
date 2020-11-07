using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Move the cube in the positive X axis
        // deltaTime
        // start_time = time.get()
            // update() game
        // end_time = time.get()
        // delta = end_time - start_time
        // delta ms per frame
        transform.position = transform.position + new Vector3(Time.deltaTime, 0, 0);
    }

    void LateUpdate()
    {
        // Called after all the updates are called
    }
}
