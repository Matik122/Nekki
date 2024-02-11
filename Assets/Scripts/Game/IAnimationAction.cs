using Core;
using UnityEngine;

namespace Game
{
    public interface IAnimationAction
    {
        public void SetTrigger(string triggerName);

        public void SetBool(string triggerName, bool state);
    }
    
    public class AnimationAction : IAnimationAction
    {
        private readonly Animator _animator;

        public AnimationAction(Animator animator)
        {
            _animator = animator;
        }
        
        public void SetTrigger(string triggerName) =>
            _animator.SetTrigger(Animator.StringToHash(triggerName));
        
        public void SetBool(string triggerName, bool state) =>
            _animator.SetBool(Animator.StringToHash(triggerName), state);
    }
}