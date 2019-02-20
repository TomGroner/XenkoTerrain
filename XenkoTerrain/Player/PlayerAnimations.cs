using Xenko.Core;
using Xenko.Engine;
using Xenko.Engine.Events;

namespace XenkoTerrain.Player
{
  /// <summary>
  /// Animates the player by subscribing to the player's movement changes and updating the current animation state.
  /// </summary>
  /// <seealso cref="Xenko.Engine.SyncScript" />
  public class PlayerAnimations : SyncScript
  {
    // Receives notifications from the player for whether or not they are on the ground or not.    
   private EventReceiver<bool> groundedEvent;

    // Receives notifications from the player for whether or not the character is moving or not.
    private EventReceiver<MovementState> movementEvent;

    /// <summary>
    /// The animation component attached to the player's model.
    /// </summary>
    /// <remarks>This is required. This script only coordinates the state, but defers the animation work itself to this component.</remarks>
    [DataMember(10)]
    public AnimationComponent AnimationComponent { get; set; }

    /// <summary>
    /// The character component associated with the player's model.
    /// </summary>    
    /// <remarks>This is required. This component is the source of the animation changes.</remarks>
    [DataMember(11)]
    public PlayerController CharacterController { get; set; }

    /// <summary>
    /// The current movement state that the running animation.
    /// </summary>    
    [DataMemberIgnore]
    public MovementState MovementState { get; set; }

    /// <summary>
    /// The current jump state of the running animation.
    /// </summary>    
    [DataMemberIgnore]
    public JumpingState JumpingState { get; set; }    

    public override void Start()
    {
      if (CanProcessAnimations())
      {        
        groundedEvent = new EventReceiver<bool>(CharacterController.CharacterIsJumpingEvent);
        movementEvent = new EventReceiver<MovementState>(CharacterController.MovementSpeedChangedEvent);
      }
    }

    public override void Update()
    {
      if (CanProcessAnimations())
      {
        var priorMovementState = MovementState;
        var priorJumpingState = JumpingState;

        UpdateMoveState(priorMovementState);
        UpdateJumpState(priorJumpingState);
        UpdateAnimationIfChanged(priorMovementState, priorJumpingState);
      }      
    }

    private void UpdateMoveState(MovementState priorMovementState)
    {
      if (movementEvent.TryReceive(out var newMovementState))
      {        
        MovementState = newMovementState;
      }
    }

    private void UpdateJumpState(JumpingState priorJumpingState)
    {
      if (groundedEvent.TryReceive(out var isGrounded))
      {
        if (isGrounded)
        {
          // The player is grounded now, so if the current animation is the end of a jump or already grounded then update
          // the state to be grounded.
          if (priorJumpingState == JumpingState.JumpEnd || priorJumpingState == JumpingState.Grounded)
          {
            JumpingState = JumpingState.Grounded;
          }
          else
          {
            // Otherwise the animation is in the middle of jumping so trigger the end of the jump animation.
            JumpingState = JumpingState.JumpEnd;
          }
        }
        else
        {
          // The character is in the air. If the animation is grounded start jumping. Otherwise continue jumping.
          JumpingState = (priorJumpingState == JumpingState.Grounded) ? JumpingState.JumpStart : JumpingState.JumpLoop;
        }
      }
    }

    private void UpdateAnimationIfChanged(MovementState priorMovementState, JumpingState priorJumpingState)
    {
      // If the character is or was jumping, the jump start or stop animation takes priority over changes in
      // running / walking.
      if (JumpingState != priorJumpingState)
      {
        ApplyJumpAnimation();
      }
      else if (IsGroundedAndMovementStateChanged(priorMovementState))
      {
        ApplyMovementAnimation();
      }
    }

    protected bool IsGroundedAndMovementStateChanged(MovementState priorMovementState)
    {
      return JumpingState == JumpingState.Grounded && MovementState != priorMovementState;
    }

    protected virtual void ApplyJumpAnimation()
    {
      switch (JumpingState)
      {
        // The character just stopped a jump sequence so defer to movement animations
        case JumpingState.Grounded: ApplyMovementAnimation(); break;

        // Otherwise the character is still in the jump animation sequence, apply the matching animation.
        case JumpingState.JumpStart: AnimationComponent.Play("Jump Start"); break;
        case JumpingState.JumpLoop: AnimationComponent.Play("Jump Loop"); break;          
        case JumpingState.JumpEnd: AnimationComponent.Play("Jump End"); break;       
        default: AnimationComponent.Play("Idle"); break;
      }
    }

    protected virtual void ApplyMovementAnimation()
    {
      switch (MovementState)
      {
        case MovementState.Walking: AnimationComponent.Play("Walk"); break;
        case MovementState.Running: AnimationComponent.Play("Run"); break;
        case MovementState.Idle: AnimationComponent.Play("Idle"); break;
        default: AnimationComponent.Play("Idle"); break;
      }
    }

    private bool CanProcessAnimations() => AnimationComponent != null && CharacterController != null;
  }
}