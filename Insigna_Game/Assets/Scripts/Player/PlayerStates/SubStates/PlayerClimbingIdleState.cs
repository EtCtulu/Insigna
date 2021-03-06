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
        player.SetVelocityY(0f);
        player.transform.position = new Vector2(playerData.ladderGO.transform.position.x, player.transform.position.y);
        player.MovementCollider.isTrigger = true;
        player.RB.gravityScale = 0;

        if (CursorManager.Instance.cursorState == false)
        {
            CursorManager.Instance.rend.enabled = false;
        }
    }

    public override void Exit()
    {
        base.Exit();

        if (CursorManager.Instance.cursorState == false)
        {
            CursorManager.Instance.rend.enabled = true;
            GameManager.Instance.globalInterractionSecurity = false;
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (CursorManager.Instance.cursorState == false)
        {
            GameManager.Instance.globalInterractionSecurity = true;
        }

        if (!isExitingState)
        {
            if (yInput == -1 || yInput == 1 && playerData.ladderTaken == true)
            {
                stateMachine.ChangeState(player.ClimbingState);
            }
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
