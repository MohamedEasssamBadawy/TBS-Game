using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveAction : BaseAction
{
    public event EventHandler OnStartMoving;
    public event EventHandler OnStopMoving;



    [SerializeField] private int maxMoveDistance = 4;

    private List<Vector3> positionList;
    private int currentPositionIndex;
   
    private void Update() {
        if (!isActive) return;

        Vector3 targetPosition = positionList[currentPositionIndex];
        Vector3 moveDirection = (targetPosition - transform.position).normalized;

        float rotateSpeed = 10f;
        transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);

        float stoppingDistance = .1f;
        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance) {
            float moveSpeed = 4;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;

        }
        else {

            currentPositionIndex++;
            if (currentPositionIndex >= positionList.Count) {
                OnStopMoving?.Invoke(this, EventArgs.Empty);
                ActionComplete();
            }
        }
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete) {
        List<GridPosition> pathGridPositionList =
            PathFinding.Instance.FindPath(unit.GetGridPosition(), gridPosition, out int pathLength);

        currentPositionIndex = 0;
        positionList = new List<Vector3>();

        foreach (GridPosition pathGridPosition in pathGridPositionList) {
            positionList.Add(LevelGrid.Instance.GetWorldPosition(pathGridPosition));
        }

        OnStartMoving?.Invoke(this, EventArgs.Empty);

        ActionStart(onActionComplete);
    }

    public override List<GridPosition> GetValidActionGridPositionList() {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();

        for(int x = -maxMoveDistance; x <= maxMoveDistance; x++) {
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++) {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) {
                    continue;
                }

                if (unitGridPosition == testGridPosition) {
                    continue;
                }

                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition)) {
                    // Grid Position already occupied with another unit...
                    continue;
                }

                if (!PathFinding.Instance.IsWalkableGridPosition(testGridPosition)) {
                    continue;
                }

                if (!PathFinding.Instance.HasPath(unitGridPosition, testGridPosition)) {
                    continue;
                }

                int pathfindingDistanceMulitplier = 10;
                if (PathFinding.Instance.GetPathLength(unitGridPosition, testGridPosition)
                    > maxMoveDistance * pathfindingDistanceMulitplier){
                    // Path length is too long.
                    continue;
                }
        
                validGridPositionList.Add(testGridPosition);
                //Debug.Log(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override string GetActionName() {
        return "Move";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition) {

        int targetCountAtGridPosition = unit.GetAction<ShootAction>().GetTargetCountAtPosition(gridPosition);

        return new EnemyAIAction {
            gridPosition = gridPosition,
            actionValue = targetCountAtGridPosition * 10,
        };
    }
}
