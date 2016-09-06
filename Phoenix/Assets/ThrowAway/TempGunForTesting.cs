using UnityEngine;
using System.Collections;
using System;

namespace Phoenix
{
    public class TempGunForTesting : MonoBehaviour
    {
        // Because all guns will have to have    {
        #region GUN Flags

        //Reference to struct, is in Weapon class normally, but weapon class has nvr so just making here.
        public GameConstants.WeaponType weaponType;

        [Header("Fields below from Gun class")]
        [Space(15)]

        //Made visible in inspector to test each one individually, since choosing weaponType has combinations.

        //bullet count will never go down.
        public bool isInfinite;

        //This is for if some guns need engagement, keeping here incase we decide to re-implement it but for now it is not used.
        public bool needsEngagment = false;

        //true if the player can just press and hold to shoot.
        public bool isAutomatic;
       
        //true if player can press and hold and charge up a shot.
        public bool isCharge = false;

        //true if the gun needs to re-engage after every shot false otherwise
        public bool isRepeater = false;
        #endregion


        #region AUTOMATIC VARIABLES
        private float timeBetweenShots = 0.5f;
        private float timeSinceLastShot = 0;
        #endregion

        private bool _isEngaged = false;
        private SlideEngage SlideEngage;
        public Transform firePoint;

        private SoundEffects gunSounds;
        private GunAnimations gunAnim;
        //Just doing here since only reason for mag class now would be bullets, might as well put in here.
        #region BULLET VARS
        GameObject bulletPrefab;
        bool currentlyShooting;
        private int _amountOfBullets;
        protected int _maxBullets;
        #endregion

        #region HAPTIC FEEDBACK VARS
        float VibrationLength = 0.1f;
        ushort VibrationIntensity = 2000;
        #endregion

        #region Properties

        //Made into properties just incase need either of these to be public for whatever reason, like current bullets for UI or whatever.
        bool IsEngaged
        {
            //If it is infinite ammo it will never need engagement(Subject to change)
            get { return _isEngaged; }
            set { _isEngaged = value; }
        }

        int currentBullets
        {
            get { return _amountOfBullets; }
            set { _amountOfBullets = value; }
        }

        #endregion

        #region Unity Methods
        protected void Awake()
        {
            gunAnim = GetComponent<GunAnimations>();
            
            bulletPrefab = Resources.Load("Prefabs/BulletPrefabs/Bullet") as GameObject;
        }
        protected virtual void Start()
            //there's a speed issue, it gets component and happens before all other shit is ready to play.
        {   gunSounds = GetComponent<SoundEffects>();
            //before doing anything, we should initialize the flag values for the gun or next statement will not work
            gunAnim.GunToAnimate = gameObject.name;

            timeBetweenShots = gunAnim.getAnimSpeed;
            initWeaponFlags(GameConstants.gunTypeInitValues[weaponType]);

            //Just to test, prob change depending on gun.
            _maxBullets = 6;

            if (!IsEngaged)
            {
                gunNeedsEngage();
                /*SlideEngage = GetComponentInChildren<SlideEngage>();
                if (SlideEngage != null)
                    //Not using the slideEngage function here because it requires NVR stuff.
                    engageGun();*/
            }

        }


        protected void Update()
        {
            
            if (timeSinceLastShot < timeBetweenShots)
                timeSinceLastShot += Time.deltaTime;
        }
        #endregion


        #region Methods
        void initWeaponFlags(GunTypeInitValues _gunInitValues)
        {
            isInfinite = _gunInitValues.isInfinite;
            isAutomatic = _gunInitValues.isAutomatic;
            isCharge = _gunInitValues.isCharge;
            needsEngagment = _gunInitValues.needsEngagment;
            isRepeater = _gunInitValues.isRepeater;
        }

        public void engageGun()
        {
            
            gunSounds.playSoundEffect('e');
            gunAnim.playGunAnim("Engage");
            currentBullets = _maxBullets;
            IsEngaged = true;
        }

        private void gunNeedsEngage()
        {
            IsEngaged = false;
            gunSounds.playSoundEffect('n');
            gunAnim.playGunAnim("NeedEngage");
            
        }
        
        protected GameObject spawnBullet()
        {
            GameObject bullet = Instantiate(bulletPrefab);
            return bullet;
        }
        virtual protected IEnumerator shootGun()
        { 
            
            gunAnim.playGunAnim("Shoot");
            gunSounds.playSoundEffect('s');
            //Sets currentlyShooting to true. This is to make sure they don't shoot more than
            //max bullets. Because there are separate threads calling this it will shoot off multiple bullets 
            //before it hits the line for setting engaged to false because other threads are
            //still in here
            
            currentlyShooting = true;

            //This delay puts shooting and animation in sync.
            yield return new WaitForSeconds(gunAnim.getAnimSpeed);
            GameObject tempBullet = spawnBullet();
            tempBullet.transform.position = firePoint.position;
            tempBullet.transform.rotation = firePoint.rotation;
            tempBullet.GetComponent<Bullet>().initialize();

            if (!isInfinite)
            {
                --currentBullets;
                if (currentBullets == 0)
                    gunNeedsEngage();
            }
            tempBullet = null;
            
            if (isRepeater)
                IsEngaged = false;
            
            //This thread ends here so I set this back to false so future threads can 
            //call this function.
            currentlyShooting = false;

        }

        // the mechanics for the automatic guns        
        public void autoGunMechanics()
        {

            if (IsEngaged)
            {  //Since they can fire while pressing down, need to force some time in between 
                //shots so that it's not one big line.
                if (timeSinceLastShot >= timeBetweenShots)
                {
                    if (!currentlyShooting)
                    {
                        StartCoroutine(shootGun());
                        timeSinceLastShot = 0;
                    }
                }
            }
            else
                gunSounds.playSoundEffect('n');
        }

        //the mechanics for non-automatic guns
        public void gunMechanics()
        {
            if (IsEngaged)
            {
                //If it's currently shooting then that means a separate thread is still
                //going through the shootGun function
                if (!currentlyShooting)
                    StartCoroutine(shootGun());
            }
            else
                gunSounds.playSoundEffect('n');
        }
        #endregion


    } // End of gun class
}

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



