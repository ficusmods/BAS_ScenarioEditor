using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerWatch : MonoBehaviour
{
    public delegate void TriggerEnter(Collider other);
    public event TriggerEnter onTriggerEnter;

    public delegate void TriggerStay(Collider other);
    public event TriggerStay onTriggerStay;

    public delegate void TriggerEnterExit(Collider other);
    public event TriggerEnterExit onTriggerExit;

    void Start()
    {
    }

    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        onTriggerEnter?.Invoke(other);
    }

    private void OnTriggerStay(Collider other)
    {
        onTriggerStay?.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        onTriggerExit?.Invoke(other);
    }
}
