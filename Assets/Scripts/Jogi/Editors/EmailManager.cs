using UnityEngine;

public class EmailManager : MonoBehaviour
{
    public void OpenEmailApp()
    {
        string email = "info@taralityspace.com"; // Replace with your email address

#if UNITY_ANDROID
        AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
        AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
        intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SENDTO"));
        AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
        AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", "mailto:" + email);
        intentObject.Call<AndroidJavaObject>("setData", uriObject);

        AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");

        currentActivity.Call("startActivity", intentObject);
#elif UNITY_iOS
        string url = "mailto:" + email;
        Application.OpenURL(url);
#endif
    }
}
