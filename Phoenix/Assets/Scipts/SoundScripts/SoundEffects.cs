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
          
        }

        // Use this for initialization
        //was string for anims cause needed but for this
        public void playSoundEffect(char clipToPlay)
        {
            gunEffectToPlay[clipToPlay].Play();
        }
    }
}