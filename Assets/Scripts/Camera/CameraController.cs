using UnityEngine;

namespace WSP.Camera
{
    public class CameraController : MonoBehaviour
    {
        static CameraController instance;
        UnityEngine.Camera mainCamera;

        [SerializeField] float cameraSpeed = 1;
        Vector2 targetPosition;
        
        void Awake()
        {
            mainCamera = UnityEngine.Camera.main;
            instance = this;
        }

        void Update()
        {
            var current = mainCamera.transform.position;
            var target = new Vector3(targetPosition.x, targetPosition.y, mainCamera.transform.position.z);
            mainCamera.transform.position = Vector3.Lerp(current, target, Time.deltaTime * cameraSpeed);
        }

        public static void SetTargetPosition(Vector2 position)
        {
            instance.targetPosition = position;
        }
        
        public static void ForceSetPosition(Vector2 position)
        {
            instance.targetPosition = position;
            instance.mainCamera.transform.position = new Vector3(position.x, position.y, instance.mainCamera.transform.position.z);
        }
    }
}