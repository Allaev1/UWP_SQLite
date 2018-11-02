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
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml;

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

        #region EventHendlers

        #region Events of availale list
        public void DragItemsStarting(object sender, DragItemsStartingEventArgs e)
        {
            e.Data.RequestedOperation = DataPackageOperation.Move;
        }
        #endregion

        #region Events of discontinued list
        public void DragOver(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = DataPackageOperation.Move;
        }

        public void Drop(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = DataPackageOperation.Move;
        }
        #endregion
        #endregion
    }
}
