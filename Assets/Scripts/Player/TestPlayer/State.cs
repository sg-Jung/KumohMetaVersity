using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    protected PlayerMovement _playerMovement;

    public State(PlayerMovement playerMovement)
    {
        _playerMovement = playerMovement;
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
}

public class FreezeState : State
{
    public FreezeState(PlayerMovement playerMovement) : base(playerMovement) { }

    public override void Enter() 
    {
        _playerMovement.state = PlayerMovement.MovementState.freeze;
        _playerMovement.rb.velocity = Vector3.zero;
    }

    public override void Update()
    {

    }

    public override void Exit()
    {

    }
}

public class WalkState : State
{
    public WalkState(PlayerMovement playerMovement) : base(playerMovement) { }

    public override void Enter()
    {
        _playerMovement.state = PlayerMovement.MovementState.walking;
        _playerMovement.moveSpeed = _playerMovement.walkSpeed;
    }

    public override void Update()
    {

    }

    public override void Exit()
    {

    }
}

public class SprintState : State
{
    public SprintState(PlayerMovement playerMovement) : base(playerMovement) { }

    public override void Enter()
    {
        _playerMovement.state = PlayerMovement.MovementState.sprinting;
        _playerMovement.moveSpeed = _playerMovement.sprintSpeed;
    }

    public override void Update()
    {

    }

    public override void Exit()
    {

    }
}

public class JumpState : State
{
    public JumpState(PlayerMovement playerMovement) : base(playerMovement) { }

    public override void Enter()
    {
        
    }

    public override void Update()
    {

    }

    public override void Exit()
    {

    }
}

public class CrouchState : State
{
    public CrouchState(PlayerMovement playerMovement) : base(playerMovement) { }

    public override void Enter()
    {
        _playerMovement.state = PlayerMovement.MovementState.crouching;
        _playerMovement.moveSpeed = _playerMovement.crouchSpeed;
        
        _playerMovement.crouched = true;

        _playerMovement.transform.localScale = new Vector3(_playerMovement.transform.localScale.x, _playerMovement.crouchYScale, _playerMovement.transform.localScale.z);
        _playerMovement.rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
    }

    public override void Update()
    {

    }

    public override void Exit()
    {
        _playerMovement.crouched = false;

        _playerMovement.transform.localScale = new Vector3(_playerMovement.transform.localScale.x, _playerMovement.startYScale, _playerMovement.transform.localScale.z);
    }
}

public class AirState : State
{
    public AirState(PlayerMovement playerMovement) : base(playerMovement) { }

    public override void Enter()
    {
        _playerMovement.state = PlayerMovement.MovementState.air;
    }

    public override void Update()
    {

    }

    public override void Exit()
    {

    }
}

public class SlideState : State
{
    public SlideState(PlayerMovement playerMovement) : base(playerMovement) { }

    public override void Enter()
    {
        _playerMovement.state = PlayerMovement.MovementState.slliding;
    }

    public override void Update()
    {
        // increase speed by one every second
        if (_playerMovement.OnSlope() && _playerMovement.rb.velocity.y < 0.1f)
            _playerMovement.moveSpeed = _playerMovement.slideSpeed;
    }

    public override void Exit()
    {

    }
}
