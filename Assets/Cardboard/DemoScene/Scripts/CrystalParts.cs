using UnityEngine;
using System.Collections;

public class CrystalParts : MonoBehaviour {

    private float AlphaValue = 1;
    private float timeforCP = 0;
    private bool state = false;

    public AudioClip sCrystalParts;
    protected AudioSource crystalpartsAudio;

    void Start()
    {
        crystalpartsAudio = this.audio;
        crystalpartsAudio.clip = sCrystalParts;
        crystalpartsAudio.Play();

        Invoke("crystalpartsDisappear", 3f);
    }

    void crystalpartsDisappear()
    {
        timeforCP += Time.deltaTime;

        if (timeforCP >= 0.1f)
        {
            state = true;
            timeforCP = 0;
        }
        if (state == true)
        {
            AlphaValue -= 0.2f;
            state = false;
        }
        if (AlphaValue <= -0.2)
        {
            AlphaValue = 0;
            Destroy(this.transform.gameObject);
            return;
        }
        foreach (Transform child in this.transform)
        {
            child.gameObject.renderer.material.SetFloat("_Alpha", AlphaValue);
        }
        Invoke("crystalpartsDisappear", Time.deltaTime);
    }
}
