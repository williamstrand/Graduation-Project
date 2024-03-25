using UnityEngine;

namespace WSP.VFX
{
    public class VfxAnimator : VfxObject
    {
        [SerializeField] Animator animator;

        public override void Play()
        {
            animator.Play("Vfx");
        }

        void Finish()
        {
            OnFinished?.Invoke();
        }
    }
}