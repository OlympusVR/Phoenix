using UnityEngine;
using System.Collections;
using Phoenix;
public class TempPlayerMoveScript : MonoBehaviour {

    TempGunForTesting gunRef;
    public float moveSpeed;
    //Going to have mouse movement move gun around, so can't move camera but can move gun, then wasd will be movement.
    public float gunSpeed;
    Transform holder;
    GameObject gun;
    // Use this for initialization
    void Awake()
    {
        holder = transform.GetChild(0);

    }


    void Start()
    {

        gunSpeed = 50.0f;
        moveSpeed = 2;
    }

    // Update is called once per frame
    void Update()
    {  //Not going to bother changing input names.
            float x = Input.GetAxis("MoveX");
            float z = Input.GetAxis("MoveZ");
            if (x != 0)
                transform.Translate(transform.right * x * Time.deltaTime * moveSpeed);
            if (z != 0)
                transform.Translate(transform.forward * z * Time.deltaTime * moveSpeed);
           
        if (gun != null)
        {
            if (Input.GetAxis("Shoot") > 0)
            {
                if (gunRef.isAutomatic)
                    gunRef.autoGunMechanics();
                else
                    gunRef.gunMechanics();
            }
            if (Input.GetAxis("Reload") > 0)
            {
                gunRef.engageGun();
            }

            float moveGunX = Input.GetAxis("Mouse X");
            float moveGunY = Input.GetAxis("Mouse Y");

            if (moveGunX != 0)
            {
                holder.Translate(holder.right * moveGunX * Time.deltaTime * moveSpeed);

                //    transform.Rotate(new Vector3(0, 1 * gunSpeed * Time.deltaTime,0));
            }
            if (moveGunY != 0)
            {
                holder.Translate(holder.up * moveGunY * Time.deltaTime * moveSpeed);
                // transform.Rotate(new Vector3(1 * gunSpeed * Time.deltaTime, 0, 0));
            }
            //x rotates up and down, y rotates left and right.
            float rotateGunX = Input.GetAxis("RotateGunX");
            float rotateGunY = Input.GetAxis("RotateGunY");

            if (rotateGunX != 0)
            {
                holder.Rotate(new Vector3(0, 1 * rotateGunX * gunSpeed * Time.deltaTime, 0));
            }

            if (rotateGunY != 0)
            {
                holder.Rotate(new Vector3(1 * rotateGunY * gunSpeed * Time.deltaTime, 0, 0));
            }

        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Revolver")
        {
            other.transform.parent = holder;
            gun = other.gameObject;
            gunRef = gun.GetComponent<TempGunForTesting>();

            other.transform.localPosition = Vector3.zero;
            other.transform.localRotation = Quaternion.identity;
        }

    }
}
