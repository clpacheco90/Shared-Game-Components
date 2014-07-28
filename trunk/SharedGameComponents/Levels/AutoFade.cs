using UnityEngine;
using System.Collections;

namespace SGC.Levels {
    public class AutoFade : MonoBehaviour {
        
        //-----------------------------------------------------------------------------------------------------------------------------//		

        public static bool Fading {
            get { return Instance.fading; }
        }
        
        //-----------------------------------------------------------------------------------------------------------------------------//			
        
        private Material material = null;
        private string levelName  = "";
        private int levelIndex    = 0;
        private bool fading       = false;
        private AsyncOperation async;        
        private static AutoFade instance = null;
        private static AutoFade Instance {
            get {
                if (instance == null) {
                    instance = (new GameObject("AutoFade")).AddComponent<AutoFade>();
                }
                return instance;
            }
        }
        
        //-----------------------------------------------------------------------------------------------------------------------------//			    

        private void Awake() {
            DontDestroyOnLoad(this);
            instance = this;
            material = new Material("Shader \"Plane/No zTest\" { SubShader { Pass { Blend SrcAlpha OneMinusSrcAlpha ZWrite Off Cull Off Fog { Mode Off } BindChannels { Bind \"Color\",color } } } }");
        }
        
        //-----------------------------------------------------------------------------------------------------------------------------//			
        
        private void DrawQuad(Color color, float alpha) {
            color.a = alpha;
            material.SetPass(0);
            GL.Color(color);
            GL.PushMatrix();
            GL.LoadOrtho();
            GL.Begin(GL.QUADS);
            GL.Vertex3(0, 0, -1);
            GL.Vertex3(0, 1, -1);
            GL.Vertex3(1, 1, -1);
            GL.Vertex3(1, 0, -1);
            GL.End();
            GL.PopMatrix();
        }
        
        //-----------------------------------------------------------------------------------------------------------------------------//			
        
        private IEnumerator Fade(float fadeOutTime, float fadeInTime, Color color, bool asyncLevel = false) {
            float t = 0.0f;
            while (t < 1.0f) {
                yield return new WaitForEndOfFrame();
                t = Mathf.Clamp01(t + Time.deltaTime / fadeOutTime);
                DrawQuad(color, t);
            }
            if (levelName != "") {
                if (asyncLevel) async = (AsyncOperation)Application.LoadLevelAsync(levelName);
                else Application.LoadLevel(levelName);
            } else {
                if (asyncLevel) async = (AsyncOperation)Application.LoadLevelAsync(levelIndex);
                else Application.LoadLevel(levelIndex);
            }

            while (t > 0.0f) {
                yield return new WaitForEndOfFrame();
                t = Mathf.Clamp01(t - Time.deltaTime / fadeInTime);
                DrawQuad(color, t);
            }
            fading = false;
        }
        
        //-----------------------------------------------------------------------------------------------------------------------------//			
        
        private void StartFade(float fadeOutTime, float fadeInTime, Color color, bool asyncLevel = false) {
            fading = true;
            StartCoroutine(Fade(fadeOutTime, fadeInTime, color, asyncLevel));
        }
        
        //-----------------------------------------------------------------------------------------------------------------------------//			
        
        public static void LoadLevel(string levelName, float fadeOutTime, float fadeInTime, Color color, bool asyncLevel = false) {
            if (Fading) return;
            Instance.levelName = levelName;
            Instance.StartFade(fadeOutTime, fadeInTime, color, asyncLevel);
        }
        
        //-----------------------------------------------------------------------------------------------------------------------------//			
        
        public static void LoadLevel(int levelIndex, float fadeOutTime, float fadeInTime, Color color) {
            if (Fading) return;
            Instance.levelName = "";
            Instance.levelIndex = levelIndex;
            Instance.StartFade(fadeOutTime, fadeInTime, color);
        }
        
        //-----------------------------------------------------------------------------------------------------------------------------//			
        
        public static void LoadLevel(LoadLevel.AutoFadeConfig autoFadeConfig) {
            if (Fading) return;
            Instance.levelName = autoFadeConfig.nextScene;
            Instance.StartFade(autoFadeConfig.fadeOutTime, autoFadeConfig.fadeInTime, autoFadeConfig.fadeColor);
        }
        
        //-----------------------------------------------------------------------------------------------------------------------------//			
        
        IEnumerator LoadingScene(string levelName) {
            async = (AsyncOperation)Application.LoadLevelAsync(levelName);
            Debug.Log(async.progress);
            if (async.isDone && async.progress >= 0.9f) {
                Debug.LogWarning(async.progress);
                yield return async;
            }
        }
        
        //-----------------------------------------------------------------------------------------------------------------------------//			
        
        IEnumerator LoadingScene(int levelIndex) {
            async = (AsyncOperation)Application.LoadLevelAsync(levelIndex);
            Debug.Log(async.progress);
            if (async.isDone && async.progress >= 0.9f) {
                Debug.LogWarning(async.progress);
                yield return async;
            }
        }
        
        //-----------------------------------------------------------------------------------------------------------------------------//			
    }
}