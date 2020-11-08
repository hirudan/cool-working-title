using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Public Class Variables are Auto Exposed to Inspector
    public float decayTime = 1f;
    public float speed = 1f;
    public Vector3 direction = new Vector3(1, 0, 0);

    void Start()
    {
        // Destroy self in N seconds after emitt
        Destroy(this.gameObject, decayTime);
    }

    void Update()
    {
        // Move in a fixed direction until otherwise
        transform.Translate(direction * speed * Time.deltaTime);
    }
}