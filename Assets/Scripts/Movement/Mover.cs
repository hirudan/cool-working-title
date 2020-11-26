using UnityEngine;

namespace Movement
{
    public class Mover : MonoBehaviour
    {

        public bool snapToCameraBorder = false;
        private Animator animator;

        protected void Start()
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

            // Triggers used for exiting condition
            if (translate.x == 0)
                animator.SetTrigger("NoHorizontalMovement");
            else
                animator.ResetTrigger("NoHorizontalMovement");

            transform.Translate(translate);

            // Snap object to camera if toggled
            if (snapToCameraBorder)
            {
                Vector3 cameraPos = Camera.main.WorldToViewportPoint(transform.position);
                cameraPos.x = Mathf.Clamp01(cameraPos.x);
                cameraPos.y = Mathf.Clamp01(cameraPos.y);
                transform.position = Camera.main.ViewportToWorldPoint(cameraPos);
            }
        }
    }
}
