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

namespace UWP_SQLite_CRUD_Sample.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        NorthwindContext context = new NorthwindContext();
        DelegateCommand _addCommand;
        ObservableCollection<Product> _products;
        INavigationService navigationService;

        public MainPageViewModel()
        {
            _addCommand = new DelegateCommand(AddExecute);
            _products = new ObservableCollection<Product>(context.Products);
            navigationService = WindowWrapper.Current().NavigationServices.FirstOrDefault();
            //Creating instance of NavigationService in 26 line
            //Otherwise it will be null
            //TODO: Find out why it happens
        }

        #region Bindable proprties
        public ObservableCollection<Product> Products
        {
            set { Set(ref _products, value); }
            get { return _products; }
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
    }
}
