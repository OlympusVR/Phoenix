using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Phoenix
{
    public class GunSounds : MonoBehaviour
    {

        //Thinking bout just checking bools of gun for when to play these

        //Shoot is s, Engage is e, Need to engage is N
        Dictionary<char, AudioSource> gunEffectToPlay;
        void Start()
        {
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

        // Use this for initialization
        //was string for anims cause needed but for this
        public void playSoundEffect(char clipToPlay)
        {
            gunEffectToPlay[clipToPlay].Play();
        }
    }
}