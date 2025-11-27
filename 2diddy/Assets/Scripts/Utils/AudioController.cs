using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance;

    public AudioSource pause;
    public AudioSource unpause;
    public AudioSource selectUpgrade;
    public AudioSource gameOver;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void PlaySound(AudioSource sound)
    {
        sound.Stop();
        sound.Play();
    }

 
}
