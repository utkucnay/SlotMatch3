using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Case.Shared.ScreenFitter
{
#if UNITY_EDITOR
    [ExecuteAlways]
#endif
    public class ScreenFitter : MonoBehaviour
    {
        [SerializeField] Vector3 scale;

        ScreenFitterConfig screenFitterConfig = new();

        void Start()
        {
            FitToScreen();
        }

#if UNITY_EDITOR
        void Update()
        {
            FitToScreen();
        }
#endif

        void FitToScreen()
        {
            var mainCamera = Camera.main;
            if (mainCamera == null) return;

            float mainCameraAspect = mainCamera.aspect;
            float mainCameraSize = mainCamera.orthographicSize * 2;

            float refAspect = screenFitterConfig.RefWidth / (float)screenFitterConfig.RefHeight;

            float calculatedScaleX = refAspect * mainCameraSize;

            transform.localScale = new Vector3() 
            { 
                x = mainCameraAspect * mainCameraSize * (scale.x / calculatedScaleX), 
                y = scale.y, 
                z = scale.z
            };
        }
    }
}
