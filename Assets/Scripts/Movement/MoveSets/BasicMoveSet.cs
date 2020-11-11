using UnityEngine;

namespace Movement
{
    public class BasicMoveSet : SlipTimeMover
    {
        public Vector3 moveDirection = new Vector3(0, 1, 0);
        public Vector3 velocity = new Vector3(1, 1, 1);

        // Update is called once per frame
        private void Update()
        {
            Vector3 translateVect = moveDirection.normalized * Time.deltaTime;
            translateVect.Scale(velocity);
            this.SetMovement(translateVect);
        }
    }
}
