using UnityEngine;
using System.Collections;

namespace Phoenix
{
    public class LevelAnimations : MonoBehaviour
    {

        private GameObject _panels;
        private GameObject player;
        private Animator levelAnims;
        private SoundEffects levelSounds;


        void Awake()
        {
            levelSounds = GetComponent<SoundEffects>();
            levelAnims = GetComponent<Animator>();
            _panels = GameObject.Find("Panel");
         
        }

        //Might have to be coroutine, because opeing and closing shutters and other level anims will be in here, and only using once instance, so can't have main thread stuch in here, unable to do other anims, problem with I don't want to do iENum without something meaningful to yield it/return.
        public IEnumerator MovePanels(char state)
        {
            

            //For now just timer, cause fuck it.
            float rotateTime = 0;
            float panelSpeed = 0;
            if (state == 's')
                panelSpeed = 35.0f;
            else if (state == 'e')
                panelSpeed = 20.0f;
            //just rotate y.
            do
            {

                rotateTime += Time.deltaTime;
                _panels.transform.Rotate(new Vector3(0, Time.deltaTime * panelSpeed, 0));
                yield return new WaitForEndOfFrame();
            } while (rotateTime < 5.0f);


        }

    

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