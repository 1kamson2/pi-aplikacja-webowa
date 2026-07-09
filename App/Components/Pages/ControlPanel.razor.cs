using Core.Components.Pages;
using Core.Utils;

namespace App.Components.Pages
{
    public partial class ControlPanel : BasePage
    {
        private Dictionary<Elements.Button.Type, List<Elements.Button.Information>> _buttonMap = new()
        {
            { Elements.Button.Type.EncryptionHistory, new() },
            { Elements.Button.Type.Algorithms, new() },
            { Elements.Button.Type.Histograms, new() },
            { Elements.Button.Type.SaveImage, new() }
        };

        public IEnumerable<Elements.Button.Information> GetButtonsByType(Elements.Button.Type type) => _buttonMap.TryGetValue(type, out var buttons) ? buttons : new();

        public ControlPanel() : base("/")
        {

        }

        public void OnButtonClicked(Elements.Button.Type type, int id)
        {
            Console.WriteLine("Hello World");
        }

        private void SaveImage() { }
        private void OpenEncryptionHistory() { }
        private void ViewHistograms() { }
        private void SelectAlgorithmEntropy() { }
    }
}