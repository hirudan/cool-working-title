using UnityEngine;

namespace Movement
{
    public class Mover : MonoBehaviour
    {
        private Animator animator;

        private void Start()
        {
            // Always require an animator for anything that moves
            // Animators can be set to a no-op. Much more faster
            // than checking for a null.
            animator = gameObject.GetComponent<Animator>();
        }

        public void SetMovement(Vector3 translate)
        {
            animator.SetFloat("HorizontalMovement", translate.x);
            animator.SetFloat("VerticalMovement", translate.y);
            transform.Translate(translate);
        }
    }
}
