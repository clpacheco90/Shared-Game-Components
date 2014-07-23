using UnityEngine;
using System.Collections;

namespace SGC.Cameras {
    /// <summary>
    /// Provides the camera edges and the middle. Based on MainCamera.
    /// </summary>
    [System.Serializable]
    public class CameraPivot {

        //-----------------------------------------------------------------------------------------------------------------------------//

        public Vector3 left;
        public Vector3 right;
        public Vector3 center;

        //-----------------------------------------------------------------------------------------------------------------------------//

        private UnityEngine.Camera unityCamera = UnityEngine.Camera.main;

        //-----------------------------------------------------------------------------------------------------------------------------//

        public CameraPivot() {
            UpdatePivot();
        }

        //-----------------------------------------------------------------------------------------------------------------------------//

        public void UpdatePivot() {
            this.left   = unityCamera.ScreenToWorldPoint(Vector3.zero);
            this.right  = unityCamera.ScreenToWorldPoint(new Vector3(unityCamera.transform.position.x + Screen.width, unityCamera.transform.position.y + Screen.height, 0));
            this.center = unityCamera.ScreenToWorldPoint(new Vector3((unityCamera.transform.position.x + Screen.width) * .5f, (unityCamera.transform.position.y + Screen.height) * .5f, 0));
        }

        //-----------------------------------------------------------------------------------------------------------------------------//
    }
}
