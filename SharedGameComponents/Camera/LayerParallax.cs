using UnityEngine;
using System.Collections;

namespace SGC.Cameras {
    /// <summary>
    /// [Not Implemented Yet] Support parallax effect to the game.
    /// </summary>
    [RequireComponent(typeof(BoxCollider))]
    public class LayerParallax : MonoBehaviour {
        
        //-----------------------------------------------------------------------------------------------------------------------------//			
        
        public CharacterController characterController;
        public float speed;
        public bool useTrigger;
        
        //-----------------------------------------------------------------------------------------------------------------------------//			

        private Vector3 velocity;
        private bool startParallax;
        
        //-----------------------------------------------------------------------------------------------------------------------------//			
        
        void Start() {
            this.characterController = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
            this.gameObject.collider.isTrigger = true;
        }
        
        //-----------------------------------------------------------------------------------------------------------------------------//			
        
        void Update() {
            if (this.useTrigger && !this.startParallax) return;
            this.velocity.x = this.characterController.velocity.x * this.speed;
            this.transform.Translate(this.velocity * Time.deltaTime);
        }
        
        //-----------------------------------------------------------------------------------------------------------------------------//			
        
        void OnTriggerEnter(Collider other) {
            if (!other.CompareTag(characterController.tag)) return;
            this.startParallax = true;
        }
        
        //-----------------------------------------------------------------------------------------------------------------------------//				
    }
}
