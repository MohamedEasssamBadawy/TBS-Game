using UnityEngine;

public class PathfindingUpdater : MonoBehaviour
{

    private void Start() {
        DestructibleCrate.OnAnyDestroyed += DestructibleCrate_OnAnyDestroyed;
    }

    private void OnDisable() {
        DestructibleCrate.OnAnyDestroyed -= DestructibleCrate_OnAnyDestroyed;
    }

    private void DestructibleCrate_OnAnyDestroyed(object sender, System.EventArgs e) {
        DestructibleCrate destructibleCrate = sender as DestructibleCrate;
        PathFinding.Instance.SetIsWalkableGridPosition(destructibleCrate.GetGridPosition(), true);
    }
}
