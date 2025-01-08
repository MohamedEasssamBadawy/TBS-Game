using UnityEngine;

public class ActionBuzyUI : MonoBehaviour
{

    private void Start() {
        UnitActionSystem.Instance.OnBuzyChanged += UnitActionSytem_OnBuzyChanged;

        Hide();
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

    private void UnitActionSytem_OnBuzyChanged(object sender, bool isBuzy) {
        if (isBuzy) {
            Show();
        }
        else {
            Hide();
        }
    }

}
