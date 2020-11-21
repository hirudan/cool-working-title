using UnityEngine;

namespace Movement
{
    public class SplitDirection: SlipTimeMover
    {
        public Vector3 direction1 = new Vector3(0, 1, 0);
        public Vector3 direction2 = new Vector3(1, 0, 0);
        public float timeBeforeSwitch = 3f;
        public float timeBetweenDirections = 3f;
        public Vector3 velocity1 = new Vector3(1, 1, 1);
        public Vector3 velocity2 = new Vector3(1, 1, 1);

        private float timeAlive = 0f;

        private Vector3 translateVect;
        // Update is called once per frame
        private void Update()
        {
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
            this.SetMovement(translateVect);
        }
    }
}
