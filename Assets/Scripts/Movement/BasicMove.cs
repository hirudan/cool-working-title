using UnityEngine;

namespace Movement
{
    /// <summary>
    /// Behaviour that moves an object in a single direction. Great for testing movement.
    /// </summary>
    public class BasicMove : MonoBehaviour
    {
        public Vector3 moveDirection = new Vector3(0, 1, 0);

        public Vector3 velocity = new Vector3(1, 1, 1);

        private void Update()
        {
            var translateVect = moveDirection.normalized * Time.deltaTime;
            translateVect.Scale(velocity);
            transform.position += translateVect;
        }
    }
}
