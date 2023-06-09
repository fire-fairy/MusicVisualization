using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;

public class DrumSoundPlayer : MonoBehaviour
{
    public AudioSource crash, kick_bass, snare, tom_1, tom_2, tom_3, tom_4;
    // Start is called before the first frame update
    void Start()
    {
        InputSystem.onDeviceChange += (device, change) =>
        {
            if (change != InputDeviceChange.Added) return;
            var midiDevice = device as Minis.MidiDevice;
            if (midiDevice == null) return;
            midiDevice.onWillNoteOn += (note, velocity) =>
        {
            // Note that you can't use note.velocity because the state
            // hasn't been updated yet (as this is "will" event). The note
            // object is only useful to specify the target note (note
            // number, channel number, device name, etc.) Use the velocity
            // argument as an input note velocity.
            switch (note.shortDisplayName)
            {
                case "D6":
                    tom_4.volume = velocity * 2f;
                    tom_4.Play();
                    break;
                case "E6":
                    tom_3.volume = velocity * 0.4f;
                    tom_3.Play();
                    break;
                case "F6":
                    tom_2.volume = velocity * 0.4f;
                    tom_2.Play();
                    break;
                case "G6":
                    tom_1.volume = velocity * 0.4f;
                    tom_1.Play();
                    break;
                case "A6":
                    snare.volume = velocity * 0.4f;
                    snare.Play();
                    break;
                case "B6":
                    kick_bass.volume = velocity;
                    kick_bass.Play();
                    break;
                case "C7":
                    crash.volume = velocity * 0.4f;
                    crash.Play();
                    break;
            }
        };
        };
    }
}
