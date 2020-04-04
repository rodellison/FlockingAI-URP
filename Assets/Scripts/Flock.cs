using System;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class Flock : MonoBehaviour
{
    public FlockManager myManager;
    private Animator anim;
    public float speed;
    private bool turning = false;

    private void Start()
    {
        speed = Random.Range(myManager.minSpeed, myManager.maxSpeed);
        anim = GetComponent<Animator>();
        anim.SetFloat("swimOffset", Random.Range(0.0f, 1.0f));
    }

    private void Update()
    {
        //Code that tests if an object is within a bounds
        //if it is NOT, then set a flag, and use that to then steer the object back Into acceptable bounds
        Bounds b = new Bounds(myManager.transform.position, myManager.swimLimits * 2);
        RaycastHit hit = new RaycastHit();
        Vector3 direction = Vector3.zero;
        turning = false;
        
        //First test if the Fish has left the bounding box area, if so, we need to turn it back towards the flockManager
        //position
        if (!b.Contains(transform.position))
        {
            turning = true;
            direction = myManager.transform.position - transform.position;
        }
        else if (Physics.Raycast(transform.position, transform.forward * 2, out hit))
        {
            //The fish is in bounds, so make sure its not going to hit an obstacle (something with a collider)
            //Note this is not a test for collision with other fish. 
            turning = true;
            direction = Vector3.Reflect(transform.forward, hit.normal);
        }
        else
        {
            turning = false;
        }

        //If the tests above for whether we need to turn the fish are true, then use this update to turn.
        //If we're not turning due to leaving boundary, OR colliding with an object, THEN
        //perform normal Flocking logic.
        if (turning)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(direction),
                myManager.rotationSpeed * Time.deltaTime);
        }
        else
        {
            if (Random.Range(0, 100) < 10)
            {
                speed = Random.Range(myManager.minSpeed, myManager.maxSpeed);
                var speedMult = ((speed - myManager.minSpeed) / (myManager.maxSpeed - myManager.minSpeed))
                                * (3.0f - 1f) + 1f;

                anim.SetFloat("swimSpeedMult", speedMult);
            }

            if (Random.Range(0, 100) < 20)
                ApplyRules();
        }

        //The fish always needs to be moving, and since the Z direction represents
        //Transform.Forward = 1, we can just use:
        transform.Translate(0, 0, Time.deltaTime * speed);
    }

    void ApplyRules()
    {
        //Flocking Rules:
        //1. Move towards the Average position of the group
        //2. Align with the average heading of the group
        //3. Avoid crowding other group members

        Vector3 vCenter = Vector3.zero; //this will hold the average center for the group
        Vector3 vAvoid = Vector3.zero;
        float gSpeed = 0.01f; // this will hold the average for the group
        float nDistance;
        int groupSize = 0;

        foreach (GameObject go in myManager.allFish)
        {
            if (go != this.gameObject)
            {
                nDistance = Vector3.Distance(go.transform.position, transform.position);
                if (nDistance <= myManager.neighborDistance)
                {
                    vCenter += go.transform.position;
                    groupSize++;

                    if (nDistance < 1.0f)
                    {
                        vAvoid += (transform.position - go.transform.position);
                    }

                    Flock anotherFlock = go.GetComponent<Flock>();
                    gSpeed += anotherFlock.speed;
                }
            }
        }

        if (groupSize > 0)
        {
            vCenter = vCenter / groupSize + (myManager.goalPos - transform.position);
            speed = gSpeed / groupSize;

            Vector3 direction = (vCenter + vAvoid) - transform.position;
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation,
                    Quaternion.LookRotation(direction),
                    myManager.rotationSpeed * Time.deltaTime);
            }
        }
    }
}