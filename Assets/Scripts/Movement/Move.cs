using UnityEngine;

namespace Movement
{
    /// <summary>
    /// Base movement class.
    /// </summary>
    public abstract class Move : MonoBehaviour
    {
        private void Update()
        {
            MoveOnUpdate();
        }

        /// <summary>
        /// Movement actions to take when OnUpdate is called.
        /// </summary>
        protected abstract void MoveOnUpdate();
    }
}
