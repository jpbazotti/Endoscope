using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ContactZone : MonoBehaviour
{
    private List<Transform> segments = new List<Transform>();
    private Vector3 direction=Vector3.zero;
    private Vector3 previousPosition=Vector3.zero;
    private Quaternion previousRotation;
    private Quaternion rotation;

    private void Update()
    {
        if (segments.Count>0)
        {
            rotation = segments.Last().rotation * Quaternion.Inverse(previousRotation);
            direction = segments.Last().position - previousPosition;
            //Debug.Log("Name:"+ segments.Last().gameObject.name +" Current Pos:" + segments.Last().position+" Last Pos:"+ previousPosition +  " Direction:" + direction);
            //Debug.Log(direction.x + " " + direction.y + " " +direction.z);
            previousPosition = segments.Last().position;
            previousRotation = segments.Last().rotation;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.position.x > transform.position.x)
        {
            if (other.gameObject.CompareTag("Rope"))
            {
                if (segments.Count > 0)
                {
                    segments.Last().gameObject.GetComponent<VisibilityManager>().visibility = false;
                }
                segments.Add(other.transform);
                previousPosition = segments.Last().position;
                previousRotation = segments.Last().rotation;
                other.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;

            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Rope"))
        {
            other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.position.x > transform.position.x)
        {
            if (other.gameObject.CompareTag("Rope"))
            {
                segments.Remove(other.transform);
                if (segments.Count > 0)
                {
                    previousPosition = segments.Last().position;
                    previousRotation = segments.Last().rotation;
                    //segments.Last().gameObject.GetComponent<VisibilityManager>().visibility = true;
                }
                other.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            }
        }
    }
}
