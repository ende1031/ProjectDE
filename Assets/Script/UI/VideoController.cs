using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    public string NextScene;
    public bool isResetGame = false;

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
            ChangeScene();
        }

        if(videoPlayer.isPlaying == false)
        {
            notPlayingTime += Time.deltaTime;
        }

        if(notPlayingTime > 1)
        {
            ChangeScene();
        }
    }

    void ChangeScene()
    {
        if(isResetGame == true)
        {
            SceneObjectManager.instance.ResetGame();
        }

        SceneChanger.instance.FadeAndLoadScene(NextScene);
    }
}
