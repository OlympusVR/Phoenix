using UnityEngine;
using System.Collections;

namespace Phoenix
{
    //Not currently in use
   /* public class MagazineCollider : MonoBehaviour
    {
        public GameObject parentGun;

        void OnTriggerEnter(Collider other)
        {
            Magazine collidingMag = other.GetComponent<Magazine>();
            if (collidingMag != null && !collidingMag.isEquipped)
            {
                //means a magazine is currently touching the collider
                collidingMag.attachMagazine(true, parentGun);
            }
        }
    }*/
}

