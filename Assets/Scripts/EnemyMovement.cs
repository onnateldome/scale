using System.Collections;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 15.0f;
    public float rotationSpeed = 15.0f;
    public Transform player;
    public Transform target;
    public bool followTarget = true;
    public float spiralRadius = 5.0f;
    public float spiralSpeed = 2.0f;
    public GameObject shotPrefab;
    public Transform gun;
    public float timeBetweenShots = 0.3f;
    private float timer = 0f;

    private Vector3 randomMoveTarget; // New variable to store the randomly selected move target

    void Start()
    {
        player = GameObject.Find("moveto").transform;
        target = GameObject.Find("target").transform;
        if (followTarget && player == null)
        {
            gameObject.SetActive(false);
        }
        else
        {
            // Select a random move target upon spawn
            SelectRandomMoveTarget();
        }
    }

    void Update()
    {
        if (followTarget)
        {
            if (player != null)
            {
                Vector3 direction = (player.position - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
            }
            else
            {
                followTarget = false;
            }
        }
        else
        {
            // Perform a spiral movement
            float angle = Time.time * spiralSpeed;
            Vector3 spiralPosition = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * spiralRadius;
            transform.position = spiralPosition;

            // Check if the enemy is close to the randomly selected move target
            if (Vector3.Distance(transform.position, randomMoveTarget) < 1.0f)
            {
                // Upon reaching the move target, select a new random move target
                SelectRandomMoveTarget();
            }
        }

        if (IsFacingPlayer() && timer >= timeBetweenShots)
        {
            float distance = Vector3.Distance(target.position, transform.position);
            if (distance < 150f)
            {
                FireShot();
                timer = 0f;
            }
        }

        timer += Time.deltaTime;
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    bool IsFacingPlayer()
    {
        if (player != null)
        {
            Vector3 directionToPlayer = player.position - transform.position;
            float angleToPlayer = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
            float angleDifference = Mathf.Abs(Mathf.DeltaAngle(transform.rotation.eulerAngles.z, angleToPlayer));

            float thresholdAngle = 90f;

            return angleDifference < thresholdAngle;
        }

        return false;
    }

    void FireShot()
    {
        GameObject go = Instantiate(shotPrefab, gun.position, gun.rotation) as GameObject;
        Destroy(go, 3f);
    }

    void SelectRandomMoveTarget()
    {
        // Generate a new random move target within a certain range
        randomMoveTarget = transform.position + new Vector3(Random.Range(-10f, 10f), 0f, Random.Range(-10f, 10f));
    }
}
