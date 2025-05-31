using UnityEngine;
using UnityEngine.XR;

public class CameraManager : MonoBehaviour
{
    public Camera aerialCamera;
    public Camera xrCamera;
    private bool usingAerial = false;

    private float lastSwitchTime = 0f;
    public float switchCooldown = 0.5f; // Seconds between switches

    void Start()
    {
        aerialCamera.gameObject.SetActive(false);
        xrCamera.gameObject.SetActive(true);
    }

    void Update()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        bool aButtonPressed = false;

        if (device.TryGetFeatureValue(CommonUsages.primaryButton, out aButtonPressed) && aButtonPressed)
        {
            if (Time.time - lastSwitchTime > switchCooldown)
            {
                SwitchCameras();
                lastSwitchTime = Time.time;
            }
        }
    }

    void SwitchCameras()
    {
        usingAerial = !usingAerial;
        aerialCamera.gameObject.SetActive(usingAerial);
        xrCamera.gameObject.SetActive(!usingAerial);
    }
}

