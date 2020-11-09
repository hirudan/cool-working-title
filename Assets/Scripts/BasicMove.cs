using UnityEngine;

// Moves object in a single direction. Great for testing movement.
public class BasicMove : MonoBehaviour
{
    public Vector3 moveDirection = new Vector3(0, 1, 0);
    public Vector3 speed = new Vector3(1, 1, 1);

    // Update is called once per frame
    void Update()
    {
        Vector3 translateVect = moveDirection.normalized * Time.deltaTime;
        translateVect.Scale(speed);
        transform.position = transform.position + translateVect;
    }
}
