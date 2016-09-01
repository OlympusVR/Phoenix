using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Phoenix
{
    public class GunAnimations : MonoBehaviour
    {
        
        Animator _gunAnimations;
        string _gunToAnimate;
        string _currentGunState;

        public string GunToAnimate
        {
            get { return _gunToAnimate; }
            set { _gunToAnimate = value; }
        }
        public float getAnimSpeed
        {
            get { return _gunAnimations.speed / 3; }
        }
        public void playGunAnim(string action)
        {
            _currentGunState = GunToAnimate + action;
            //If it's still playing then restart the shooting animation
            if (AnimationStillPlaying && action == "Shoot" && _currentGunState == "RevolverShoot")
            {
                //Added extra and for revolver shoot because if first 2 are met, don't want for example charge gun to reset.
                _gunAnimations.Play(_currentGunState, -1, 0f);

            }
                _gunAnimations.SetTrigger(Animator.StringToHash(_currentGunState));
        }

        // Use this for initialization
        void Awake()
        {
            _gunAnimations = GetComponent<Animator>();
        }
        bool AnimationStillPlaying
        {
            //Checks if the last animation we played is still happening.
            get { return (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName(_currentGunState)); }
        }
        
      
     
    }
}