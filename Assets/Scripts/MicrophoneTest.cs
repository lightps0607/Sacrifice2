using UnityEngine;
#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif

public class MicrophoneTest : MonoBehaviour
{
    //GameObject dialog = null;

    [SerializeField] GameObject CustomDialogueObj;

    void Start()
    {
#if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            Permission.RequestUserPermission(Permission.Microphone);

            CustomDialogueObj.SetActive(true);

            //dialog = new GameObject();
            }
#endif
    }


    //    void OnGUI()
    //    {
    //#if PLATFORM_ANDROID
    //        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
    //        {
    //            // The user denied permission to use the microphone.
    //            // Display a message explaining why you need it with Yes/No buttons.
    //            // If the user says yes then present the request again
    //            // Display a dialog here.
    //            dialog.AddComponent<PermissionsRationaleDialog>();
    //            return;
    //        }
    //        else if (dialog != null)
    //        {
    //            Destroy(dialog);
    //        }
    //#endif

    //        // Now you can do things with the microphone
    //    }



    public void OnYes()
    {
#if PLATFORM_ANDROID
            Permission.RequestUserPermission(Permission.Microphone);
            CustomDialogueObj.SetActive(false);
#endif
    }

    public void OnNo()
    {
        CustomDialogueObj.SetActive(false);
    }


}