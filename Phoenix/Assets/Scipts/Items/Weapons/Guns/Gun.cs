using UnityEngine;
using UnityEngine.UI;
using NewtonVR;
using System.Collections;
using System;

namespace Phoenix
{
    /// <summary>
    /// Abstract class Gun will hold all the methods, properties and Fields that 
    /// are general to ALL guns in the game
    /// </summary>
    // Because all guns will have to have a sprite to accompany the weapon for UI purposes
    [RequireComponent(typeof(Image))]
    public class Gun : Weapon
    {
        #region GUN Flags

        [Header("Fields below from Gun class")]
        [Space(15)]
        [HideInInspector]
        private bool _isInfinite = true;
        [HideInInspector]
        public bool needsEngagment = false;
        //true if the gun is automatic.
        [HideInInspector]
        public bool isAutomatic;
        // is the gun a charge type weapon
        [HideInInspector]
        public bool isCharge = false;
        [HideInInspector]
        public bool isRepeater = false;
        #endregion


        #region AUTOMATIC VARIABLES
        public float timeBetweenShots = 0.5f;
        public float timeSinceLastShot = 0;
        #endregion

        public bool _isEngaged; 
        public SlideEngage SlideEngage;
        public Transform firePoint;

        /*#region MAGAZINE VARIABLES
        public Magazine currentMagazine;
        public float secondsAfterDetach = 0.2f;
        public Transform magazinePosition;
        #endregion*/
        #region GUN ANIMATION VARS
        protected GunAnimations gunAnim;
        //Just doing here since only reason for mag class now would be bullets, might as well put in here.
        //This prefab will change depending on kind of gun, if charge or whatever. For now it's all same bullet.
        //I'm thinking of using switch case for tag of game object this script is on to determine which bullet prefab
        //to use.
        #endregion
        #region BULLET VARS
        protected GameObject bulletPrefab;
        public int _amountOfBullets;
        public int _maxBullets;
        #endregion

        #region HAPTIC FEEDBACK VARS
        float VibrationLength = 0.1f;
        ushort VibrationIntensity = 2000;
        #endregion

        #region Properties

