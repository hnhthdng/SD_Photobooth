using PhotoBooth_App.Services;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace PhotoBooth_App
{
    public partial class QuantitySelectionPage : Page
    {
        public ObservableCollection<TypeSession> TypeSessions { get; set; } = new ObservableCollection<TypeSession>();
        private ApiService apiService = new ApiService();
        private int selectedId;
        private decimal selectedPrice;


        public QuantitySelectionPage()
        {
            InitializeComponent();
            LoadAdsFromApi();
        }

        private async void LoadAdsFromApi()
        {
            try
            {   
                var typeSessionList = await apiService.GetAsync<List<TypeSession>>("/api/TypeSession");
                TypeSessions.Clear();
                foreach (var session in typeSessionList)
                {
                    TypeSessions.Add(session);
                }

                DataContext = this;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải quảng cáo: " + ex.Message);
            }
        }
        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            var filtered = TypeSessions.ToList();

            if (int.TryParse(DurationFilterBox.Text.Trim(), out int duration))
            {
                filtered = filtered.Where(x => x.Duration <= duration).ToList();
            }

            if (decimal.TryParse(PriceFilterBox.Text.Trim(), out decimal price))
            {
                filtered = filtered.Where(x => x.Price <= price).ToList();
            }

            if (int.TryParse(AbleTakenFilterBox.Text.Trim(), out int ableTaken))
            {
                filtered = filtered.Where(x => x.AbleTakenNumber <= ableTaken).ToList();
            }

            TypeSessionGrid.ItemsSource = filtered;
        }

        private void ClearFilterButton_Click(object sender, RoutedEventArgs e)
        {
            DurationFilterBox.Text = "";
            PriceFilterBox.Text = "";
            AbleTakenFilterBox.Text = "";

            TypeSessionGrid.ItemsSource = TypeSessions;
        }

        private void TypeSessionGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TypeSessionGrid.SelectedItem is TypeSession selectedSession)
            {
                selectedId = selectedSession.Id;
                selectedPrice = selectedSession.Price;
                SelectedSessionText.Text = $"Bạn đã chọn loại dịch vụ: {selectedSession.Name}";
            }
            else
            {
                SelectedSessionText.Text = string.Empty;
            }
        }


        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            if (TypeSessionGrid.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn một loại dịch vụ trước khi tiếp tục.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            NavigationService.Navigate(new DepositPage(selectedId, selectedPrice));
        }

        public class TypeSession
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public int Duration { get; set; }
            public decimal Price { get; set; }
            public int AbleTakenNumber { get; set; }
        }

    }
}
