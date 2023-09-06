using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//include this
using ElRaccoone.Tweens;

/* 
if the Tweens library is not recognized 
Windows -> package manager
click the + (plus) on the top left
add package from git url
copy paste this:
git + https://github.com/jeffreylanters/unity-tweens
 */

public class Tweens : MonoBehaviour
{
    public GameObject objectA;
    public GameObject objectB;
    public GameObject objectC;
    public GameObject objectD;
    public GameObject objectE;
    public GameObject objectF;
    public GameObject objectG;
    public GameObject objectH;
    public GameObject objectI;

    private Color initialColor;
    public Color colorTarget;

    private Coroutine coroutineSequence;

    // Start is called before the first frame update
    void Start()
    {
        //save my initial color so I can go back to it
        initialColor = objectF.GetComponent<Renderer>().material.color;
    }

    // Update is called once per frame
    void Update()
    {
        //tweening cheatsheet
        //https://easings.net/

        //library documentation
        //https://github.com/jeffreylanters/unity-tweens

        //changing position
        if (Input.GetKeyDown(KeyCode.Z))
        {
            //move to coordinates in 3 seconds
            objectA.TweenPosition(new Vector3(0, 3, 0), 3);

            //same upward movement but using a tweening 
            objectB.TweenPosition(new Vector3(objectB.transform.position.x, 3, 0), 3).SetEaseSineOut();

            //same movement, different way to handle the variables
            //SetFrom sets the initial value, try to press repeteadly
            Vector3 startingPosition = new Vector3(2, 1, 0);
            objectC.TweenPosition(new Vector3(startingPosition.x, 3, startingPosition.z), 3).SetFrom(startingPosition).SetEaseQuadIn();

            //Try to change the easing functions and see what happens

        }

        //changing position with 
        if (Input.GetKeyDown(KeyCode.X))
        {
            //since object A can also be moved by the function above 
            //it's a good idea to cancel all the existing tweens to avoid glitches
            objectA.TweenCancelAll();

            Vector3 randomPosition = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-5f, 5f));
            objectA.TweenPosition(randomPosition, 1).SetEaseExpoInOut();

        }


        if(Input.GetKeyDown(KeyCode.C))
        {
            Vector3 targetRotation = new Vector3(0, 0, 0);

            //change rotation with delay of 2 seconds
            objectD.TweenRotation(targetRotation, 5).SetDelay(2).SetEaseCubicOut();

            Vector3 targetScale = new Vector3(1, 1, 1);
            Vector3 initialScale = new Vector3(0.5f, 0.5f, 0.5f);

            objectE.TweenCancelAll();
            
            //change scale with ping pong and loop it
            objectE.TweenLocalScale(targetScale, 0.5f).SetFrom(initialScale).SetPingPong().SetLoopCount(4).SetEaseCubicOut();

            //two tweens at the same time 
            objectE.TweenRotationY(Random.Range(0f,360f), 1).SetEaseSineInOut();

        }


        //Bounce and Back need an overshooting value to work properly
        if (Input.GetKeyDown(KeyCode.N))
        {
            objectH.TweenLocalScale(new Vector3(2, 2, 2), 1).SetFrom(Vector3.one).SetEaseBounceOut().SetOvershooting(0.5f);
        }


        //there are many other methods here
        //https://github.com/jeffreylanters/unity-tweens#tweening-methods

        //more advanced uses

        if (Input.GetKeyDown(KeyCode.V))
        {
            //tween the color and call a function at the end
            objectF.TweenMaterialColor(colorTarget, 5).SetEaseCircInOut().SetOnComplete(Disappear);
        }


        //coroutines are usually a better way to have a sequence of tweens
        if (Input.GetKeyDown(KeyCode.B))
        {
            //this prevent the coroutine from being called until the end
            if (coroutineSequence == null)
            {
                coroutineSequence = StartCoroutine(CodedSequence(objectG));
            }
        }


        //if the value you want to change isn't covered you can use generic values tweens
        if (Input.GetKeyDown(KeyCode.M)) {

            //change a value from 0 to 1 in 5 seconds,
            //the value is passed to my function ChangeEffect at every update
            objectI.TweenValueFloat(1, 5, value => { print("my value is "+value); ChangeEffect(objectI, value); }).SetFrom(0).SetEaseSineInOut();
        }
    }



    public void Disappear()
    {
        print("Disappear function called");

        //another way to call a function without declaring it
        objectF.TweenLocalScale(Vector3.zero, 2f).SetOnComplete( ()=> Destroy(objectF) );
    }

    //this tween library can be used inside coroutines
    //but you have to add yield return and the function .Yield() at the end
    //it will wait to move to the next line
    private IEnumerator CodedSequence(GameObject obj)
    {
        //save the initial color
        Color myColor = objectF.GetComponent<Renderer>().material.color;

        //this tween stop the execution (i want the color to change while the other tweens happen)
        obj.TweenMaterialColor(colorTarget, 5);

        //this tween will stop the coroutine until it's done

        //SetEaseBackOut works with overshooting giving it a bit of a bounce
        yield return obj.TweenLocalScale(new Vector3(2,2,2), 1).SetEaseBackOut().SetOvershooting(0.25f).Yield();

        yield return new WaitForSeconds(1f);

        yield return obj.TweenRotationY(180, 2).Yield();

        yield return new WaitForSeconds(1f);

        yield return obj.TweenLocalScale(new Vector3(0, 0, 0), 2).SetEaseCubicIn().Yield();

        yield return obj.TweenLocalScale(new Vector3(1, 1, 1), 1).SetEaseBackOut().SetOvershooting(0.25f).Yield();

        //assign the initial color at the end
        obj.GetComponent<Renderer>().material.color = myColor;

        //set the scale to 1
        obj.transform.localScale = Vector3.one;

        //null the reference to the coroutine so I can call it again when it's done
        coroutineSequence = null;

        print("Coroutine is over");
    }

    //access a custom shader and sets a value (see shaders and effects tutorial)
    void ChangeEffect(GameObject obj, float value)
    {
        obj.GetComponent<Renderer>().material.SetFloat("_Cutoff", value);
    }
}
