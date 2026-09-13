using TMPro;
using UnityEngine;

public class ShipStatusView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _speedText;
    [SerializeField] private TextMeshProUGUI _shipPositionText;
    [SerializeField] private TextMeshProUGUI _angleText;
    [SerializeField] private TextMeshProUGUI _timeLaserText;
    [SerializeField] private TextMeshProUGUI _countLaserText;

    private ShipStatusViewModel _viewModel;

    private void Update()
    {
        _viewModel.Tick();
    }

    public void Initialize(ShipStatusViewModel viewModel)
    {
        _viewModel = viewModel;
        _viewModel.OnStatsChanged += UpdateDisplay;
    }

    private void UpdateDisplay()
    {
        _speedText.text = $"Speed: {_viewModel.Speed:F1}";
        _shipPositionText.text = _viewModel.PositionText;
        _angleText.text = _viewModel.RotationText;
        _countLaserText.text = _viewModel.CurrentLaserCountText;
        _timeLaserText.text = _viewModel.LaserRechargeText;
    }
}