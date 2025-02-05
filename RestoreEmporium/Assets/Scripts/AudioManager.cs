using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    private List<StudioEventEmitter> eventEmitters;
    private List<EventInstance> eventInstances;
    private List<EventInstance> eventInstances3D;

    public static AudioManager _instance { get; private set; }

    private void Awake()
    {
        Singleton();

        eventEmitters = new List<StudioEventEmitter>();
        eventInstances = new List<EventInstance>();
        eventInstances3D = new List<EventInstance>();
    }

    //Makes sure there's only one of this object
    private void Singleton()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            _instance = this;
        }

        DontDestroyOnLoad(_instance);
    }

    //Allows you to change the parameters of sounds (so changing to the next segment of the track)
    public void SetEventParameter(EventInstance event_instance, string name, float value)
    {
        event_instance.setParameterByName(name, value);
    }

    //Allows you to change the global parameters of sounds (so volume, paused, etc)
    public void SetGlobalParameter(string name, float value)
    {
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName(name, value);
    }

    //Plays an instance of the sound at the specified position, e.g spawn gun sound at the player when you shoot.
    public void PlayOneShot(EventReference sound, Vector3 position)
    {
        RuntimeManager.PlayOneShot(sound, position);
    }

    //Allows you to create sound events within a 3D space and have them attach to a gameobject (look at radio for example)
    public EventInstance CreateEventInstance3D(EventReference sound, GameObject playerGO)
    {
        var instance = RuntimeManager.CreateInstance(sound);
        instance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(playerGO));
        RuntimeManager.AttachInstanceToGameObject(instance, playerGO);
        eventInstances3D.Add(instance);
        return instance;
    }

    //Allows you to create a sound event instance (use for sfx which are used frequently and that can change)
    public EventInstance CreateEventInstance(EventReference event_ref)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(event_ref);
        eventInstances.Add(eventInstance);
        return eventInstance;
    }

    //Allows you to create a studio event emitter for very specific instances (use mostly for music)
    public StudioEventEmitter InitaliseEventEmitter(EventReference event_ref, GameObject emitterGO)
    {
        StudioEventEmitter emitter = emitterGO.GetComponent<StudioEventEmitter>();
        emitter.EventReference = event_ref;
        eventEmitters.Add(emitter);
        return emitter;
    }

    //Returns a true or false statement on a sound, can be used to see if a sound has stopped playing.
    public bool CheckAudioClipStatus(EventReference event_ref)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(event_ref);

        eventInstance.getPlaybackState(out FMOD.Studio.PLAYBACK_STATE playbackstate);

        if (playbackstate == PLAYBACK_STATE.STOPPING) { return true; }
        else { return false; }
    }

    //Returns a true or false statement on a event instance, can be used to see if a voice line or piece of music has finished.
    public bool CheckEventInstanceStatus(EventInstance event_ref)
    {
        event_ref.getPlaybackState(out FMOD.Studio.PLAYBACK_STATE playbackstate);

        if (playbackstate == PLAYBACK_STATE.STOPPING) { return true; }
        else { return false; }
    }

    //Cleans up all the audio pieces which have been created, this is called if the audio manager is ever destroyed.
    private void CleanUp()
    {
        foreach (EventInstance eventInstance in eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }

        foreach (EventInstance eventInstance3D in eventInstances3D)
        {
            eventInstance3D.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance3D.release();
        }

        foreach (StudioEventEmitter emiiter in eventEmitters)
        {
            emiiter.Stop();
        }
    }


    private void OnDestroy()
    {
        CleanUp();
    }
}
