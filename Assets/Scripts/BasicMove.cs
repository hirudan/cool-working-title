using UnityEngine;

public class BasicMove : MonoBehaviour
{
    public Vector3 moveDirection = new Vector3(0, 1, 0);
    public Vector3 speed = new Vector3(1, 1, 1);

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 translateVect = moveDirection.normalized * Time.deltaTime;
        translateVect.Scale(speed);
        transform.position = transform.position + translateVect;
    }
}
