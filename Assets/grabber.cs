using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class grabber : MonoBehaviour
{
    public bool direito;

    List<InputDevice> devices = new List<InputDevice>();

    private GameObject obj;
    Quaternion previousRotation;

    private Transform wheel1;
    private Transform wheel2;
    private bool firstWheel = false;
    private bool wasClicking = false;
    private float previousAxis = 0;
    private bool increasing;
    private bool deacreasing;

    private bool holding;
    private bool sliding;

    public GameObject movement;
    private List<Transform> segments = new List<Transform>();

    private List<Transform> pontaSegments = new List<Transform>();
    private List<Quaternion> rotationBuffer = new List<Quaternion>();
    private List<Transform> pontaRotationPoints= new List<Transform>();
    // Start is called before the first frame update
    void Start()
    {
        GameObject ponta = GameObject.Find("Ponta");
        pontaSegments.Add(ponta.transform.GetChild(0));
        pontaRotationPoints.Add(ponta.transform.GetChild(1));
        int current = 0;
        while (pontaSegments[current].transform.childCount > 2)
        {
            pontaSegments.Add(pontaSegments[current].transform.GetChild(0));
            pontaRotationPoints.Add(pontaSegments[current].transform.GetChild(1));
            current++;
        }

        if (direito)
        {
            InputDevices.GetDevicesAtXRNode(XRNode.RightHand, devices);
        }
        else
        {
            InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, devices);
        }
        obj = null;
        holding = false;
        sliding = false;
        previousRotation = Quaternion.identity;
        foreach(Transform child in GameObject.Find("Rope").transform)
        {
            segments.Add(child);
        }

    }

    private void FixedUpdate()
    {
        if (holding && obj != null)
        {
            obj.GetComponent<Rigidbody>().freezeRotation = true;
            obj.GetComponent<Rigidbody>().velocity = (transform.position - obj.transform.position)*20;
            //obj.GetComponent<Rigidbody>().MovePosition(transform.position);
            Quaternion difference = transform.rotation * Quaternion.Inverse(previousRotation);
            Quaternion finalRotation = difference * obj.transform.rotation;
            obj.GetComponent<Rigidbody>().MoveRotation(finalRotation);

            previousRotation = transform.rotation;

            if (obj.CompareTag("Manopla"))
            {
                if (!direito)
                {
                    movement.SetActive(false);
                }
                Vector2 axis = Vector2.zero;
                bool click = false;
                foreach (InputDevice device in devices)
                {
                    device.TryGetFeatureValue(CommonUsages.primary2DAxis, out axis);
                }
                foreach (InputDevice device in devices)
                {
                    device.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out click);
                }
                if (!wasClicking && click)
                {
                    firstWheel = !firstWheel;
                }
                wasClicking = click;
                

                float axisChange = axis.y - previousAxis;
                if (previousAxis == 0)
                {
                    increasing = false;
                    deacreasing = false;
                    if (axisChange > 0)
                    {
                        increasing = true;
                    }
                    else if(axisChange < 0)
                    {
                        deacreasing = true;
                    }
                }
                previousAxis = axis.y;

                if (firstWheel)
                {
                    if ((increasing&&axisChange>0)|| (deacreasing && axisChange<0))
                    {
                        Vector3 newRotation = wheel1.rotation.eulerAngles;
                        newRotation.z -= axisChange*10;
                        wheel1.rotation = Quaternion.Euler(newRotation);
                        for(int i =0;i<pontaSegments.Count;i++)
                        {
                            pontaSegments[i].RotateAround(pontaRotationPoints[i].position, pontaSegments[i].right, axisChange*5);
                        }
                        // pontaSegment1.RotateAround(pontaRotationPoint1.position, pontaRotationPoint1.right, axisChange*10);
                    }

                }
                else
                {
                    if ((increasing && axisChange > 0) || (deacreasing && axisChange < 0))
                    {
                        Vector3 newRotation = wheel2.rotation.eulerAngles;
                        newRotation.z -= axisChange*10;
                        wheel2.rotation = Quaternion.Euler(newRotation);
                        for (int i = 0; i < pontaSegments.Count; i++)
                        {
                            pontaSegments[i].RotateAround(pontaRotationPoints[i].position, pontaSegments[i].forward, axisChange*5);

                        }
                        // pontaSegment2.RotateAround(pontaRotationPoint2.position, pontaRotationPoint2.forward, axisChange*10);
                    }
                }
            }
        }
        else if (sliding && obj!=null && obj.CompareTag("Rope"))
        {
            Transform closest = segments[0];
            foreach(Transform segment in segments)
            {
                if (Vector3.Distance(transform.position, segment.position) < Vector3.Distance(transform.position, closest.position))
                {
                    closest = segment;
                }
            }



            obj.GetComponent<Rigidbody>().freezeRotation = false;
            obj = closest.gameObject;
           // obj.GetComponent<Rigidbody>().freezeRotation = true;
            obj.GetComponent<Rigidbody>().velocity = Vector3.Project(((transform.position - obj.transform.position) * 20),Vector3.up);
            //obj.GetComponent<Rigidbody>().MovePosition(transform.position);
            //Quaternion difference = transform.rotation * Quaternion.Inverse(previousRotation);
            //Quaternion finalRotation = difference * obj.transform.rotation;
            //obj.GetComponent<Rigidbody>().MoveRotation(finalRotation);

            previousRotation = transform.rotation;

        }
        else
        {
            if (!direito)
            {
                movement.SetActive(true);
            }
        }
    }
    private void Update()
    {
        if (obj != null)
        {
            Debug.Log("objeto na hitbox:" + obj.name);
        }
        float gripping = 0.0f;
        foreach (InputDevice device in devices)
        {
            device.TryGetFeatureValue(CommonUsages.grip, out gripping);
        }
       
        if (holding || sliding)
        {
           
            if (gripping==0 && obj!=null)
            {
                obj.GetComponent<Rigidbody>().freezeRotation = false;
                obj = null;
                holding = false;
                sliding = false;
            }
            if (obj != null && gripping >= 0.9f)
            {

                holding = true;
                sliding = false;
            }
            else if (obj != null && gripping < 0.9f)
            {
                holding = false;
                sliding = true;
            }
        }
        else
        {
            if (obj != null && gripping>=0.9f)
            {

                holding = true;
                sliding = false;
            }
            else if(obj != null && gripping < 0.9f && gripping!=0)
            {
                holding = false;
                sliding = true;
            }
        }
        //Debug.Log("holding: " + holding + " sliding " + sliding);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!holding && !sliding && other.CompareTag("Rope"))
        {
            obj = other.gameObject;
        }
        if (!holding && !sliding && other.CompareTag("Manopla"))
        {
            obj = other.gameObject;
            wheel1 = obj.transform.Find("Wheel1");
            wheel2 = obj.transform.Find("Wheel2");
        }

    }

    private void OnTriggerExit(Collider other)
    {
       
        if (!holding && !sliding && other.CompareTag("Rope"))
        {
            obj =null;
        }
        if (!holding && !sliding && other.CompareTag("Manopla"))
        {
            obj = null;
        }
    }


}
