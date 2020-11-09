using UnityEngine;

namespace Movement
{
    /// <summary>
    /// Behaviour that moves an object along the horizontal and vertical axes based on player input.
    /// </summary>
    public class InputMove : MonoBehaviour
    {
        private const float Speed = 1f;

        private void Update()
        {
            transform.position +=
                new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0) * (Speed * Time.deltaTime);
        }
    }
}
