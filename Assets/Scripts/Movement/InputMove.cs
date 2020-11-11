using UnityEngine;

namespace Movement
{
    /*
     * Behaviour that moves an object along the horizontal and vertical axes based on player input.
     */
    public class InputMove : MonoBehaviour
    {
        private const float Speed = 1f;
        private Animator animator;

        private void Start()
        {
            // Always require an animator for anything that moves
            // Animators can be set to a no-op. Much more faster
            // than checking for a null.
            animator = gameObject.GetComponent<Animator>();
        }

        // Update is called once per frame
        private void Update()
        {
            var movementH = Input.GetAxis("Horizontal");
            var movementV = Input.GetAxis("Vertical");
            animator.SetFloat("HorizontalMovement", movementH);
            animator.SetFloat("VerticalMovement", movementV);

            transform.position +=
                new Vector3(movementH, movementV, 0) * (Speed * Time.deltaTime);
        }
    }
}
