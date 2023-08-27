using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interpolations: MonoBehaviour
{
    /* 
     This script interpolates the transform properties between the initial state 
        and the state saved here based on a normalized variable (0-1)
     */
    
    [Tooltip("at 0 is the initial position, at 1 is the maxPosition")]
    public Vector3 maxPosition = Vector3.zero;
    
    [Tooltip("at 0 is the initial scale, at 1 is the maxScale")]
    public Vector3 maxScale = Vector3.one;

    [Tooltip("at 0 is the initial rotation, at 1 is the maxRotation")]
    public Vector3 maxRotation = Vector3.zero;

    private Vector3 initialPosition;
    private Vector3 initialScale;
    private Quaternion initialRotation;

    // Start is called before the first frame update
    void Start()
    {
        //at the beginning save the initial transforms
        initialPosition = transform.localPosition;
        initialScale = transform.localScale;
        initialRotation = transform.localRotation;
    }

    // Update is called once per frame
    public void ChangePosition(float amount)
    {
        //interpolates between the initial position and the maxPosition based on amount
        transform.localPosition = Vector3.Lerp(initialPosition, maxPosition, amount);
    }

    public void ChangeScale(float amount)
    {
        //interpolates between the initial scale and the maxScale based on amount
        transform.localScale = Vector3.Lerp(initialScale, maxScale, amount);
    }

    public void ChangeRotation(float amount)
    {
        //interpolates between the initial rotation and the maxRotation based on amount
        transform.localRotation = Quaternion.Lerp(initialRotation, Quaternion.Euler(maxRotation), amount);
    }
    
    //changes everything at the same amount
    public void Change(float amount)
    {
        ChangePosition(amount);
        ChangeRotation(amount);
        ChangeScale(amount);
    }
}
