using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Transform playerModel;

    [Space]

    [Header("Parameters")]
    public float xySpeed = 18;
    public float lookSpeed = 340;
    public float forwardSpeed = 6;

    [Space]

    [Header("Public References")]
    public Transform aimTarget;
    public Transform cameraParent;

    [Space]

    [Header("Particles")]
    public ParticleSystem trail;
    public ParticleSystem circle;
    public ParticleSystem barrel;
    public ParticleSystem stars;


    public GameObject projectilePrefab;
    public float fireSpeed = 10.0f;

    void Start()
    {
        playerModel = transform;//.GetChild(0);
      //  SetSpeed(forwardSpeed);
    }

    void Update()
    {

        float h = Input.GetAxis("Horizontal") ;
        float v = Input.GetAxis("Vertical") ;

        LocalMove(h, v, xySpeed);
        RotationLook(h,v, lookSpeed);
        HorizontalLean(playerModel, h, 80, .1f);

       
     /*   if (Input.GetButtonDown("TriggerL") || Input.GetButtonDown("TriggerR"))
        {
            int dir = Input.GetButtonDown("TriggerL") ? -1 : 1;
          //  QuickSpin(dir);
        }
     */
        // Fire with the left mouse button
        if (Input.GetButtonDown("Fire1"))
        {
            Fire();
        }

    }
    void Fire()
    {
        // Check if the projectile prefab is assigned
        if (projectilePrefab != null)
        {
            // Instantiate the projectile at the player's position
            GameObject newProjectile = Instantiate(projectilePrefab, transform.position, transform.rotation);

            // Get the rigidbody of the projectile (if it has one)
            Rigidbody projectileRigidbody = newProjectile.GetComponent<Rigidbody>();

            // Check if the projectile has a rigidbody
            if (projectileRigidbody != null)
            {
                // Set the initial velocity of the projectile
                Vector3 fireDirection = aimTarget.position - transform.position;
                projectileRigidbody.velocity = fireDirection.normalized * fireSpeed;
            }
        }
    }
    void LocalMove(float x, float y, float speed)
    {
        transform.localPosition += new Vector3(x, y, 0) * speed * Time.deltaTime;
        ClampPosition();
    }

    void ClampPosition()
    {
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);
        transform.position = Camera.main.ViewportToWorldPoint(pos);
    }

    void RotationLook(float h, float v, float speed)
    {
        aimTarget.parent.position = Vector3.zero;
        aimTarget.localPosition = new Vector3(h, v, 1);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(aimTarget.position), Mathf.Deg2Rad * speed * Time.deltaTime);
    }

    void HorizontalLean(Transform target, float axis, float leanLimit, float lerpTime)
    {
        Vector3 targetEulerAngels = target.localEulerAngles;
        target.localEulerAngles = new Vector3(targetEulerAngels.x, targetEulerAngels.y, Mathf.LerpAngle(targetEulerAngels.z, -axis * leanLimit, lerpTime));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(aimTarget.position, .5f);
        Gizmos.DrawSphere(aimTarget.position, .15f);

    }

        
}
