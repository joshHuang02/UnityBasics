
using UnityEngine;

public class SlotWheelController : MonoBehaviour {
    private float targety;

    public float rotateSpeed = 1;
    // Start is called before the first frame update
    void Start() {
        targety = gameObject.transform.localRotation.eulerAngles.y;
        Debug.Log(targety);
    }

    // Update is called once per frame
    void Update() {
        if (targety > 360) targety -= 360;
        Debug.Log(targety - gameObject.transform.localRotation.eulerAngles.y);
        if (Mathf.Abs(targety - gameObject.transform.localRotation.eulerAngles.y) > 10) {
            transform.Rotate(0, rotateSpeed * Time.deltaTime * (1), 0);
            
        }
    }

    public void rotate() {
        targety += 60;
    }
}
