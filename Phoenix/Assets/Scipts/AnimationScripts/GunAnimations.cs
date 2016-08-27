using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Phoenix
{
    public class GunAnimations : MonoBehaviour
    {
        
        Animator _gunAnimations;
        string _gunToAnimate;
        static Dictionary<string, int> animGunParams = new Dictionary<string, int>()
        {
            {"Revolver",Animator.StringToHash("Revolver") }
        };

        public string GunToAnimate
        {
            get { return _gunToAnimate; }
            set { _gunToAnimate = value; }
        }
        public float getAnimSpeed
        {
            get { return _gunAnimations.speed / 2; }
        }
        public void playShootAnim()
        {
            //If it's still playing then restart the current animation
            if (AnimationStillPlaying)
            {
                _gunAnimations.Play(GunToAnimate, -1, 0f);
               
            }
            else
                _gunAnimations.SetTrigger(animGunParams[GunToAnimate]);
        }
        // Use this for initialization
        void Awake()
        {
            _gunAnimations = GetComponent<Animator>();
        }
        bool AnimationStillPlaying
        {
            get { return (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName(GunToAnimate)); }
        }
        
      
     
    }
}