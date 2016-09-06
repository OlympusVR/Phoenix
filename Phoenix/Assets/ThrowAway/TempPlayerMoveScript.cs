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


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.F))
        {
            if (gunRef.isAutomatic)
                gunRef.autoGunMechanics();
            else
                gunRef.gunMechanics();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            gunRef.engageGun();
        }
        //Not going to bother changing input names.
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        if (x != 0)
            transform.Translate(transform.right * x * Time.deltaTime * moveSpeed);
        if (z != 0)
            transform.Translate(transform.forward * z * Time.deltaTime * moveSpeed);
        if (gun != null)
        {
            float moveGunX = Input.GetAxis("Mouse X");
            float moveGunY = Input.GetAxis("Mouse Y");

            if (moveGunX != 0)
            {
                holder.Translate(holder.right * moveGunX * Time.deltaTime * moveSpeed);

                //transform.Rotate(new Vector3(1 * rotateX * gunSpeed * Time.deltaTime, 0, 0));
            }
            if (moveGunY != 0)
            {
                holder.Translate(holder.up * moveGunY * Time.deltaTime * moveSpeed);
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
