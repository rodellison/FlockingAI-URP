using UnityEngine;
using System.Collections;
 
public class LookAtFlock : MonoBehaviour
{
    public Transform lookAt;
    public float smooth = 5F;
    public float lookAhead = 0F;
 
    Quaternion lastRotation;
    Quaternion goalRotation;
 
    void FixedUpdate ()
    {
      //  Debug.DrawLine(lookAt.position, transform.position, Color.gray);
 
        Vector3 difference = lookAt.TransformPoint(new Vector3(lookAhead, 0F, 0F)) - transform.position;
        Vector3 upVector = lookAt.position - lookAt.TransformPoint(Vector3.down);
        goalRotation = Quaternion.LookRotation(difference, upVector);
    }
 
    void Awake ()
    {
        lastRotation = transform.rotation;
        goalRotation = lastRotation;
    }  
   
    void LateUpdate ()
    {
        lastRotation = Quaternion.Slerp (lastRotation, goalRotation, smooth * Time.deltaTime);
        transform.rotation = lastRotation;
    }
 
}