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

    [Header("Fish Settings")]
    [Range(0.5f, 2.5f)] public float minSpeed;
    [Range(2.5f, 5f)] public float maxSpeed;
    
    public float neighborDistance = 10;
    [Range(2.0f, 5.0f)] public float rotationSpeed;

    void Start()
    {
        
        if (GameObject.Find("FishParent") == null)
        {
            FishParent = new GameObject("FishParent");
            FishParent.transform.position = Vector3.zero;
            StartCoroutine(ChangeFlockNeighborhood());
        }


        allFish = new GameObject[numFish];
        for (int i = 0; i < numFish; i++)
        {
            Vector3 pos = transform.position +
                          new Vector3(Random.Range(-swimLimits.x, swimLimits.x),
                              Random.Range(-swimLimits.y, swimLimits.y),
                              Random.Range(-swimLimits.z, swimLimits.z));
            allFish[i] = (GameObject) Instantiate(fishPrefab[Random.Range(0,fishPrefab.Length)], pos,
                Quaternion.identity, FishParent.transform);
            allFish[i].GetComponent<Flock>().myManager = this;
        }

        goalPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Random.Range(0, 100) < 5)
            goalPos = transform.position + new Vector3(Random.Range(-swimLimits.x, swimLimits.x),
                                                     Random.Range(-swimLimits.y, swimLimits.y),
                                                     Random.Range(-swimLimits.z, swimLimits.z));

        if (Random.Range(0, 100) < 30)
            rotationSpeed = Random.Range(2.0f, 5.0f);
        
    }

    IEnumerator ChangeFlockNeighborhood()
    {
        while (true)
        {
            neighborDistance = 10f;
            yield return new WaitForSecondsRealtime(4f);
            neighborDistance = Random.Range(1, 5f);
            yield return new WaitForSecondsRealtime(4f);
        }
    }
}