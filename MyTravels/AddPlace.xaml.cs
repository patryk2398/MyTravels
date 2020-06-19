using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MyTravels
{
    public partial class AddPlace : Window
    {
        public AddPlace()
        {
            InitializeComponent();
        }

        private void AddPlaceButtonClick(object sender, RoutedEventArgs e)
        {
            if (AddCountryTextBox.Text == "" || AddLocalityTextBox.Text == "" || AddTypeComboBox.Text == "" || AddRatingComboBox.Text == "" || AddDescriptionTextBox.Text == "" || imagePreview.Source == null)
            {
                MessageBox.Show("Wszystkie miejsca muszą być uzupełnione! Zdjęcie też.");
            }
            else
            {
                string location = ((BitmapImage)imagePreview.Source).UriSource.AbsolutePath;
                Places.addPlace(AddCountryTextBox.Text, AddLocalityTextBox.Text, AddTypeComboBox.Text, Convert.ToInt32(AddRatingComboBox.Text), AddDescriptionTextBox.Text, Places.CreateConnection(), location);
                this.Close();
                (Application.Current.MainWindow as MainWindow).Refresh();
                (Application.Current.MainWindow as MainWindow).RefreshSearchingTool();
                (Application.Current.MainWindow as MainWindow).defaultComboBox();
            }
        }

        private void AddImageButtonClick(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".jpg";
            dlg.Filter = "JPG Files (*.jpg)|*.jpg|JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png";

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                string selectedFileName = dlg.FileName;
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(selectedFileName);
                bitmap.EndInit();
                imagePreview.Source = bitmap;
            }
        }

        private void EditPlaceButtonClick(object sender, RoutedEventArgs e)
        {
            if (AddCountryTextBox.Text == "" || AddLocalityTextBox.Text == "" || AddTypeComboBox.Text == "" || AddRatingComboBox.Text == "" || AddDescriptionTextBox.Text == "" || imagePreview.Source == null)
            {
                MessageBox.Show("Wszystkie miejsca muszą być uzupełnione!");
            }
            else
            {
                string location;
                try
                {
                    location = ((BitmapImage)imagePreview.Source).UriSource.AbsolutePath;
                }
                catch
                {
                    location = null;
                }
                Places.editPlace(AddCountryTextBox.Text, AddLocalityTextBox.Text, AddTypeComboBox.Text, Convert.ToInt32(AddRatingComboBox.Text), AddDescriptionTextBox.Text, RowidTextBox.Text, Places.CreateConnection(), location);
                this.Close();
                (Application.Current.MainWindow as MainWindow).Refresh();
                (Application.Current.MainWindow as MainWindow).RefreshSearchingTool();
                (Application.Current.MainWindow as MainWindow).defaultComboBox();
            }
        }
    }
}
