using Xenko.Core;
using Xenko.Engine;
using Xenko.Engine.Events;

namespace XenkoTerrain.Player
{
  public class PlayerAnimations : SyncScript
  {
    [DataMember(10)]
    public AnimationComponent AnimationComponent { get; set; }

    [DataMember(11)]
    public PlayerController CharacterController { get; set; }

    [DataMemberIgnore]
    public MovementState MovementState { get; set; }

    [DataMemberIgnore]
    public JumpingState JumpingState { get; set; }

    [DataMemberIgnore]
    public EventReceiver<bool> GroundedEvent;

    [DataMemberIgnore]
    private EventReceiver<MovementState> MovementSpeedEvent;    

    public override void Start()
    {
      if (CanProcessAnimations())
      {
        GroundedEvent = new EventReceiver<bool>(CharacterController.CharacterIsJumpingEvent);
        MovementSpeedEvent = new EventReceiver<MovementState>(CharacterController.MovementSpeedChangedEvent);
      }
    }

    public override void Update()
    {
      if (CanProcessAnimations())
      {
        var priorMovementState = MovementState;
        var priorJumpingState = JumpingState;

        UpdateMoveState();
        UpdateJumpState();

        UpdateAnimationIfChanged(priorMovementState, priorJumpingState);
      }      
    }

    private void UpdateMoveState()
    {
      if (MovementSpeedEvent.TryReceive(out var newMovementState))
      {
        MovementState = newMovementState;
      }
    }

    private void UpdateJumpState()
    {
      var priorJumpingState = JumpingState;

      if (GroundedEvent.TryReceive(out var isGrounded))
      {
        if (isGrounded)
        {
          JumpingState = (priorJumpingState == JumpingState.JumpEnd || priorJumpingState == JumpingState.Grounded) ? JumpingState.Grounded : JumpingState.JumpEnd;
        }
        else
        {
          JumpingState = (priorJumpingState == JumpingState.Grounded) ? JumpingState.JumpStart : JumpingState.JumpLoop;
        }
      }
    }

    private void UpdateAnimationIfChanged(MovementState priorMovementState, JumpingState priorJumpingState)
    {
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
        case JumpingState.JumpStart: AnimationComponent.Play("Jump Start"); break;
        case JumpingState.JumpLoop: AnimationComponent.Play("Jump Loop"); break;
        case JumpingState.JumpEnd: AnimationComponent.Play("Jump End"); break;
        case JumpingState.Grounded: ApplyMovementAnimation(); break;
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