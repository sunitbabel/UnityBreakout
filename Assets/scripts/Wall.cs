using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Wall : MonoBehaviour
{
    private AudioSource _audio;

    private void Awake() => _audio = GetComponent<AudioSource>();

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ball"))
            _audio.Play();
    }
}