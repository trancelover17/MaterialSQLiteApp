using System.Collections.ObjectModel;

namespace MaterialDesign
{
    public class ViewModel
    {
        public ObservableCollection<string> CmbContent { get; private set; }

        public ViewModel(string[] items)
        {
            CmbContent = new ObservableCollection<string>(items);
        }
    }
}
