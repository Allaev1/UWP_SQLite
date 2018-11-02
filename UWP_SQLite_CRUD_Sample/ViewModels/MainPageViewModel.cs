using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Template10.Common;
using Template10.Services.NavigationService;
using DataBase;
using System.Collections.ObjectModel;
using UWP_SQLite_CRUD_Sample.Views;
using Windows.UI.Xaml.Controls;

namespace UWP_SQLite_CRUD_Sample.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        NorthwindContext context = new NorthwindContext();
        DelegateCommand _addCommand;
        ObservableCollection<Product> _availableProducts;
        ObservableCollection<Product> _discontinuedProducts;
        INavigationService navigationService;

        public MainPageViewModel()
        {
            _addCommand = new DelegateCommand(AddExecute);
            _availableProducts = new ObservableCollection<Product>(context.Products.Where(a => a.Discontinued != true));
            _discontinuedProducts = new ObservableCollection<Product>(context.Products.Where(a => a.Discontinued == true));
            navigationService = WindowWrapper.Current().NavigationServices.FirstOrDefault();
            //Creating instance of NavigationService in 26 line
            //Otherwise it will be null
            //TODO: Find out why it happens
        }

        #region Bindable proprties
        public ObservableCollection<Product> AvailableProducts
        {
            set { Set(ref _availableProducts, value); }
            get { return _availableProducts; }
        }

        public ObservableCollection<Product> DiscontinuedProducts
        {
            set { Set(ref _discontinuedProducts, value); }
            get { return _discontinuedProducts; }
        }
        #endregion

        #region Commands
        #region AddCommand
        public DelegateCommand AddCommand
        {
            get { return _addCommand ?? (_addCommand = new DelegateCommand(AddExecute)); }
        }

        private void AddExecute() =>
            navigationService.Navigate(typeof(AddEditPage));

        #endregion
        #endregion

        public void ListView_DragItemsStarting(object sender, DragItemsStartingEventArgs e)
        {

        }
    }
}
