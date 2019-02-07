using System;
using Xenko.Core;
using Xenko.Core.Mathematics;
using Xenko.Engine;
using Xenko.Engine.Events;
using Xenko.Input;
using Xenko.Physics;

namespace XenkoTerrain.Player
{
  public class PlayerController : SyncScript
  {
    protected bool HadMove;
    protected bool IsJumping;
    protected CharacterComponent characterComponent;
    protected CameraComponent cameraComponent;
    protected Entity cameraTarget;
    protected Entity cameraContainer;
    protected Vector3 spawnPoint;    
    protected Vector3 currentEntityRotationXYZ;
    protected Vector3 howMuchToRotate;
    protected Vector3 howMuchAlreadyRotated;
    protected Vector3 cameraRotationXYZ;
    protected Vector3 targetCameraRotationXYZ;

    public PlayerController()
    {
      howMuchToRotate = new Vector3(0, 0, 0);
      howMuchAlreadyRotated = new Vector3(0, 0, 0);
    }

    /// <summary>
    /// Speed in degress per second of camera rotation
    /// </summary>    
    public float CharacterRotateDegreesPerSecond { get; set; } = 180f;

    /// <summary>
    /// Alpha value to use when lerping character rotation between updates.
    /// </summary>    
    public float CharacterRotateLerpAlpha { get; set; } = 0.15f;

    /// <summary>
    /// Velocity of the player while running.
    /// </summary>    
    public Vector3 RunSpeed { get; set; } = new Vector3(250.0f);

    /// <summary>
    /// Velocity of the player while walking.
    /// </summary>    
    public Vector3 WalkSpeed { get; set; } = new Vector3(100.0f);

    /// <summary>
    /// Allows inverting the camera horizontal direction. When true, yaw changes will go opposite of the mouse.
    /// </summary>    
    public bool InvertX { get; set; } = false;

    /// <summary>
    /// Allow inverting the camera vertical direction. When true, pitch changes will follow the direction of the mouse.
    /// </summary>
    public bool InvertY { get; set; } = false;

    /// <summary>
    /// Gets or sets the minimum angle that the camera can be pitched around the player. 
    /// Note: A value of 0.0f is the camera looking directly at the character's back.
    /// </summary>    
    public float MinVerticalAngle { get; set; } = -10.0f;

    /// <summary>
    /// Gets or sets the maximum angle that the camera can be pitched around the player.
    /// Note: A value of 90.0f is the camera looking straight down at the players head.
    /// </summary>    
    public float MaxVerticalAngle { get; set; } = 90f;

    /// <summary>
    /// How fast the camera can Yaw in degrees per second.
    /// </summary>
    public float VerticalSpeed { get; set; } = 65f;

    [DataMemberIgnore]
    public EventKey<bool> CharacterIsJumpingEvent { get; set; } = new EventKey<bool>();

    [DataMemberIgnore]
    public EventKey<MovementState> MovementSpeedChangedEvent { get; set; } = new EventKey<MovementState>();

    public override void Start()
    {
      spawnPoint = Entity.Transform.Position;
      characterComponent = Entity.Get<CharacterComponent>();

      if (characterComponent == null)
      {
        throw new ArgumentException($"{nameof(PlayerController)} must be attached to an entity with a {nameof(CharacterComponent)}.");
      }

      cameraComponent = Entity.SearchForComponent<CameraComponent>();

      if (cameraComponent == null)
      {
        throw new ArgumentException($"{nameof(PlayerController)} must be attached to an entity with a {nameof(CameraComponent)} or contain a child that does.");
      }

      cameraTarget = cameraComponent.Entity.GetParent();
      cameraContainer = cameraTarget.GetParent();
      targetCameraRotationXYZ = new Vector3(0f);
      cameraRotationXYZ = new Vector3(0f);
    }

    public override void Update()
    {
      var dt = (float)Game.UpdateTime.Elapsed.TotalSeconds;

      HadMove = false;
      PerformJumpIfNeeded();
      RotateOnY(dt);
      MoveForwardBackward(dt);

      if (IsJumping && !characterComponent.IsGrounded)
      {
        CharacterIsJumpingEvent.Broadcast(false);
      }
      else
      {
        CharacterIsJumpingEvent.Broadcast(true);
      }

      RotateCamera(dt);
    }

    private void PerformJumpIfNeeded()
    {
      if (ShouldJump())
      {
        characterComponent.Jump();
        IsJumping = true;
        HadMove = true;
      }
      else
      {
        IsJumping = !characterComponent.IsGrounded;
      }  
    }

