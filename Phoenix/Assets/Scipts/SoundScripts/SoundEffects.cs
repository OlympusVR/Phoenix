using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Phoenix
{
    //Thought bout interface, but the function to play is too modular so no diff in implemtnation, could put interface for Playfunction that this and animations take.
    public class SoundEffects : MonoBehaviour
    {

        //Thinking bout just checking bools of gun for when to play these

        //Shoot is s, Engage is e, Need to engage is N
        Dictionary<char, AudioSource> gunEffectToPlay;
        AudioSource[] shutterSounds;

        

        void Awake()
        {
            shutterSounds = new AudioSource[2];
            //opening shutter sound
            //They clearly have the component, so wtf why?
            shutterSounds[0] = GameObject.Find("Shutters_Bot").GetComponent<AudioSource>();
            //closing shutter sound
            shutterSounds[1] = GameObject.Find("Shutters_Top").GetComponent<AudioSource>();
            gunEffectToPlay = new Dictionary<char, AudioSource>();
         for (int i = 0; i < transform.childCount; i++)
            {
                //FORGOT TO ADD TAGS.
                if (transform.GetChild(i).CompareTag("EngageSound"))
                    gunEffectToPlay['e'] = transform.GetChild(i).GetComponent<AudioSource>();
                if (transform.GetChild(i).CompareTag("ShootSound"))
                    gunEffectToPlay['s'] = transform.GetChild(i).GetComponent<AudioSource>();
                if (transform.GetChild(i).CompareTag("NeedEngageSound"))
                    gunEffectToPlay['n'] =  transform.GetChild(i).GetComponent<AudioSource>();
            }
        }
        void Start()
        {
            //Why are you null? You were working just fucking fine
            shutterSounds[0] = GameObject.Find("Shutters_Bot").GetComponent<AudioSource>();

        }

        private void stopSound(AudioSource[] sounds)
        {
            foreach (var x in sounds)
            {
                if (x.isPlaying)
                    x.Stop();
            }

        }
        // Use this for initialization
        //was string for anims cause needed but for this
        public void playSoundEffect(char clipToPlay)
        {
            gunEffectToPlay[clipToPlay].Play();
        }
        
        public void playShutterSound(int index, float delay)
        {
            //That way sound won't keep playing when go from start to close, or wont make slam sound when opening.
            stopSound(shutterSounds);
            //The delay is equal to anim speed, if didn't have seperate like this could've just used that var, but now need to change in two places
            //But maybe this seperation is better even if can't use the var, hate hard numbers like this, but eh. Or could just pass in param of it
            shutterSounds[index].PlayDelayed(delay);
            
        }
    }
}