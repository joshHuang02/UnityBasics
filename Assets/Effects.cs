using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

//fancy shaders from Cabbibo
//https://cabbibo.itch.io/

public class Effects : MonoBehaviour
{

    float angle = 0;

    public float colorTime = 0;

    public float counter = 0;

    private Color initialAmbientLight;

    public Light sun;
    public Light spotLight;

    public Color c1;
    public Color c2;

    public Material gradientSkybox;
    public Material starSkybox;

    //you need to import UnityEngine.Rendering.PostProcessing see above
    //dragged and dropped from the camera
    public PostProcessVolume postProcessing;

    //dragged and dropped from the scene
    public ParticleSystem smoke;

    // Start is called before the first frame update
    void Start()
    {

        //save the initial ambient light color
        initialAmbientLight = RenderSettings.ambientLight;

        //prevents skybox runtime changes from affecting the asset, just ignore
        RenderSettings.skybox = gradientSkybox = new Material(gradientSkybox);

        spotLight.intensity = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //oscillation (camera)

        //every public property in a component can be controlled by code!
        if (Input.GetKey(KeyCode.Z))
        {
            //same as oscillation above
            angle += 5 * Time.deltaTime;

            float sin = Mathf.Sin(angle);

            float FOV = Map(sin, -1, 1, 50f, 100f);

            Camera.main.fieldOfView = FOV;
        }


        //this kills all the lights and skybox to show the spotlight
        if (Input.GetKeyDown(KeyCode.X))
        {
            spotLight.intensity = 2;
            sun.intensity = 0;
            RenderSettings.ambientLight = Color.black;
            RenderSettings.skybox = starSkybox;
            DynamicGI.UpdateEnvironment();
        }
        
        //keyup restores the setting
        if (Input.GetKeyUp(KeyCode.X))
        {
            spotLight.intensity = 0;
            sun.intensity = 1;
            RenderSettings.ambientLight = initialAmbientLight;
            RenderSettings.skybox = gradientSkybox;
            DynamicGI.UpdateEnvironment();
        }


        //grassy david texture shift
        if (Input.GetKey(KeyCode.N))
        {
            counter += Time.deltaTime;

            //could be done faster but this is the step to step breakdown
            //access the object
            GameObject grassyDavid = GameObject.Find("grassyDavid");

            //access the renderer
            Renderer renderer = grassyDavid.GetComponent<Renderer>();
            
            //2 is the texture speed
            float offset = counter * 2;
            renderer.material.SetTextureOffset("_MainTex", new Vector2(0, offset));
            //the scrolling follows the UV map, this happens to be a model with good edge flow and unwrapping 
        }


        //furry david shader access
        if (Input.GetKey(KeyCode.C))
        {
            //just a time counter that advances when the key is pressed 
            counter += Time.deltaTime;

            //goes back and forth between 0 and 1 (second value)
            //2 makes it ping pong faster
            float t = Mathf.PingPong(counter * 2, 1);

            //ping pongs between 0.5 and 1
            float value = Mathf.Lerp(0.5f, 1, t);

            //use the same pingponging number to change color
            Color col = Color.Lerp(c1, c2, t);

            //could be done faster but this is the step to step breakdown
            //access the object
            GameObject furryDavid = GameObject.Find("furryDavid");
            
            //access the renderer
            Renderer renderer = furryDavid.GetComponent<Renderer>();

            //access the material and set a shader property
            renderer.material.SetFloat("_FurLength", value);

            //note how a different variable type requires a different setter
            renderer.material.SetColor("_Color", col);


            //exercise: change some other shader parament 

        }


        if (Input.GetKey(KeyCode.V))
        {
            //just a time counter that advances when the key is pressed 
            counter += Time.deltaTime;

            //make sure it cycles between 0 and 1
            float t = counter % 1;

            //it can be tricky to find the parameter but this is the idea
            postProcessing.profile.GetSetting<Bloom>().intensity.value = Mathf.Lerp(2, 8, t);

            //exercise: add some effect to the post processing volume and change them programmatically
        }

        //affect the smoke particle system
        if (Input.GetKey(KeyCode.B))
        {   
            //smoke is the particle system dragged n dropped from the scene
            //for some reason it has to be saved to a var
            var main = smoke.main;

            //then the properties can be changed
            main.startSizeMultiplier += 0.1f;

            //stop increasing at 10
            if (main.startSizeMultiplier > 30)
                main.startSizeMultiplier = 30;

            //some other properties here on the classes menu
            //https://docs.unity3d.com/ScriptReference/ParticleSystem.html
            //https://docs.unity3d.com/ScriptReference/ParticleSystem.MainModule.html
        }


    }
    //end update

    //similar to p5 map function remap OldValue from a range between OldMin and OldMax
    //to a range NewMin NewMax
    public static float Map(float OldValue, float OldMin, float OldMax, float NewMin, float NewMax, bool clamp = false)
    {
        float OldRange = (OldMax - OldMin);
        float NewRange = (NewMax - NewMin);
        float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;

        if (clamp)
            NewValue = Mathf.Clamp(NewValue, NewMin, NewMax);

        return (NewValue);
    }
}
