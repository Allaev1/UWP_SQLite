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
using Microsoft.EntityFrameworkCore;

namespace UWP_SQLite_CRUD_Sample.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        NorthwindContext context = new NorthwindContext();
        DelegateCommand _addCommand;
        ObservableCollection<Product> _availableProducts;
        ObservableCollection<Product> _discontinuedProducts;
        INavigationService navigationService;
        Product draggableItem;

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

        public Product SelectedAvailableProducts
        {
            set; get;
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
            draggableItem = e.Items.Last() as Product;
        }
        #endregion

        #region Events of discontinued list

        #region Reminder to DragOver
        //Usually used to implement logic that prevent
        //adding object of alien type to the type of items
        //of the list to what your are drag
        #endregion

        public void DragOver(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = DataPackageOperation.Move;
        }

        #region Reminder to Drop
        //Write the implementation of what to do when 
        //item have already draged in the other list
        #endregion

        public void Drop(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = DataPackageOperation.Move;

            Product becomeDiscontinuedItem = context.Products.First(a => a.Id == draggableItem.Id);
            //Item with old value of discontinued property
            //that will be removed on the same item but with edited discontinued prop on TRUE 
            //Product removableItem = context.Products.First(a => a.Id == becomeDiscontinuedItem.Id);

            _availableProducts.Remove(becomeDiscontinuedItem);
            context.Products.Remove(becomeDiscontinuedItem);
            context.SaveChanges();

            becomeDiscontinuedItem.Discontinued = true; //Change discontinued prop

            _discontinuedProducts.Add(becomeDiscontinuedItem);
            context.Products.Add(becomeDiscontinuedItem);

            context.SaveChanges();
        }
        #endregion
        #endregion
    }
}
