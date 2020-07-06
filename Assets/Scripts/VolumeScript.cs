using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeScript : MonoBehaviour
{
    private float volume;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        volume = 100f;
    }

    public float GetVolume()
    {
        return volume;
    }

    public void SetVolume(float new_volume)
    {
        volume = new_volume;
    }
}
