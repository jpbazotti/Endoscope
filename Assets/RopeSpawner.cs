using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeSpawner : MonoBehaviour
{

    public GameObject ropePart;
    public GameObject ropeParent;

    public float lenght;
    public float partOffset =0.21f;

    public bool reset, spawn;

    void Update()
    {
        if (reset)
        {
            foreach(GameObject obj in GameObject.FindGameObjectsWithTag("Rope"))
            {
                Destroy(obj);
            }
            reset = false;
        }
        if (spawn)
        {
            Spawn();
            spawn = false;
        }
    }

    public void Spawn()
    {
        int count = (int)(lenght / partOffset);

        for(int i = 0; i < count; i++)
        {
            Vector3 spawnPos=transform.position;
            Debug.Log(spawnPos);
            spawnPos.y = partOffset*(i+1);

            GameObject segment= Instantiate(ropePart, spawnPos, Quaternion.identity, ropeParent.transform);
            segment.transform.eulerAngles=new Vector3 (180, 0, 0);
            segment.name=ropeParent.transform.childCount.ToString();
            segment.tag = "Rope";
            if (i == 0)
            {
                Destroy(segment.GetComponent<ConfigurableJoint>());

            }
            else
            {
                segment.GetComponent<ConfigurableJoint>().connectedBody = ropeParent.transform.Find((ropeParent.transform.childCount-1).ToString()).GetComponent<Rigidbody>();
            }
        }

        
    }
}
