using SlipTime;
using UnityEngine;

namespace Movement
{
    public class SlipTimeMover : MonoBehaviour, ISlipTimeAdherent
    {
        public SlipTimeManager SlipTimeManager
        {
            get => slipTimeManager;
            set => slipTimeManager = value;
        }

        [SerializeField]
        private SlipTimeManager slipTimeManager;
        
        private Animator animator;
        private static readonly int HorizontalMovement = Animator.StringToHash("HorizontalMovement");
        private static readonly int VerticalMovement = Animator.StringToHash("VerticalMovement");

        private void Start()
        {
            // Always require an animator for anything that moves
            // Animators can be set to a no-op. Much faster
            // than checking for a null.
            animator = gameObject.GetComponent<Animator>();
        }

        public void SetMovement(Vector3 translate)
        {
            animator.SetFloat(HorizontalMovement, translate.x);
            animator.SetFloat(VerticalMovement, translate.y);
            transform.Translate(translate * SlipTimeManager.slipTimeCoefficient);
        }
    }
}
