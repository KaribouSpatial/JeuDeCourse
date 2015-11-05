using System;
using UnityEngine;
using System.Collections;

public class AccelerationBonusScript : MonoBehaviour
{

    [SerializeField]
    int accelerationForce = 50;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerStay(Collider collider)
    {
        if (collider.attachedRigidbody)
        {
            collider.attachedRigidbody.AddForce(transform.forward * accelerationForce, ForceMode.Acceleration);
        }
    }
}