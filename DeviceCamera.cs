using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Networking;


public class DeviceCamera : MonoBehaviour
{
    private bool camAvailable;
    private WebCamTexture backCam;
    private Texture defaultBackground;

    public DataStorage DS;
    public GameObject Picture;


    public RawImage background;
    public AspectRatioFitter fit;
    public GameObject Camera;

    public GameObject PhotoTakeButton;

    public float FlashWaitTime;

    private void Start()
    {
        foreach (AnimationState state in PhotoTakeButton.GetComponent<Animation>())
        {
            state.speed = 0.25F; //Set button animation speed, because it's too fast otherwise.
            //The button animation makes it grow and then shrink rapidly, letting users know the click registred.
        }

        defaultBackground = background.texture;
        WebCamDevice[] devices = WebCamTexture.devices;

        if (devices.Length == 0) //Test if any cameras are avalible.
        {
            Debug.Log("No camera detected");
            camAvailable = false;
            return;
        }

        
        if(SystemInfo.deviceModel == "Latitude 3420 (Dell Inc.)") //Test if running on dev PC. If so, use front cam. Else, use back cam
        {
            backCam = new WebCamTexture(devices[0].name, Screen.width, Screen.height);
        } else {
            for (int i = 0; i < devices.Length; i++)
            {
                if(!devices[i].isFrontFacing)
                {
                    backCam = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
                }
            }
        }
            
            

        if(backCam == null) //Test if camera was found
        {
            Debug.Log("Unable to find back camera");
            return;
        }

        Camera.GetComponent<Renderer>().material.mainTexture = backCam;
        background.texture = backCam;

        camAvailable = true;
        backCam.Play();
    }

    private void Update()
    {
        if(!camAvailable || background.texture != backCam)
        return;

        float ratio = (float)background.texture.width / (float)background.texture.height;
        //fit.aspectRatio = ratio;

        float scaleY = backCam.videoVerticallyMirrored ? -1f: 1f; //Magic!
        background.rectTransform.localScale = new Vector3(1f,scaleY,1f);

        int orient = -backCam.videoRotationAngle;
        background.rectTransform.localEulerAngles = new Vector3(0,0,orient);

        if (Picture.activeSelf == true) //This keeps the camera off when the viewing window is not shown
        {
            backCam.Play();
        } else {
            backCam.Stop();
        }
    }

    public void OnClicked() //function for the button to trigger
    {
        Debug.Log("Clicked!");
        StartCoroutine(TakePhoto());
    }
    

    IEnumerator TakePhoto()  // Start this Coroutine to take the photo
    {
        if (!PhotoTakeButton.GetComponent<Animation>().IsPlaying("ButtonPressed2")) //Dont take picture if the button is in it's animation.
        {
            PhotoTakeButton.GetComponent<Animation>().Play("ButtonPressed2"); //Button Animation


            string TeamNumber = DS.data["TeamNumber"];
            yield return new WaitForEndOfFrame();

            Texture2D photo = new Texture2D(background.texture.width, background.texture.height, TextureFormat.ARGB32, false);
            //background.texture.height = backCam.height;
            //background.texture.width = backCam.width;
            Graphics.CopyTexture(background.texture, photo);
            photo.SetPixels(photo.GetPixels());
            photo.Apply();
            background.texture = null; //camera off to give illusion of flash

            //Encode to a PNG
            byte[] bytes = photo.EncodeToPNG();
            Destroy(photo);
            //byte[] bytes = GetComponent<Renderer>().material.mainTexture.EncodeToPNG();
            //Write out the PNG.

            string filePath = Application.persistentDataPath + "/" + TeamNumber + ".png"; //Default path, may need adjusting if duplicate fils
            if (File.Exists(filePath))
            {
                int maxCount = 1000;
                for (int i = 1; i < maxCount + 1; i++) //code from DataStore
                {
                    string tmpFilePath = Application.persistentDataPath + "/" + TeamNumber + "-" + (maxCount - i + 1) + ".png"; //Try different possibilities until 1000, then give up
                    if (!File.Exists(tmpFilePath))
                    {
                        filePath = tmpFilePath;
                    }
                    if (i == maxCount && filePath == Application.persistentDataPath + "/" + TeamNumber + ".png")
                    {
                        Debug.LogError("Too many files! Couldn't save data, not clearing"); //Remnant of old code
                    }
                }
            }
            File.WriteAllBytes(filePath, bytes);
            Debug.Log("Took Photo");
            yield return new WaitForSeconds(FlashWaitTime); //Keep camera off to give the illusion of a flash
            background.texture = backCam; //Turn camera back on
        }
    }

    /* THIS CODE DOES NOT WORK YET
    IEnumerator UploadPNG() {
        DirectoryInfo dinfo = new DirectoryInfo(Application.persistentDataPath);
        foreach (FileInfo file in dinfo.GetFiles())
            {
                if (file.Name.StartsWith(".") || !file.Extension.Equals(".png"))
                    continue;
                // Create a Web Form
                WWWForm form = new WWWForm ();
                form.AddField ( "action", "Upload Image" );
                var bytes = File.ReadAllBytes(Application.persistentDataPath + "/" + file.Name);
                form.AddBinaryData ( "fileUpload", bytes, file.Name, "image/png" );
    
            // Upload to a cgi script
            using (var w = UnityWebRequest.Post(DS.serverBaseURL + "/api/v1/submit.php", form))
        {
            yield return w.SendWebRequest();
            if (w.result != UnityWebRequest.Result.Success) {
                print(w.error);
            }
            else {
                print("Finished Uploading Screenshot");
            }
        }
        }
    }

    public void SendData()
    {
        Debug.Log("Sending File!");
        StartCoroutine(UploadPNG());
    } */
}