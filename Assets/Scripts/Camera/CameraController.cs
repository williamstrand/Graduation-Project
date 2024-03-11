using UnityEngine;

namespace WSP.Camera
{
    public class CameraController : MonoBehaviour
    {
        static CameraController Instance;
        UnityEngine.Camera mainCamera;

        [SerializeField] float cameraSpeed = 1;
        Vector2 targetPosition;


        void Awake()
        {
            mainCamera = UnityEngine.Camera.main;
            Instance = this;
        }

        void Update()
        {
            var current = mainCamera.transform.position;
            var target = new Vector3(targetPosition.x, targetPosition.y, mainCamera.transform.position.z);
            mainCamera.transform.position = Vector3.Lerp(current, target, Time.deltaTime * cameraSpeed);
        }

        public static void SetTargetPosition(Vector2 position)
        {
            var transform = Instance.mainCamera.transform;
            Instance.targetPosition = position;
        }
    }
}