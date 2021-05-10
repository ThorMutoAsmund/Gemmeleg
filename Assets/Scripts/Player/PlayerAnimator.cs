using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gemmeleg
{
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField] private AnimationClip runAnimation;
        [SerializeField] private AnimationClip walkAnimation;
        [SerializeField] private AnimationClip idleAnimation;

        private Animator animator;

        private void Start()
        {
            this.animator = GetComponent<Animator>();
        }

        private void Update()
        {
        }

        public void Walk()
        {
            //this.animator
        }

        public void Run()
        {

        }

        public void Idle()
        {

        }
    }
}