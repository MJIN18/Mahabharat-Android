using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using StarterAssets;

public class VideoPlayerController : MonoBehaviour
{
    bool firstTime = true;
    int musicWasOn = 0;

    [SerializeField] string videoClipName;

    public VideoPlayer videoPlayer;
    public GameObject imageToHide;
    public GameObject skipButton;

    public float fadeDuration = 1f; // Duration of the fade-out effect in seconds
    public CanvasGroup canvasGroup;

    public CutsceneNo cutsceneNo;

    void Start()
    {
        SoundManager sManager = SoundManager.Instance;

        if (!sManager.musicSource.mute)
        {
            sManager.musicSource.mute = true;
            musicWasOn = 1;
        }

        skipButton.SetActive(false);

        switch (cutsceneNo)
        {
            case CutsceneNo.FIRST:
                if (PlayerPrefs.HasKey("IsFirstTime"))
                {
                    int i = PlayerPrefs.GetInt("IsFirstTime");

                    if (i != 0)
                    {
                        firstTime = false;
                    }
                }
                break;
            case CutsceneNo.SECOND:
                if (PlayerPrefs.HasKey("IsFirstTime2"))
                {
                    int i = PlayerPrefs.GetInt("IsFirstTime2");

                    if (i != 0)
                    {
                        firstTime = false;
                    }
                }
                break;
        }

        // Subscribe to the videoPlayer's loopPointReached event
        videoPlayer.loopPointReached += VideoPlayerLoopPointReached;

        // Start playing the video
        PlayVideo();

        if (!firstTime)
        {
            StartCoroutine(EnableSkipButton());
        }
    }

    public void PlayVideo()
    {
        VideoPlayer videoPlayer = GetComponent<VideoPlayer>();

        if (videoPlayer)
        {
            string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, videoClipName);
            videoPlayer.url = videoPath;
            videoPlayer.Play();
        }
    }

    IEnumerator EnableSkipButton()
    {
        yield return new WaitForSeconds(2f);

        skipButton.SetActive(true);
    }

    void VideoPlayerLoopPointReached(VideoPlayer vp)
    {
        // This function is called when the video completes
        // Set the image object to inactive
        imageToHide.SetActive(false);

        FadeOut();

        switch (cutsceneNo)
        {
            case CutsceneNo.FIRST:
                PlayerPrefs.SetInt("IsFirstTime", 1);
                break;
            case CutsceneNo.SECOND:
                PlayerPrefs.SetInt("IsFirstTime2", 1);
                break;
        }
    }

    public void SkipVideo()
    {
        // Stop the video
        videoPlayer.Stop();

        // Set the image object to inactive
        imageToHide.SetActive(false);

        FadeOut();
    }

    void FadeOut()
    {
        // Use a coroutine to smoothly fade out the UI image
        StartCoroutine(FadeCanvasGroup(canvasGroup, canvasGroup.alpha, 0f, fadeDuration));

        GameObject.Find("PlayerArmature").GetComponent<ThirdPersonController>().enabled = true;
    }

    IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float startAlpha, float targetAlpha, float duration)
    {
        float currentTime = 0f;

        while (currentTime < duration)
        {
            // Lerp the alpha value over time
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, currentTime / duration);

            // Increment the time
            currentTime += Time.deltaTime;

            yield return null;
        }

        // Ensure the target alpha is reached at the end
        canvasGroup.alpha = targetAlpha;
        gameObject.SetActive(false);

        if(musicWasOn != 0)
        {
            SoundManager.Instance.musicSource.mute = false;
        }
    }
}

public enum CutsceneNo
{
    FIRST,
    SECOND
}
