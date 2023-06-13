using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;

public class PianoSoundPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource F3,G3, A3,B3, C4, D4, E4, F4, G4, A4, B4, C5, D5, E5, F5, G5, A5, B5, C6, soundtrack;

    public static IEnumerator FadeMusic(AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = audioSource.volume;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }
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
                    case "F3":
                        F3.volume = velocity;
                        F3.Play();
                        break;
                    case "G3":
                        G3.volume = velocity;
                        G3.Play();
                        break;
                    case "A3":
                        A3.volume = velocity;
                        A3.Play();
                        break;
                    case "B3":
                        B3.volume = velocity;
                        B3.Play();
                        break;
                    case "C4":
                        C4.volume = velocity;
                        C4.Play();
                        break;
                    case "D4":
                        D4.volume = velocity;
                        D4.Play();
                        break;
                    case "E4":
                        E4.volume = velocity;
                        E4.Play();
                        break;
                    case "F4":
                        F4.volume = velocity;
                        F4.Play();
                        break;
                    case "G4":
                        G4.volume = velocity;
                        G4.Play();
                        break;
                    case "A4":
                        A4.volume = velocity;
                        A4.Play();
                        break;
                    case "B4":
                        B4.volume = velocity;
                        B4.Play();
                        break;
                    case "C5":
                        C5.volume = velocity;
                        C5.Play();
                        break;
                    case "D5":
                        D5.volume = velocity;
                        D5.Play();
                        break;
                    case "E5":
                        E5.volume = velocity;
                        E5.Play();
                        break;
                    case "F5":
                        F5.volume = velocity;
                        F5.Play();
                        break;
                    case "G5":
                        G5.volume = velocity;
                        G5.Play();
                        break;
                    case "A5":
                        A5.volume = velocity;
                        A5.Play();
                        break;
                    case "B5":
                        B5.volume = velocity;
                        B5.Play();
                        break;
                    case "C6":
                        C6.volume = velocity;
                        C6.Play();
                        break;
                    case "F#3":
                        soundtrack.Play();
                        break;
                    case "G#3":
                        soundtrack.Stop();
                        break;
                }
            };
        };
    }
}
