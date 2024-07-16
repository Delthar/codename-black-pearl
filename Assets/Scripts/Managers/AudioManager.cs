using UnityEngine;

public class AudioManager : MonoBehaviour 
{
    [Header("Singleton")]
    [Tooltip("The unique instance of the Audio Manager")]
    [SerializeField] public static AudioManager Instance;

    [Header("Audio List")]
    [Tooltip("The list of all possible audio clips")]
    [SerializeField] private AudioListSO audioList;


    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable() 
    {
        CannonController.OnAnyFire += CannonController_OnAnyFire;    
    }

    private void OnDisable() 
    {
        CannonController.OnAnyFire -= CannonController_OnAnyFire;    
    }


    private void CannonController_OnAnyFire(object sender, CannonController.OnAnyFireEventArgs e)
    {
        AudioClip audioClip = audioList.fireSFX.GetRandomAudioClip();
        AudioSource.PlayClipAtPoint(audioClip, e.firePosition.position);
    }   
}