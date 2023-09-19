using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouletteController : MonoBehaviour {
    private Rigidbody rb;
    private Vector3 normal;

    public float torque;
    
    // Start is called before the first frame update
    void Start() {
        rb = gameObject.GetComponent<Rigidbody>();
        normal = transform.up;
    }

    public void addTorque() {
        rb.AddTorque(normal * torque);
    }
}
