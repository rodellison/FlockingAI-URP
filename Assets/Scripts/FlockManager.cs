using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
    public GameObject[] fishPrefab;

    public int numFish = 20;

    public GameObject[] allFish;
    private GameObject FishParent;
    public Vector3 goalPos;

    public Vector3 swimLimits = new Vector3(5, 5, 5);
    // Start is called before the first frame update

    [Header("Fish Settings")] [Range(0.5f, 2.5f)]
    public float minSpeed;

    [Range(2.5f, 5f)] public float maxSpeed;

    public float neighborDistance = 10;
    [Range(2.0f, 5.0f)] public float rotationSpeed = 3f;
    [Tooltip("What percentage of the time should apply flock rules occur. Note 100% creates a very mechanical, exact look.")]
    [Range(0.0f, 100.0f)] public float applyFlockRulePcnt = 10f;


    void Start()
    {
        //Establish a Fish Parent empty game object, to allow the Fish to be created under a parent..
        if (GameObject.Find("FishParent") == null)
        {
            FishParent = new GameObject("FishParent");
            FishParent.transform.position = Vector3.zero;
            StartCoroutine(ChangeFlockNeighborhood());
        }

//Create the fish, assign each under the parent, and also provide each's Flock script with a reference to this manager
//and the percentage value for which to apply the FLock Rules..
        allFish = new GameObject[numFish];
        for (int i = 0; i < numFish; i++)
        {
            Vector3 pos = transform.position +
                          new Vector3(Random.Range(-swimLimits.x, swimLimits.x),
                              Random.Range(-swimLimits.y, swimLimits.y),
                              Random.Range(-swimLimits.z, swimLimits.z));
            allFish[i] = (GameObject) Instantiate(fishPrefab[Random.Range(0, fishPrefab.Length)], pos,
                Quaternion.identity, FishParent.transform);
            allFish[i].GetComponent<Flock>().SetFlockManager(this, applyFlockRulePcnt);
        }

        goalPos = transform.position;
    }

    /// <summary>
    /// Called every frame, apply some randomness to having the fish's goal change, as well as provide some
    /// randomness to their rotation speed. The latter creates some erratic behaviour elements to the fish
    /// </summary>
    void Update()
    {
        if (Random.Range(0, 100) < 5)
            goalPos = transform.position + new Vector3(Random.Range(-swimLimits.x, swimLimits.x),
                          Random.Range(-swimLimits.y, swimLimits.y),
                          Random.Range(-swimLimits.z, swimLimits.z));

        if (Random.Range(0, 100) < 30)
            rotationSpeed = Random.Range(2.0f, 5.0f);
    }
    
    /// <summary>
    /// Coroutine that will provide a way for the neighborhood size to expand and contract, allowing smaller flocks
    /// to develop, but then every few seconds bring the flocks together. This is somewhat of a way to manager control
    /// </summary>
    /// <returns></returns>

    IEnumerator ChangeFlockNeighborhood()
    {
        while (true)
        {
            neighborDistance = 10f;
            yield return new WaitForSecondsRealtime(4f);
            neighborDistance = Random.Range(0f, 5f);
            yield return new WaitForSecondsRealtime(4f);
        }
    }
}