    private void RotateOnY(float dt)
    {
      var rotationForUpdate = Vector2.Zero;

      if (ShouldTurnLeft())
      {
        rotationForUpdate -= Vector2.UnitX;
        HadMove = true;
      }

      if (ShouldTurnRight())
      {
        rotationForUpdate += Vector2.UnitX;
        HadMove = true;
      }

      // TODO: Need to separate the rotate speed when turning with keys vs the mouse. Make the mouse slower.

      howMuchToRotate.Y -= rotationForUpdate.X * dt * CharacterRotateDegreesPerSecond;

      var rotationAfterUpdate = howMuchAlreadyRotated.GetLerp(howMuchToRotate, CharacterRotateLerpAlpha);
      currentEntityRotationXYZ = new Vector3(MathUtil.DegreesToRadians(rotationAfterUpdate.X), MathUtil.DegreesToRadians(rotationAfterUpdate.Y), 0);
      Entity.Transform.RotationEulerXYZ = new Vector3(MathUtil.DegreesToRadians(rotationAfterUpdate.X), MathUtil.DegreesToRadians(rotationAfterUpdate.Y), 0);

      howMuchAlreadyRotated = rotationAfterUpdate;
    }

    private void MoveForwardBackward(float dt)
    {
      if (characterComponent.Entity.Transform.Position.Y < -10)
      {
        //characterComponent.Orientation
        HadMove = true;
        characterComponent.Teleport(spawnPoint);
      }
      else
      {
        var translationForUpdate = Vector3.Zero;
        var isRunning = Input.IsKeyDown(Keys.LeftShift) || Input.IsKeyDown(Keys.RightShift);
        var speedToUse = isRunning ? RunSpeed : WalkSpeed;

        if (ShouldMoveForwards())
        {
          translationForUpdate.Z = speedToUse.Z;
          HadMove = true;
        }

        if (ShouldMoveBackwards())
        {
          translationForUpdate.Z = -speedToUse.Z;
          HadMove = true;
        }

        translationForUpdate *= dt;

        characterComponent.SetVelocity(translationForUpdate.GetRotationVector(Entity.Transform));

        MovementSpeedChangedEvent.Broadcast(
          translationForUpdate.Length() <= 0 ? MovementState.Idle :
          isRunning ? MovementState.Running : MovementState.Walking);
      }
    }

    protected virtual bool ShouldJump()
    {
      return characterComponent.IsGrounded && Input.IsKeyPressed(Keys.Space);
    }

    protected virtual bool ShouldTurnRight()
    {
      if (Input.IsKeyDown(Keys.D))
      {
        return true;
      }

      if (ShouldMoveForwards() && Input.IsMouseButtonDown(MouseButton.Right) && Input.MouseDelta.X > 0)
      {
        return true;
      }

      return false;
    }

    protected virtual bool ShouldTurnLeft()
    {
      if (Input.IsKeyDown(Keys.A))
      {
        return true;
      }

      if (ShouldMoveForwards() && Input.IsMouseButtonDown(MouseButton.Right) && Input.MouseDelta.X < 0)
      {
        return true;
      }

      return false;
    }

    protected virtual bool ShouldMoveForwards()
    {
      return Input.IsKeyDown(Keys.W) || Input.IsMouseButtonDown(MouseButton.Right) && Input.IsMouseButtonDown(MouseButton.Left);
    }

    protected virtual bool ShouldMoveBackwards()
    {
      return Input.IsKeyDown(Keys.S);
    }

    private void RotateCamera(float dt)
    {
      if (Input.IsMouseButtonDown(MouseButton.Right))
      {
        var cameraMovement = new Vector2(Input.MouseDelta.X, Input.MouseDelta.Y) * 100;

        if (Input.IsKeyDown(Keys.W) || Input.IsKeyDown(Keys.S) || Input.IsMouseButtonDown(MouseButton.Right) && Input.IsMouseButtonDown(MouseButton.Left))
        {          
          cameraMovement.X = 0;
        }

        if (InvertX)
        {
          cameraMovement.X *= -1;
        }

        if (InvertY)
        {
          cameraMovement.Y *= -1;
        }

        targetCameraRotationXYZ.X += cameraMovement.Y * dt * VerticalSpeed;
        targetCameraRotationXYZ.X = Math.Min(targetCameraRotationXYZ.X, MaxVerticalAngle);
        targetCameraRotationXYZ.X = Math.Max(targetCameraRotationXYZ.X, MinVerticalAngle);
        targetCameraRotationXYZ.Y -= cameraMovement.X * dt * CharacterRotateDegreesPerSecond;
      }

      cameraRotationXYZ = Vector3.Lerp(cameraRotationXYZ, targetCameraRotationXYZ, 0.15f);
      cameraContainer.Transform.RotationEulerXYZ = new Vector3(MathUtil.DegreesToRadians(cameraRotationXYZ.X), MathUtil.DegreesToRadians(cameraRotationXYZ.Y), 0);
    }
  }
}