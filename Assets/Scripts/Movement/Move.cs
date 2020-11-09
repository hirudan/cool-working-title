using UnityEngine;

namespace Movement
{
    public abstract class Move : MonoBehaviour
    {
        private void Update()
        {
            MoveOnUpdate();
        }

        protected abstract void MoveOnUpdate();
    }
}