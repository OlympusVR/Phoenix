using UnityEngine;
using System.Collections;

namespace Phoenix
{
    public class LevelAnimations : MonoBehaviour
    {

        private GameObject _panels;
        private Animator levelAnims;
        private SoundEffects levelSounds;
        public Transform target;


        void Awake()
        {
            levelSounds = GetComponent<SoundEffects>();
            levelAnims = GetComponent<Animator>();
            _panels = GameObject.Find("Panel");
         
        }

        //Might have to be coroutine, because opeing and closing shutters and other level anims will be in here, and only using once instance, so can't have main thread stuch in here, unable to do other anims, problem with I don't want to do iENum without something meaningful to yield it/return.
        public IEnumerator MovePanels(Vector3 playerPos)
        {

            int panelSpeed = 5;
            do
            {
                transform.RotateAround(target.position, transform.up, panelSpeed * Time.deltaTime);
            } while ((playerPos.sqrMagnitude - transform.position.sqrMagnitude) > 1.5f);
            //This isn't meaningful condition, and bool that's set right before it isn't either. But it's not useless 0 seconds atleast lol, I just need it to be seperate thread, and ahven't looked into threads in c# yet, so unity's built in coroutine will have to suffice.
            yield return new WaitForEndOfFrame();//Will change to wait till sound is done playing, instead. So it's actually meaningful.

        }

        //Seperate functions better, than passing in acopied argument. Won't have to check.
        //And it makes it modular to pass in, but hmm idk what's better, not passing and two seperate, but virtually the same functions, or passing in argument, I used to always to latter, hmm.

        public bool ShuttersStill
        {
            get { return (levelAnims.GetCurrentAnimatorStateInfo(0).IsName("ShuttersStill")); }
        }

        public void openShutters()
        {
           
            //It would be two arguments unless I add a check, hmmmm fuck it I'll do differntly seprate for both for now as switch to what I usually do, thne change again whenever, just get shit working.
            levelAnims.SetTrigger("OpenShutters");

            //Don't want delay on this, one this is best to paly during time.
            levelSounds.playShutterSound(0,0);
        }

        public void closeShutters()
        {
            levelAnims.SetTrigger("CloseShutters");
            levelSounds.playShutterSound(1,levelAnims.GetCurrentAnimatorStateInfo(0).speed/2);
        }


    }
}