using System;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable, DisplayName("Cue Marker")]
public class CueMarker : Marker, IMarker, INotification, INotificationOptionProvider
{
    [SerializeField] private string message = "";
    [SerializeField] private bool retroactive = false;
    [SerializeField] private bool emitOnce = false;
    
    public PropertyName id => new PropertyName();
    public string Message => message;
    
    public NotificationFlags flags => (retroactive ? NotificationFlags.Retroactive : default) | (emitOnce ? NotificationFlags.TriggerOnce : default);



}