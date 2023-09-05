using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using Random = System.Random;

public class FoodController : MonoBehaviour {

    public float speed;

    private Renderer renderer;
    private Material material;
    private Rigidbody rigidbody;
    
    private int partNum;

    private bool changingColor;

    private bool changingOpacity;
    private int opacityDirection;

    void Start() {
        renderer = gameObject.GetComponent<Renderer>();
        material = renderer.material;
        rigidbody = gameObject.GetComponent<Rigidbody>();
        
        String num = Regex.Match(gameObject.name, @"\d+").Value;
        if (num.Length != 0) {
            partNum = Int32.Parse(num);
            transform.position += Vector3.forward * partNum * 10;
        }
        else {
            partNum = 0;
        }
    }

    void Update() {
        if (Input.GetKey(KeyCode.J)) {
            transform.Rotate(100 * Time.deltaTime * speed * (1+partNum*0.1f) * Vector3.left);
            transform.localScale += new Vector3(speed, speed, speed) * 0.001f;
        }
        if (Input.GetKey(KeyCode.K)) {
            transform.Rotate(-100 * Time.deltaTime * speed * (1+partNum*0.1f) * Vector3.left);
            transform.localScale -= new Vector3(speed, speed, speed) * 0.001f;

        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += 0.003f * (partNum*partNum) * Vector3.up;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += 0.003f * (partNum*partNum) * Vector3.down;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += 0.003f * (partNum*partNum) * Vector3.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += 0.003f * (partNum*partNum) * Vector3.right;
        }
        if (Input.GetKey(KeyCode.Q)) {
            transform.RotateAround(Vector3.zero, Vector3.forward, 20 * speed * Time.deltaTime);
            transform.RotateAround(transform.position, Vector3.forward, -20 * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.E)) {
            transform.RotateAround(Vector3.zero, Vector3.forward, -20 * speed * Time.deltaTime);
            transform.RotateAround(transform.position, Vector3.forward, 20 * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.C)) {
            if (!changingColor) {StartCoroutine(RandomColor());}
        }
        if (Input.GetKey(KeyCode.O)) {
            if (!changingOpacity) {StartCoroutine(CycleOpacity());}
        }
        if (Input.GetKeyDown(KeyCode.M)) {
            UnityEngine.Object[] materials = Resources.LoadAll("Materials", typeof(Material));
            // material = materials[UnityEngine.Random.Range(0, materials.Length)];
            gameObject.GetComponent<Renderer>().material = (Material)materials[UnityEngine.Random.Range(0, materials.Length)];
        }
        if (Input.GetKeyDown(KeyCode.L)) {
            if (renderer.shadowCastingMode == ShadowCastingMode.Off) {
                renderer.shadowCastingMode = ShadowCastingMode.On;
            }
            else {
                renderer.shadowCastingMode = ShadowCastingMode.Off;
            }
        }

        if (Input.GetKeyDown(KeyCode.P)) {
            rigidbody.isKinematic = false;
        }
    }

    private IEnumerator RandomColor() {
        changingColor = true;
        material.color = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        yield return new WaitForSeconds(0.05f);
        changingColor = false;
        yield return null;
    }

    private IEnumerator CycleOpacity() {
        changingOpacity = true;
        var opacity = material.color.a;
        if (opacity >= 0.95f) {
            opacityDirection = -1;
        }
        else if (opacity <= 0.05) {
            opacityDirection = 1;
        }

        opacity += opacityDirection * 0.05f;
        Color oldColor = material.color;
        Color newColor = new Color(oldColor.r, oldColor.g, oldColor.b, opacity);
        material.SetColor("_Color", newColor);
        yield return new WaitForSeconds(0.05f);
        changingOpacity = false;
        yield return null;
    }
}