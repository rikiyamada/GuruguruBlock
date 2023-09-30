using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    AudioSource audioSource;

    private void Start()
    {
        //audioSource = GetComponent<AudioSource>();

    }

    public void PlaySound()
    {
        VibrationMng.ShortVibration();
        /*if (TitleManager.isMute) return;
        audioSource.PlayOneShot(audioSource.clip);*/
    }
}
