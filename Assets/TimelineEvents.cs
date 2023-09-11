using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//this must be added
using UnityEngine.Playables;

//INotificationReceiver class is necessary if this class wants to receive timeline notification
public class TimelineEvents : MonoBehaviour, INotificationReceiver
{
    public Material newSkybox;
    
    //called whrn a notification is triggered by the timeline on this object 
    public void OnNotify(Playable origin, INotification notification, object context)
    {
        //all notifications are sent here so I want to check if it's the (sub)class CueMarker
        if (notification is CueMarker)
        {
            //get the class reference
            CueMarker cue = notification as CueMarker;

            print("Notification received " + cue.Message+ " at "+cue.time);

            //do something based on a property of the cue
            if(cue.Message == "ohmygosh")
            {
                RenderSettings.skybox = newSkybox;
            }
        }
    }
}
