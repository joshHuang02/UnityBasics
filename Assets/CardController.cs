
using System.Collections;
using UnityEngine;

public class CardController : MonoBehaviour {
    public GameObject camera;
    public float cardSpacing;
    public float cardScale;
    public int rows;
    public int cols;
    public float flipDuration;
    
    private GameObject[,] cards;
    private Vector3 startingPoint = new Vector3(0, 0, 0);

    // Start is called before the first frame update
    void Start() {
        cards = new GameObject[30, 30];
        SpawnCards();
    }

    private void SpawnCards() {
        Object[] objects = Resources.LoadAll("Cards", typeof(GameObject));
        for (int i = 0; i < cols; i++) {
            for (int j = 0; j < rows; j++) {
                GameObject card = (GameObject)objects[Random.Range(0, objects.Length)];
                var newCard = Instantiate(card, transform);
                newCard.transform.localPosition += new Vector3(i * cardSpacing, j * -cardSpacing, 0);
                newCard.transform.localScale *= cardScale;
                newCard.transform.LookAt(camera.transform);
                newCard.transform.Rotate(90, 0, 0);
                cards[i, j] = newCard;
            }
        }
    }

    public IEnumerator FlipCard()
    {
        GameObject card = cards[Random.Range(0, cols), Random.Range(0, rows)];
        float startRotation = card.transform.eulerAngles.x;
        float endRotation = startRotation + 180.0f;
        float t = 0.0f;

        while ( t  < flipDuration )
        {
            t += Time.deltaTime;

            float rotation = Mathf.Lerp(startRotation, endRotation, t / flipDuration) % 360.0f;

            card.transform.eulerAngles = new Vector3(rotation, card.transform.eulerAngles.y, card.transform.eulerAngles.z);

            yield return null;
        }
    }
}
