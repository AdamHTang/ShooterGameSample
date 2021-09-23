/*
 *  Created by: Adam Tang
 *  Date Created: Sept 13, 2021
 *  
 *  Last Edited by:
 *  Last Updated: Sept 22, 2021
 *  
 *  Description: Player control movements
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    /* VARIABLES */
    public bool mouseLook = true;           // Are we looking at the mouse.
    public string horzAxis = "Horizontal";
    public string vertAxis = "Vertical";
    public float maxSpeed = 5f;             // Speed of the character.
    private Rigidbody ThisBody = null;

    public string FireAxis = "Fire1";
    public float ReloadDelay = 0.3f;
    public bool CanFire = true;
    public Transform[] TurretTransforms;

    /* Awake - Call before game start*/
    private void Awake()
    {
        ThisBody = GetComponent<Rigidbody>();        // Sets the object's rigidbody to a variable called playerRB.
    }   // End Awake()

    private void FixedUpdate()
    {
        float horz = Input.GetAxis(horzAxis);
        float vert = Input.GetAxis(vertAxis);
        Vector3 moveDirection = new Vector3(horz, 0.0f, vert);

        ThisBody.AddForce(moveDirection.normalized * maxSpeed);            // normalized keeps the magnitude at 1 (unit vector).
        ThisBody.velocity = new Vector3(Mathf.Clamp(ThisBody.velocity.x, -maxSpeed, maxSpeed),
                                        Mathf.Clamp(ThisBody.velocity.y, -maxSpeed, maxSpeed),
                                        Mathf.Clamp(ThisBody.velocity.z, -maxSpeed, maxSpeed));

        if (mouseLook)
        {
            Vector3 mousePosWorld = Camera.main.ScreenToWorldPoint(
                                  new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.0f));

            mousePosWorld = new Vector3(mousePosWorld.x, 0.0f, mousePosWorld.z);

            Vector3 lookDirection = mousePosWorld - transform.position;
            transform.localRotation = Quaternion.LookRotation(lookDirection.normalized, Vector3.up);

        }

        if (Input.GetButtonDown(FireAxis) && CanFire)
        {
            foreach (Transform T in TurretTransforms)
            {
                AmmoManager.SpawnAmmo(T.position, T.rotation);
            }
            CanFire = false;
            Invoke("EnableFire", ReloadDelay);
        }
    }
    void EnableFire()
    {
        CanFire = true;
    }
}
