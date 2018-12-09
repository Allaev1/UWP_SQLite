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
        DelegateCommand _editCommand;
        ObservableCollection<Product> _availableProducts;
        ObservableCollection<Product> _discontinuedProducts;
        INavigationService navigationService;
        Product draggableItem;

        public MainPageViewModel()
        {
            _addCommand = new DelegateCommand(AddExecute);
            _editCommand = new DelegateCommand(EditExecute, CanEditExecute);
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

        Product _selectedProduct;
        public Product SelectedProduct
        {
            set
            {
                _selectedProduct = value;
                EditCommand.RaiseCanExecuteChanged();
            }
            get { return _selectedProduct; }
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

        #region EditCommand
        public DelegateCommand EditCommand
        {
            get { return _editCommand ?? (_editCommand = new DelegateCommand(EditExecute, CanEditExecute)); }
        }

        private bool CanEditExecute() =>
            SelectedProduct == null ? false : true;

        private void EditExecute() =>
            navigationService.Navigate(typeof(AddEditPage), SelectedProduct);
        #endregion
        #endregion

        #region Generic event handlers
        public void DragItemsStarting(object sender, DragItemsStartingEventArgs e)
        {
            e.Data.RequestedOperation = DataPackageOperation.Move;
            draggableItem = e.Items.Last() as Product;
        }

        #region Reminder to DragOver
        //Usually used to implement logic that prevent
        //adding object of alien type to the type of items
        //of the list to what your are drag
        #endregion

        public void DragOver(object sender, DragEventArgs e)
        {
            ListView listViewSender = sender as ListView;
            List<Product> currentList = new List<Product>(listViewSender.ItemsSource as IEnumerable<Product>);

            if (currentList.Count == 0 || currentList.Count == 0)
                e.AcceptedOperation = DataPackageOperation.Move;
            else if (draggableItem.Discontinued == false && currentList.First().Discontinued == false
                || draggableItem.Discontinued == true && currentList.First().Discontinued == true)
                e.AcceptedOperation = DataPackageOperation.None;
            else
                e.AcceptedOperation = DataPackageOperation.Move;
        }

        #region Reminder to Drop
        //Write the implementation of what to do when 
        //item have already draged in the other list
        #endregion

        public void Drop(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = DataPackageOperation.Move;

            Product item = context.Products.First(a => a.Id == draggableItem.Id);
            //Item with old value of discontinued property
            //that will be removed on the same item but with edited discontinued prop on TRUE 
            //Product removableItem = context.Products.First(a => a.Id == becomeDiscontinuedItem.Id);

            switch (item.Discontinued)
            {
                case true:
                    _discontinuedProducts.Remove(item);
                    context.Products.Remove(item);
                    context.SaveChanges();

                    item.Discontinued = false; //Change discontinued prop

                    _availableProducts.Add(item);
                    context.Products.Add(item);
                    break;
                case false:
                    _availableProducts.Remove(item);
                    context.Products.Remove(item);
                    context.SaveChanges();

                    item.Discontinued = true; //Change discontinued prop

                    _discontinuedProducts.Add(item);
                    context.Products.Add(item);
                    break;
            }

            context.SaveChanges();
        }
        #endregion
    }
}
