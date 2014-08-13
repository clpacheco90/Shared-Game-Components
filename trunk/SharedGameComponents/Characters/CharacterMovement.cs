using UnityEngine;
using System.Collections;

namespace SGC.Characters {
    public static class CharacterMovement {
        
        //-----------------------------------------------------------------------------------------------------------------------------//			
        
        private static bool jumping;
        
        //-----------------------------------------------------------------------------------------------------------------------------//			
        
        private static bool IsTouchingCeiling(ControllerMovement movement) {
            return (movement.collisionFlags & CollisionFlags.CollidedAbove) != 0;
        }
        
        //-----------------------------------------------------------------------------------------------------------------------------//	
        
        public static bool IsMovingHorizontal() {
            var h = Input.GetAxisRaw("Horizontal");
            return h != 0f;
        }

        //-----------------------------------------------------------------------------------------------------------------------------//	
        
        public static bool IsMovingHorizontal(out float h, bool abs = false) {
            h = Input.GetAxisRaw("Horizontal");
            return (abs) ? (Mathf.Abs(h) != 0f) : h != 0f;
        }
        
        //-----------------------------------------------------------------------------------------------------------------------------//	
        
        public static bool IsMovingVertical() {
            var v = Input.GetAxisRaw("Vertical");
            return v != 0f;
        }

        //-----------------------------------------------------------------------------------------------------------------------------//	
        
        public static bool IsMovingVertical(out float v, bool abs = false) {
            v = Input.GetAxisRaw("Vertical");
            return (abs) ? (Mathf.Abs(v) != 0f) : v != 0f;
            //return (abs) ? (Mathf.Abs(v) > 0.1f) : v != 0f;
        }

        public static bool IsMoving(out float h, out float v) {
            h = Input.GetAxisRaw("Horizontal");
            v = Input.GetAxisRaw("Vertical");            
            return (h != 0) || (v != 0) ? true : false;
        }
        
        //-----------------------------------------------------------------------------------------------------------------------------//	
        
        public static bool IsJumpping() {
            return jumping;
        }
        
        //-----------------------------------------------------------------------------------------------------------------------------//	
        
        private static bool JustBecameUngrounded(ControllerJumping jump) {
            return (Time.time < (jump.lastGroundedTime + jump.groundingTimeout) && jump.lastGroundedTime > jump.lastTime);
        }
        
        //-----------------------------------------------------------------------------------------------------------------------------//	
        
        private static float CalculateJumpVerticalSpeed(ControllerMovement movement, float targetJumpHeight) {
            // From the jump height and gravity we deduce the upwards speed 
            // for the character to reach at the apex.
            return Mathf.Sqrt(2 * targetJumpHeight * movement.gravity);
        }
        
        //-----------------------------------------------------------------------------------------------------------------------------//		
        
        private static void DidJump(ControllerMovement movement, ControllerJumping jump) {
            jumping              = jump.jumping = true;
            jump.reachedApex     = false;
            jump.lastTime        = Time.time;
            jump.lastStartHeight = movement.transform.position.y;
            jump.lastButtonTime  = -10;
            jump.touchedCeiling  = false;
            jump.buttonReleased  = false;

        }
        
        //-----------------------------------------------------------------------------------------------------------------------------//	

        public static void UpdateSmoothedMovementDirection(ref ControllerMovement movement, ControllerJumping jump, CharacterController controller, bool canControl = false) {
            var h = Input.GetAxisRaw("Horizontal");

            if (!canControl) h = 0.0f;

            movement.isMoving = Mathf.Abs(h) > 0.1f;

            //Debug.Log(h);
            if (movement.isMoving) movement.direction = new Vector3(h, movement.direction.y, movement.direction.z);

            var curSmooth = 0.0f; // Smooth the speed based on the current target direction
            var targetSpeed = Mathf.Min(Mathf.Abs(h), 1.0f); // Choose target speed

            if (controller.isGrounded) {
                curSmooth = movement.speedSmoothing * Time.smoothDeltaTime;
                targetSpeed *= movement.runSpeed;
                movement.hangTime = 0.0f;
                jumping = false;
            } else {
                curSmooth = jump.speedSmoothing * Time.smoothDeltaTime;
                targetSpeed *= jump.jumpSpeed;
                movement.hangTime += Time.smoothDeltaTime;
            }
            movement.speed = Mathf.Lerp(movement.speed, targetSpeed, curSmooth);
        }

        //-----------------------------------------------------------------------------------------------------------------------------//	

