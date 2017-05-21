using UnityEngine;
using System;
using UnityEditor;

public class UnityVideoPlayerController : MonoBehaviour, IVideoPlayerController {

    private UnityEngine.Video.VideoPlayer vPlayer;
    private String videoName;
    private GameController gameController;

    private void Awake() {
        gameController = FindObjectOfType<GameController>();
        vPlayer = gameObject.GetComponent<UnityEngine.Video.VideoPlayer>();
    }


    public string GetVidName() {
        return videoName;
    }

    public void MoveTo(Vector3 newPos) {
        this.transform.position = newPos;
    }

    public void PauseVideo() {
        vPlayer.Pause();
    }

    public void PlayVideo() {
        vPlayer.Play();
    }

    public void PrepareVideo(string vidName) {         
        this.videoName = vidName;
        vPlayer.url = Application.streamingAssetsPath + "/" + vidName;
        vPlayer.Play(); // shouldn't need this ??? !!!
        vPlayer.prepareCompleted += Prepared;
        vPlayer.Prepare();

        vPlayer.isLooping = false;
        vPlayer.loopPointReached += EndReached;
    }

    private void Prepared(UnityEngine.Video.VideoPlayer vPlayer) {       // Note: This is currently not being reached !!!
        Debug.Log("Video prepared: " + this.videoName);
        vPlayer.Pause();

        // inform gameController vid is prepped
        gameController.VidInitCompleted(videoName);
    }

    private void EndReached(UnityEngine.Video.VideoPlayer vPlayer) {
        Debug.Log("Video End reached: " + this.videoName);
        // inform gameController vid ended
        gameController.OnMediaDecoderVidEnd(videoName);
    }

    public void PrepareVideos(string[] vidNames) {
        throw new NotImplementedException();
    }

    public void RotateVideo(int rotateY) {
        // Not implemented
    }

    public void StopVideo() {
        vPlayer.Stop();
    }

    public int SwitchVideo() {
        // Not implemented
        return 0;
    }
}