
using UnityEngine;

public class SlotWheelController : MonoBehaviour {
    private float angleY;
    private float angleX;
    private float angleZ;
    private Quaternion targetRotation;

    public float rotateSpeed = 1;
    // Start is called before the first frame update
    void Start() {
        // Debug.Log(targety);
        targetRotation = transform.rotation;
        angleY = transform.rotation.eulerAngles.y;
        angleX = transform.rotation.eulerAngles.x;
        angleZ = transform.rotation.eulerAngles.z;
    }

    // Update is called once per frame
    void Update() {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);

    }

    public void rotate() {
        angleX -= 60;
        if (angleX > 360) angleX += 360;
        targetRotation = Quaternion.Euler(angleX, angleY, angleZ);
    }
}
