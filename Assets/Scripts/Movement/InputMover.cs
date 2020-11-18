using UnityEngine;

namespace Movement
{
    /*
     * Behaviour that moves an object along the horizontal and vertical axes based on player input.
     */
    public class InputMover : Mover
    {
        public float Speed = 1f;

        protected new void Start()
        {
            base.Start();
            // Player should be restricted to main camera
            snapToCameraBorder = true;
        }

        private void Update()
        {
            this.SetMovement(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0) * Speed * Time.deltaTime);
        }
    }
}
