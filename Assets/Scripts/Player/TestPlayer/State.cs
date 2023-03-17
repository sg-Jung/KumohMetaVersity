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

public class IdleState : State
{
    public IdleState(PlayerMovement playerMovement) : base(playerMovement) { }

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

public class WalkState : State
{
    public WalkState(PlayerMovement playerMovement) : base(playerMovement) { }

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

public class RunningState : State
{
    public RunningState(PlayerMovement playerMovement) : base(playerMovement) { }

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

    }

    public override void Update()
    {

    }

    public override void Exit()
    {

    }
}

public class AirState : State
{
    public AirState(PlayerMovement playerMovement) : base(playerMovement) { }

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
