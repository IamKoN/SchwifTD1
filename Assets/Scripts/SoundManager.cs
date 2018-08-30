using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

// Manages all the sounds. 
//Inherits from singleton to make the class into a singleton
public class SoundManager : Singleton<SoundManager>
{
    // A reference to the audios' source
    [SerializeField]
    private AudioSource musicSource;

    // A reference to the sfx Audiosource
    [SerializeField]
    private AudioSource sfxSource;

    // A reference to the sfxSlider
    [SerializeField]
    private Slider sfxSlider;
 
    /// A reference to the musicSlider
    [SerializeField]
    private Slider BGMSlider;

    // A dictionary for all the audio clips
    Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

    // Use this for initialization
    void Start()
    {
        //Instantiates the audioclips array by loading all audioclips from the assetfolder
        AudioClip[] clips = Resources.LoadAll<AudioClip>("Audio") as AudioClip[];

        //Stores all the audio clips
        foreach (AudioClip clip in clips)
        {
            audioClips.Add(clip.name, clip);
        }

        //Laods the saved volumes
        LoadVolume();

        //Makes the updatevolume listen to the musicsliders
        BGMSlider.onValueChanged.AddListener(delegate { UpdateVolume(); });
        sfxSlider.onValueChanged.AddListener(delegate { UpdateVolume(); });
    }

    // Plays an sfx sound
    /// <param name="name"></param>
    public void PlaySFX(string name)
    {
        //Plays the clip once
        sfxSource.PlayOneShot(audioClips[name]);
    }

    /// Updates the volumes according to the sliders
    private void UpdateVolume()
    {
        //Sets the music volume
        musicSource.volume = BGMSlider.value;

        //Sets the sfx volume
        sfxSource.volume = sfxSlider.value;

        //Saves the values
        PlayerPrefs.SetFloat("SFX", sfxSlider.value);
        PlayerPrefs.SetFloat("Music", BGMSlider.value);
    }

    // Loads the volumes 
    private void LoadVolume()
    {
        //Loads the sfx volume
        sfxSource.volume = PlayerPrefs.GetFloat("SFX", 0.5f);
        //Loads the muisc volumes
        musicSource.volume = PlayerPrefs.GetFloat("Music", 0.5f);

        //Updates the sliders
        BGMSlider.value = musicSource.volume;
        sfxSlider.value = sfxSource.volume;
    }
}
