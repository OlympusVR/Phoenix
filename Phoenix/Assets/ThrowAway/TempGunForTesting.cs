using UnityEngine;
using System.Collections;

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
        //can continuously shoot bullets without re-ngaging on true, else need to re-engage.
        public bool isInfinite;

        //This is for if some guns need engagement, keeping here incase we decide reimplement it but for now it is not used I'm going to take it out.
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

        protected GunAnimations gunAnim;
        //Just doing here since only reason for mag class now would be bullets, might as well put in here.
        #region BULLET VARS
        GameObject bulletPrefab;
        bool currentlyShooting;
        public int _amountOfBullets;
        protected int _maxBullets;
        #endregion

        #region HAPTIC FEEDBACK VARS
        float VibrationLength = 0.1f;
        ushort VibrationIntensity = 2000;
        #endregion

        #region Properties

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
        {
            //before doing anything, we should initialize the flag values for the gun or next statement will not work
            initWeaponFlags(GameConstants.gunTypeInitValues[weaponType]);
           
            //Just to test, prob change depending on gun.
            _maxBullets = 12;
            if (!IsEngaged)
            {
                SlideEngage = GetComponentInChildren<SlideEngage>();
                if (SlideEngage != null)
                    //Not using the slideEngage function here because it requires NVR stuff.
                    engageGun();
            }

            gunAnim.GunToAnimate = gameObject.name;

            timeBetweenShots = gunAnim.getAnimSpeed;
        }


        protected void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {

                gunMechanics();
            }
            if (Input.GetKey(KeyCode.F))
            {
                if (isAutomatic)
                    autoGunMechanics();
                
            }
            if (Input.GetKeyDown(KeyCode.E))
                engageGun();

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

        private void engageGun()
        {
            currentBullets = _maxBullets;
            IsEngaged = true;
        }

        
        protected GameObject spawnBullet()
        {
            GameObject bullet = Instantiate(bulletPrefab);
            return bullet;
        }
        virtual protected IEnumerator shootGun()
        { 
            gunAnim.playShootAnim();

            //Sets currentlyShooting to true. This is to make sure they don't shoot more than
            //max bullets. Because it is a detached thread it will shoot off multiple bullets 
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
                    IsEngaged = false;
            }
            tempBullet = null;
            //This thread ends here so I set this back to false so future threads can 
            //call this function.
            if (isRepeater)
                IsEngaged = false;
            currentlyShooting = false;

        }
        // the mechanics for the automatic guns
        void autoGunMechanics()
        {
            if (!IsEngaged)
            {
                Debug.Log("Please engage your gun");
            }
            else
            {
                //Since they can fire while pressing down, need to force some time in between 
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
        }

        //the mechanics for non-autmatic guns
        void gunMechanics()
        {
            if (!IsEngaged)
            {
                Debug.Log("Please engage your gun");
            }
            else
            {
                //If it's currently shooting then that means a detached thread is still
                //going through the shootGun function
                if (!currentlyShooting)
                    StartCoroutine(shootGun());
            }
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



