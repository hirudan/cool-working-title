using UnityEngine;

namespace Movement
{
    public class SplitDirection: SlipTimeMover
    {
        // Vectors describing the directions of movement
        public Vector3 direction1 = new Vector3(0, 1, 0);
        public Vector3 direction2 = new Vector3(1, 0, 0);
        
        // How long the first movement pattern runs
        public float timeBeforeSwitch = 3f;
        
        // How long to pause before switching directions
        public float timeBetweenDirections = 3f;
        
        // Used to scale the direction vectors
        public Vector3 velocity1 = new Vector3(1, 1, 1);
        public Vector3 velocity2 = new Vector3(1, 1, 1);
        
        // How fast the actor should spin while traveling
        public double rpm = 0;

        private float timeAlive = 0f;

        private Vector3 translateVect;
        // Update is called once per frame
        private void Update()
        {
            // Compute how many degrees should be rotated through in dT based on RPM
            // degress = rpm / 60 * 360 == rpm * 6
            var degreesToRotate = rpm * 6;
            if (timeAlive < timeBeforeSwitch)
            {
                translateVect = direction1.normalized * Time.deltaTime;
                translateVect.Scale(velocity1);
            }
            else if (timeAlive > timeBeforeSwitch + timeBetweenDirections)
            {
                translateVect = direction2.normalized * Time.deltaTime;
                translateVect.Scale(velocity2);
            }
            else
            {
                translateVect = Vector3.zero;
            }

            timeAlive += Time.deltaTime;
            this.SetMovement(translateVect, (float)degreesToRotate);
        }
    }
}