        // Below are properties for whther the user is pressing the left or right pad buttons
        public bool rightPadButtonDown
        {
            get
            {
                return AttachedHand.Controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad) &&
                            (AttachedHand.Controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad)[0] > 0.05f);
            }
        }

        public bool leftPadButtonDown
        {
            get
            {
                return AttachedHand.Controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad) &&
                            (AttachedHand.Controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad)[0] < 0.05f);
            }
        }
        //This returns whether or not the gun is engaged.
        bool IsEngaged
        {
            get { return _isEngaged; }
            set { _isEngaged = value; }
        }
        //Amount of bullets until need to engage again.
        int CurrentBullets
        {
            get { return _amountOfBullets; }
            set { _amountOfBullets = value; }
        }
      
        bool IsInfinite
        {
            get { return _isInfinite; }
            set { _isInfinite = value; }
        }

        #endregion

        #region Unity Methods
        protected override void Awake()
        {
            base.Awake();
            gunAnim = GetComponent<GunAnimations>();
            bulletPrefab = Resources.Load("Prefabs/BulletPrefabs/Bullet") as GameObject;
        }
        protected override void Start()
        {
            //before doing anything, we should initialize the flag values for the gun or next statement will not work
            initWeaponFlags(GameConstants.gunTypeInitValues[weaponType]);

            base.Start();
            if (!IsEngaged)
            {
                SlideEngage = GetComponentInChildren<SlideEngage>();
                if (SlideEngage != null)
                    SlideEngage.setEngage(engageGun);
            }
            //Sets which gun animations to play.
            gunAnim.GunToAnimate = gameObject.name;
            //Regardless of guntype this assignment will happen, if infinite it will never hit 0 so engage will never equal false for infinite.
            CurrentBullets = _maxBullets;
            
            //This will be apart of UI which Ron is doing, so getting rid of it here.
            //We need to initialize the image, for UI.
            //initWeaponImage();
        }


        protected override void Update()
        {
            base.Update();
                 
            if (leftPadButtonDown)
            {
                SlideEngage.setEngage(engageGun);
            }
            if (timeSinceLastShot == 0)
            {
                timeSinceLastShot += Time.deltaTime;
            }
        }
        #endregion

        #region NVR Overrides
        public override void UseButtonDown()
        {
            // do not do this if the gun uses a charging mechanism instead
            //I could implement charge mechanic later, so I'll leave this as is.
            if (!isCharge)
            {
                gunMechanicsSemiAuo();
            }
        }

        //UseButtonPressed is when the button is being held down
        // thus, this only works if your na automatic weapon
        public override void UseButtonPressed()
        {
            if (isAutomatic)
                gunMechanicsAuto();
        }

        //When an autonmatic is shooting, we need to reset the time because 
        // if they release the trigger being mid shot, we dont want it to shoot early next time they shoot
        public override void UseButtonUp()
        {
            if (isAutomatic)
                timeSinceLastShot = 0;
            //Only shoot on button up when it's a charge Gun.
            if (isCharge)
                shootGun();
        }
        #endregion

        #region Methods
        //Initializes all of the bools to the corresponding gun type;
        void initWeaponFlags(GunTypeInitValues _gunInitValues)
        {
            IsInfinite = _gunInitValues.isInfinite;
            isAutomatic = _gunInitValues.isAutomatic;
            isCharge = _gunInitValues.isCharge;
            needsEngagment = _gunInitValues.needsEngagment;
            isRepeater = _gunInitValues.isRepeater;
        }

        private void engageGun()
        {
            _amountOfBullets = _maxBullets;
            IsEngaged = _amountOfBullets > 0;
        }

        /*protected void dropCurrentMagazine()
        {
            if (isLoaded)
            {
                StartCoroutine(disableMagazineCollider());
                currentMagazine.attachMagazine(false);
                currentMagazine = null;
            }

            isEngaged = false;
        }*/
      
        private GameObject spawnBullet()
        {
            GameObject bullet = Instantiate(bulletPrefab);
            return bullet;
        }
       
        virtual protected IEnumerator shootGun()
        {

            gunAnim.playShootAnim();
            //This delay puts firing bullet and animation in sync.
            yield return new WaitForSeconds(gunAnim.getAnimSpeed);
            GameObject tempBullet = spawnBullet();
            tempBullet.transform.position = firePoint.position;
            tempBullet.transform.rotation = firePoint.rotation;
            tempBullet.GetComponent<Bullet>().initialize();
            //If it's not infinite or doesn't need engagement then minus current bullets
            if (!IsInfinite && !needsEngagment)
            {
                --CurrentBullets;
                if (CurrentBullets <= 0)
                    IsEngaged = false;
            }
                tempBullet = null;
            AttachedHand.LongHapticPulse(VibrationLength, VibrationIntensity);

           
        }



        /*  public IEnumerator disableMagazineCollider()
          {
              magazinePosition.gameObject.GetComponent<BoxCollider>().enabled = false;
              yield return new WaitForSeconds(secondsAfterDetach);
              magazinePosition.gameObject.GetComponent<BoxCollider>().enabled = true;
              StopCoroutine(disableMagazineCollider());
          }*/


        // the mechanics for semi-automatic guns
        private void gunMechanicsSemiAuo()
        {

            if (!IsEngaged)
            {
                Debug.Log("Please engage your gun");
            }
            else
            {
                    StartCoroutine(shootGun());   
            }
        }


        //The mechanics for automatic guns.
        void gunMechanicsAuto()
        {
            if (!IsEngaged)
            {
                Debug.Log("Please engage your gun");
            }
            else
            {
                //Since they can hold down trigger for automatics, need to put delay inbetween shots.
                if (timeSinceLastShot >= timeBetweenShots)
                {
                    StartCoroutine(shootGun());
                    timeSinceLastShot = 0;
                }            
            }
        }
        #endregion

    }
    // End of gun class


    //this will allow us to automate the flag value assigning for each diff. type of weapon
    public struct GunTypeInitValues
    {
        public bool isInfinite;
        public bool isAutomatic;
        public bool isCharge;
        public bool isRepeater;
        public bool needsEngagment;

        // constructor
        public GunTypeInitValues
            (bool _isInfinite = false, bool _isAutomatic = false, bool _isRepeater = false, bool _needsEnagement = false, bool _isCharge = false)
        {
            this.isInfinite = _isInfinite;
            this.isAutomatic = _isAutomatic;
            this.isRepeater = _isRepeater;
            this.needsEngagment = _needsEnagement;
            this.isCharge = _isCharge;
        }
    }

}