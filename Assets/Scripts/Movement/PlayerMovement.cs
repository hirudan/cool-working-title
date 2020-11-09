using UnityEngine;

namespace Movement
{
    public class PlayerMovement : MonoBehaviour
    {
        private const float Speed = 1f;

        // Update is called once per frame
        private void Update()
        {
            var move = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);

            transform.position += move * (Speed * Time.deltaTime);
        }
    }
}
