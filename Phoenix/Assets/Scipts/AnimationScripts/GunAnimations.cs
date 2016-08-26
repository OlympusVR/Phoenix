using UnityEngine;
using System.Collections;
//Could just change the transforms themselves instead of animation but both work.
//This was tested in GunAnimations scene in Throwaway folder.
public class GunAnimations : MonoBehaviour
{

    Animator gunAnimations;
    GameObject bulletPrefabs;
    Transform spinner;
    public Transform firePos;
    //Created array, just incase we were going to have more GunAnimations;
    string[] animStates = new string[1]
    {
        "Shoot"
    };
	// Use this for initialization
    void Awake()
    {
        spinner = GameObject.Find("Revolver_Spinner").GetComponent<Transform>();
        bulletPrefabs = Resources.Load("Prefabs/BulletPrefabs/Bullet") as GameObject;
        gunAnimations = GetComponent<Animator>();
    }
    /*Did for testing and seeing how in sync animation+bullet firing are.
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            playShootAnim();
            StartCoroutine("shoot");            
        }
    }*/

    //This is the only function that's going to be in final version, maybe we could move shoot in here too. Or we could keep it where
    //it is and just get reference to the animation speed.
    //Everything else in here is scrapped, just used for testing.
    public void playShootAnim()
    {
        gunAnimations.SetTrigger(Animator.StringToHash(animStates[0]));
    }
    IEnumerator shoot()
    {
        //This delay puts animation and shooting in sync
        yield return new WaitForSeconds(gunAnimations.speed / 2);

        GameObject bullet = Instantiate(bulletPrefabs);
        bullet.transform.position = firePos.position;
        bullet.GetComponent<Bullet>().initialize();
    }
}
