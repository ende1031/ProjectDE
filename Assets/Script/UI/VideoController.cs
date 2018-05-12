using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    VideoPlayer videoPlayer;
    float notPlayingTime = 0;

    void Start ()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.Play();
    }
	
	void Update ()
    {
        if (Input.GetKeyUp(KeyCode.X) || Input.GetKeyUp(KeyCode.Escape))
        {
            GameStart();
        }

        if(videoPlayer.isPlaying == false)
        {
            notPlayingTime += Time.deltaTime;
        }

        if(notPlayingTime > 1)
        {
            GameStart();
        }
    }

    void GameStart()
    {
        SceneObjectManager.instance.SetUIActive(true);
        SceneChanger.instance.FadeAndLoadScene("Stage01", 6);
    }
}
