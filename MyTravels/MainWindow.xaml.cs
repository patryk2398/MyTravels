using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyTravels
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            List<Place> Place = new List<Place>();
            foreach (Place m in Places.PlaceList)
            {
                Place.Add(new Place(m.Rowid, m.Country, m.Locality, m.Type, m.Rating, m.Description, m.Image));
            }
            this.DataContext = Place;
        }

        private void RibbonAddPlaceButtonClick(object sender, RoutedEventArgs e)
        {
            AddPlace window = new AddPlace();
            window.Show();
            window.EditPlaceButton.Visibility = Visibility.Collapsed;
        }

        private void RibbonEditPlaceButtonClick(object sender, RoutedEventArgs e)
        {
            string id = RowidTextBlock.Text;
            if (id != "")
            {
                AddPlace window = new AddPlace();
                window.Show();
                window.AddPlaceButton.Visibility = Visibility.Collapsed;
                window.RowidTextBox.Text = RowidTextBlock.Text;
                window.AddCountryTextBox.Text = CountryTextBlock.Text;
                window.AddLocalityTextBox.Text = LocalityTextBlock.Text;
                window.AddTypeComboBox.Text = TypeTextBlock.Text;
                window.AddRatingComboBox.Text = RatingTextBlock.Text;
                window.AddDescriptionTextBox.Text = DescriptionTextBlock.Text;
                window.imagePreview.Source = fullImage.Source;
            }
            else
                MessageBox.Show("Nie można edytować, ponieważ nie wybrano miejsca!");

        }

        private void RibbonDeletePlaceButtonClick(object sender, RoutedEventArgs e)
        {
            string id = RowidTextBlock.Text;
            if (id != "")
            {
                string messageBoxText = "Czy na pewno usunąć Place?";
                string caption = "Usuń";
                MessageBoxButton button = MessageBoxButton.YesNo;
                MessageBoxImage icon = MessageBoxImage.Warning;
                MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        Places.deletePlace(Places.CreateConnection(), id);
                        RowidTextBlock.Text = "";
                        CountryTextBlock.Text = "";
                        LocalityTextBlock.Text = "";
                        TypeTextBlock.Text = "";
                        RatingTextBlock.Text = "";
                        DescriptionTextBlock.Text = "";
                        fullImage.Source = null;
                        Refresh();
                        RefreshSearchingTool();
                        defaultComboBox();
                        CountyLabel.Visibility = Visibility.Collapsed;
                        LocalityLabel.Visibility = Visibility.Collapsed;
                        TypeLabel.Visibility = Visibility.Collapsed;
                        RatingLabel.Visibility = Visibility.Collapsed;
                        DescriptionLabel.Visibility = Visibility.Collapsed;
                        fullImage.Visibility = Visibility.Collapsed;
                        break;
                    case MessageBoxResult.No:
                        break;
                }
            }
            else
                MessageBox.Show("Nie można usunąć, ponieważ nie wybrano miejsca!");

        }

        public void Refresh()
        {
            List<Place> Place = new List<Place>();
            foreach (Place m in Places.PlaceList)
            {
                Place.Add(new Place(m.Rowid, m.Country, m.Locality, m.Type, m.Rating, m.Description, m.Image));
            }
            this.DataContext = Place;

            if (RowidTextBlock.Text != "")
            {
                Place m = Places.showPlace(Places.CreateConnection(), RowidTextBlock.Text);
                CountryTextBlock.Text = m.Country;
                LocalityTextBlock.Text = m.Locality;
                TypeTextBlock.Text = m.Type;
                RatingTextBlock.Text = m.Rating.ToString();
                DescriptionTextBlock.Text = m.Description;

                BitmapImage bmImg = new BitmapImage();
                MemoryStream ms = new MemoryStream(m.Image);
                bmImg.BeginInit();
                bmImg.StreamSource = ms;
                bmImg.EndInit();
                fullImage.Source = bmImg as ImageSource;
            }
        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            string id = e.Parameter.ToString();
            Place m = Places.showPlace(Places.CreateConnection(), id);
            CountyLabel.Visibility = Visibility.Visible;
            LocalityLabel.Visibility = Visibility.Visible;
            TypeLabel.Visibility = Visibility.Visible;
            RatingLabel.Visibility = Visibility.Visible;
            DescriptionLabel.Visibility = Visibility.Visible;
            fullImage.Visibility = Visibility.Visible;
            RowidTextBlock.Text = m.Rowid.ToString();
            CountryTextBlock.Text = m.Country;
            LocalityTextBlock.Text = m.Locality;
            TypeTextBlock.Text = m.Type;
            RatingTextBlock.Text = m.Rating.ToString();
            DescriptionTextBlock.Text = m.Description;

            BitmapImage bmImg = new BitmapImage();
            MemoryStream ms = new MemoryStream(m.Image);
            bmImg.BeginInit();
            bmImg.StreamSource = ms;
            bmImg.EndInit();
            fullImage.Source = bmImg as ImageSource;
        }

        private void RibbonSearchPlaceButtonClick(object sender, RoutedEventArgs e)
        {
            if (SearchTool.Visibility == Visibility.Visible)
            {
                SearchTool.Visibility = Visibility.Collapsed;
            }
            else
            {
                SearchTool.Visibility = Visibility.Visible;
                RefreshSearchingTool();
            }
        }

        public void RefreshSearchingTool()
        {
            CountryComboBox.Items.Clear();
            LocalityComboBox.Items.Clear();
            TypeComboBox.Items.Clear();
            List<Place> Place = new List<Place>();
            foreach (Place m in Places.PlaceList)
            {
                bool repeatCountry = false;
                bool repeatLocality = false;
                bool repeatTypee = false;
                string country, locality, Typee;
                country = m.Country;
                locality = m.Locality;
                Typee = m.Type;
                foreach (Place m2 in Place)
                {
                    if (country == m2.Country)
                    {
                        repeatCountry = true;
                        break;
                    }
                }
                if (repeatCountry == false)
                {
                    CountryComboBox.Items.Add(country);
                }

                foreach (Place m2 in Place)
                {
                    if (locality == m2.Locality)
                    {
                        repeatLocality = true;
                        break;
                    }
                }
                if (repeatLocality == false)
                {
                    LocalityComboBox.Items.Add(m.Locality);
                }

                foreach (Place m2 in Place)
                {
                    if (Typee == m2.Type)
                    {
                        repeatTypee = true;
                        break;
                    }
                }
                if (repeatTypee == false)
                {
                    TypeComboBox.Items.Add(m.Type);
                }
                Place.Add(new Place(m.Rowid, m.Country, m.Locality, m.Type, m.Rating, m.Description, m.Image));
            }
        }

        private void searchButtonClick(object sender, RoutedEventArgs e)
        {
            string[] array = new string[5];
            string all = "";

            if (CountryComboBox.Text != "Kraj") array[0] = "Country='" + CountryComboBox.Text + "'";
            if (LocalityComboBox.Text != "Miejsce") array[1] = "Locality='" + LocalityComboBox.Text + "'";
            if (TypeComboBox.Text != "Typ") array[2] = "Type='" + TypeComboBox.Text + "'";
            if (minRatingComboBox.Text != "Ocena od") array[3] = "Rating >=" + minRatingComboBox.Text.ToString();
            if (maxRatingComboBox.Text != "Ocena do") array[4] = "Rating <=" + maxRatingComboBox.Text.ToString();

            for (int i = 0; i < 5; i++)
            {
                if (i == 4)
                {
                    all += array[i];
                    continue;
                }
                if (array[i] != null)
                {
                    all += array[i] + " AND ";
                }
            }
            if (all.EndsWith("AND ") == true)
            {
                all = all.Remove(all.Length - 4);
            }

            List<Place> Place = new List<Place>();
            foreach (Place m in Places.searchPlace(all))
            {
                Place.Add(new Place(m.Rowid, m.Country, m.Locality, m.Type, m.Rating, m.Description, m.Image));
            }
            this.DataContext = Place;
        }

        private void refreshButtonClick(object sender, RoutedEventArgs e)
        {
            Refresh();
            defaultComboBox();
        }

        public void defaultComboBox()
        {
            CountryComboBox.Text = "Kraj";
            LocalityComboBox.Text = "Miejsce";
            TypeComboBox.Text = "Typ";
            minRatingComboBox.Text = "Ocena od";
            maxRatingComboBox.Text = "Ocena do";
        }

        private void SelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            LocalityComboBox.Items.Clear();
            try
            {
                string country = CountryComboBox.SelectedItem.ToString();
                List<Place> Place = new List<Place>();
                foreach (Place m in Places.searchLocality(country))
                {
                    bool repeatLocality = false;
                    string locality = m.Locality;

                    foreach (Place m2 in Place)
                    {
                        if (locality == m2.Locality)
                        {
                            repeatLocality = true;
                            break;
                        }
                    }
                    if (repeatLocality == false)
                    {
                        LocalityComboBox.Items.Add(m.Locality);
                    }
                }
            }
            catch
            {
            }
        }
    }
}
