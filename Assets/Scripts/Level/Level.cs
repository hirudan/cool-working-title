using UnityEngine;

namespace Level
{
    /// <summary>
    /// Logic associated with a Unity Scene. Can handle movement of objects and instantiation of them.
    /// </summary>
    /// <remarks>
    /// Generally you want anything that should be at the start of the game to be
    /// placed in the scene first. For example the Camera which should already be in the Scene.
    /// </remarks>
    public class Level : MonoBehaviour
    {
        // Camera of the scene
        private Camera mainCamera;

        public string mainLightName = "Main Light";
        private Light mainLight;

        private void InitCamera()
        {
            // Enable the main camera for usage
            mainCamera.enabled = true;
            mainCamera.transform.position = new Vector3(0, 0, -10);

            // Look towards negate Z axis
            mainCamera.transform.forward = new Vector3(0, 0, 1);
            mainCamera.rect = new Rect(0f, 0f, 1f, 1f);

            // Background color instead of default skyline
            mainCamera.backgroundColor = Color.black;
            mainCamera.clearFlags = CameraClearFlags.SolidColor;
        }

        private void InitSun()
        {
            // Grab a directional light if possible otherwise create one
            foreach (var light in FindObjectsOfType<Light>())
            {
                if (light.type == LightType.Directional && light.name == this.mainLightName)
                {
                    mainLight = light;
                    break;
                }
            }

            if (mainLight == null)
            {
                GameObject lightGameObject = new GameObject(this.mainLightName);
                mainLight = lightGameObject.AddComponent<Light>();
                mainLight.type = LightType.Directional;
            }

            // Face the light as the same direction as the camera
            mainLight.transform.position = new Vector3(0, 0, 0);
            mainLight.transform.forward = new Vector3(0, 0, 1);
        }

        // Start is called before the first frame update
        private void Start()
        {
            // initialize class variables
            mainCamera = Camera.main;

            // Initialize Camera
            InitCamera();
            InitSun();
        }
    }
}
