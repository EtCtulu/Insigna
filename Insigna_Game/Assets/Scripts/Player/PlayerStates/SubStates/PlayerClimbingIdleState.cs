using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClimbingIdleState : PlayerGroundedState
{
    public PlayerClimbingIdleState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
        player.SetVelocityX(0f);
        playerData.ladderTaken = false;
        player.transform.position = new Vector2(playerData.ladderGO.transform.position.x, player.transform.position.y);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isExitingState)
        {
            /*if (yInput == -1)
            {
                stateMachine.ChangeState(player.CrouchIdleState);
            }*/
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
