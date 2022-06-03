using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbisonicsController : MonoBehaviour
{
    private SteamAudio.SteamAudioAmbisonicSource source;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<SteamAudio.SteamAudioAmbisonicSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
