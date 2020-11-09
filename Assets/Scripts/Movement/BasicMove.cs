using UnityEngine;

namespace Movement
{
    /**
     * Behaviour that moves an object in a single direction. Great for testing movement.
     */
    public class BasicMove : Move
    {
        public Vector3 moveDirection = new Vector3(0, 1, 0);

        public Vector3 velocity = new Vector3(1, 1, 1);

        protected override void MoveOnUpdate()
        {
            var translateVect = moveDirection.normalized * Time.deltaTime;
            translateVect.Scale(velocity);
            transform.position += translateVect;
        }
    }
}
