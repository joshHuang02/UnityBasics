using UnityEngine;

public class CardSpitter : MonoBehaviour
{
    public float cardScale;
    public float spitStrength;
    public float rotateSpeed;

    public void SpitCard() {
        Object[] objects = Resources.LoadAll("Cards", typeof(GameObject));
        GameObject card = (GameObject)objects[Random.Range(0, objects.Length)];
        var newCard = Instantiate(card, transform);
        newCard.transform.localScale *= cardScale;
        Rigidbody rb = newCard.AddComponent<Rigidbody>();
        rb.useGravity = true;
        Vector3 spitDir = new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.01f, 0.3f), -1);
        rb.AddForce(spitDir * spitStrength);
        int direction = Random.Range(0, 2);
        Vector3 spinDir;
        if (direction == 0) {
            spinDir = new Vector3(Random.Range(0.05f, 0.05f), 1, 0);
        }
        else {
            spinDir = new Vector3(Random.Range(0.05f, 0.05f), -1, 0);
        }
        rb.AddTorque(spinDir * rotateSpeed);
    }
}
