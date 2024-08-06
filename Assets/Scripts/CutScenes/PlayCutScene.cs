using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PlayCutScene : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public KeyCode triggerKey = KeyCode.Space;

    void Update()
    {
        if (Input.GetKeyDown(triggerKey))
        {
            if (playableDirector.state != PlayState.Playing)
            {
                playableDirector.Play();
            }
        }
    }
}