        public static void UpdateSmoothedMovementFreeDirection(ref ControllerMovement movement, CharacterController controller, bool canControl = false) {
            float h = 0.0f;
            float v = 0.0f;

            if (!canControl) {
                v = 0.0f;
                h = 0.0f;
                return;
            }

            //movement.isMoving = IsMovingHorizontal(out h, true) || IsMovingVertical(out v, true);
            //if (movement.isMoving) movement.direction = new Vector3(h, v, movement.direction.z);

            if ((IsMovingHorizontal(out h, true))&&(IsMovingVertical(out v, true)))
                movement.direction = new Vector3(h, v, movement.direction.z);
            if (IsMovingHorizontal(out h, true))
                movement.direction = new Vector3(h, movement.direction.y, movement.direction.z);
            else if (IsMovingVertical(out v, true))
                movement.direction = new Vector3(movement.direction.x, v, movement.direction.z);

            var curSmooth = 0.0f; // Smooth the speed based on the current target direction
            var tsMin = (h != 0.0f) ? h : v;
            var targetSpeed = Mathf.Min(Mathf.Abs(tsMin), 1.0f); // Choose target speed

            curSmooth = movement.speedSmoothing * Time.smoothDeltaTime;
            targetSpeed *= movement.runSpeed;
            movement.hangTime = 0.0f;
       
            movement.speed = Mathf.Lerp(movement.speed, targetSpeed, curSmooth);
        }
        
        //-----------------------------------------------------------------------------------------------------------------------------//	

        public static void ApplyGravity(ref ControllerMovement movement, ref ControllerJumping jump, CharacterController controller, bool canControl = false) {
            var jumpButton = Input.GetButton("Jump");

            if (!canControl) jumpButton = false;

            // When we reach the apex of the jump we send out a message		
            if (jump.jumping && !jump.reachedApex && movement.verticalSpeed <= 0.0) {
                jump.reachedApex = true;
                //? DidJump(movement,jump);
                //SendMessage ("DidJumpReachApex", SendMessageOptions.DontRequireReceiver);
            }

            // * When jumping up we don't apply gravity for some time when the user is holding the jump button
            //   This gives more control over jump height by pressing the button longer
            if (!jump.touchedCeiling && IsTouchingCeiling(movement)) {
                jump.touchedCeiling = true; // store this so we don't allow extra power jump to continue after character hits ceiling.
            }
            if (!jumpButton) {
                jump.buttonReleased = true;
            }

            var extraPowerJump = jump.jumping && movement.verticalSpeed > 0.0f && jumpButton && !jump.buttonReleased && movement.transform.position.y < jump.lastStartHeight + jump.extraHeight && !jump.touchedCeiling;

            if (extraPowerJump) {
                return;
            } else if (controller.isGrounded) {
                movement.verticalSpeed = -movement.gravity * Time.smoothDeltaTime;
            } else {
                movement.verticalSpeed -= movement.gravity * Time.smoothDeltaTime;
            }

            // Make sure we don't fall any faster than maxFallSpeed.  This gives our character a terminal velocity.
            movement.verticalSpeed = Mathf.Max(movement.verticalSpeed, -movement.maxFallSpeed);
        }
        
        //-----------------------------------------------------------------------------------------------------------------------------//	

        public static void ApplyJumping(ref ControllerMovement movement, ref ControllerJumping jump, CharacterController controller, bool canControl = false) {
            if (Input.GetButtonDown("Jump") && canControl) jump.lastButtonTime = Time.time;

            // Prevent jumping too fast after each other
            if (jump.lastTime + jump.repeatTime > Time.time) return;

            var isGrounded = controller.isGrounded;

            // Allow jumping slightly after the character leaves a ledge,
            // as long as a jump hasn't occurred since we became ungrounded.
            if (isGrounded || JustBecameUngrounded(jump)) {
                if (isGrounded) {
                    jump.lastGroundedTime = Time.time;
                }
                // Jump
                // - Only when pressing the button down
                // - With a timeout so you can press the button slightly before landing	
                if (jump.enabled && Time.time < jump.lastButtonTime + jump.timeout) {
                    movement.verticalSpeed = CalculateJumpVerticalSpeed(movement, jump.height);
                    // If we're on a platform, add the platform's velocity (times 1.4)
                    // to the character's velocity. We only do this if the platform
                    // is traveling upward.
                    if (movement.activePlatform) {
                        var apRb = movement.activePlatform.rigidbody;
                        if (apRb) {
                            var apRbY = movement.activePlatform.rigidbody.velocity.y;
                            if (apRbY > 0.0f) {
                                apRbY *= 1.4f;
                                movement.verticalSpeed += apRbY;
                            }
                        }
                    }
                    DidJump(movement, jump);
                }
            }
        }
        
        //-----------------------------------------------------------------------------------------------------------------------------//	
        
        public static Vector3 ChangeVector3Position(Vector3 from, Vector3 to, string Axis, string Axis2 = null) {
            if (Axis.ToUpper() == "X") {
                to = new Vector3(to.x, from.y, from.z);
            } else if (Axis.ToUpper() == "Y") {
                to = new Vector3(from.x, to.y, from.z);
            } else if (Axis.ToUpper() == "Z") {
                to = new Vector3(from.x, from.y, to.z);
            }
            if (!string.IsNullOrEmpty(Axis2)) {
                if (Axis2.ToUpper() == "X") {
                    to = new Vector3(to.x, from.y, from.z);
                } else if (Axis2.ToUpper() == "Y") {
                    to = new Vector3(from.x, to.y, from.z);
                } else if (Axis2.ToUpper() == "Z") {
                    to = new Vector3(from.x, from.y, to.z);
                }
            }
            return to;
        }
        
        //-----------------------------------------------------------------------------------------------------------------------------//	
    }
}