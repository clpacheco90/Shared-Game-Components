using UnityEngine;
using System.Collections;

namespace SGC.Characters {
    public class Character : MonoBehaviour {

        //-----------------------------------------------------------------------------------------------------------------------------//
        
        [System.Serializable]
        public class DebugingCharacter {
            public bool godMode;
            public bool debugLog;
            public bool cheats;
        }

        //-----------------------------------------------------------------------------------------------------------------------------//
        
        public int live;
        public DebugingCharacter debugingCharacter = new DebugingCharacter(); //! Not Tested Yet
        
        //-----------------------------------------------------------------------------------------------------------------------------//
        
        #region UnityMethods
        
        //-----------------------------------------------------------------------------------------------------------------------------//

        public virtual void Awake() {
        }

        //-----------------------------------------------------------------------------------------------------------------------------//

        public virtual void Start() {
        }

        //-----------------------------------------------------------------------------------------------------------------------------//

        public virtual void Update() {
            if (debugingCharacter.debugLog) {
                Debug.Log("My name is " + Name());
            }
            //Cheats();
        }

        //-----------------------------------------------------------------------------------------------------------------------------//

        public virtual void FixedUpdate() {
        }

        //-----------------------------------------------------------------------------------------------------------------------------//

        public virtual void OnTriggerStay(Collider other) {
        }

        //-----------------------------------------------------------------------------------------------------------------------------//

        public virtual void OnTriggerExit(Collider other) {
        }

        //-----------------------------------------------------------------------------------------------------------------------------//

        public virtual void OnTriggerEnter(Collider other) {
        }

        //-----------------------------------------------------------------------------------------------------------------------------//

        #endregion

        //-----------------------------------------------------------------------------------------------------------------------------//

        #region CharacterMethods
        
        //-----------------------------------------------------------------------------------------------------------------------------//


        public virtual string Name() {
            throw new System.NotImplementedException();        
        }
        
        //-----------------------------------------------------------------------------------------------------------------------------//

        public virtual float Hit() {
            throw new System.NotImplementedException();
        }

        //-----------------------------------------------------------------------------------------------------------------------------//

        public virtual float Damage() {
            throw new System.NotImplementedException();
        }

        //-----------------------------------------------------------------------------------------------------------------------------//

        public virtual bool Die() {
            throw new System.NotImplementedException();
        }

        //-----------------------------------------------------------------------------------------------------------------------------//

        public virtual void Cheats() {
            throw new System.NotImplementedException();
        }

        //-----------------------------------------------------------------------------------------------------------------------------//

        #endregion

        //-----------------------------------------------------------------------------------------------------------------------------//
    }
}