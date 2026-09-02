using TMPro;
using UnityEngine;

public class ShipStatusView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _speedText;

    private ShipStatusViewModel _viewModel;

    public void Initialize(ShipStatusViewModel viewModel)
    {
        _viewModel = viewModel;
        _viewModel.OnStatsChanged += UpdateDisplay;
    }

    private void UpdateDisplay()
    {
        _speedText.text = $"Speed: {_viewModel.Speed:F1}";
    }

    private void Update()
    {
        _viewModel.Tick();
    }

}
