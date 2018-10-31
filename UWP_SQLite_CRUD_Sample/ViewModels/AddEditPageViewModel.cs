using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using DataBase;
using UWP_SQLite_CRUD_Sample.Views;
using Template10.Common;
using Template10.Services.NavigationService;

namespace UWP_SQLite_CRUD_Sample.ViewModels
{
    public class AddEditPageViewModel : ViewModelBase
    {
        #region Fields
        DelegateCommand _saveCommand;
        DelegateCommand _backCommand;
        NorthwindContext context = new NorthwindContext();
        INavigationService navigationService;
        #endregion

        public AddEditPageViewModel()
        {
            _saveCommand = new DelegateCommand(SaveExecute);
            _backCommand = new DelegateCommand(BackExecute);
            navigationService = WindowWrapper.Current().NavigationServices.FirstOrDefault();
        }

        #region Bindable properties
        int _id;
        public int Id
        {
            set { Set(ref _id, value); }
            get { return _id; }
        }

        string _productName;
        public string ProductName
        {
            set { Set(ref _productName, value); }
            get { return _productName; }
        }

        int _unitPrice;
        public int UnitPrice
        {
            set { Set(ref _unitPrice, value); }
            get { return _unitPrice; }
        }
        #endregion

        #region Commands

        #region SaveCommand
        public DelegateCommand Save
        {
            get { return _saveCommand ?? (_saveCommand = new DelegateCommand(SaveExecute)); }
        }

        private void SaveExecute()
        {
            Product newProduct = new Product
            {
                Id = Id,
                ProductName = ProductName,
                UnitPrice = UnitPrice,
                Discontinued = false,
                ReorderLevel = 1,
                UnitsOnOrder = 10,
                SupplierId = 1,
                UnitsInStock = 10 //Value with const set here for not to be set in the form 
            };
            context.Products.Add(newProduct);
            context.SaveChanges();
            navigationService.Navigate(typeof(MainPage));
        }
        #endregion

        #region BackCommand
        public DelegateCommand Back
        {
            get { return _backCommand ?? (_backCommand = new DelegateCommand(BackExecute)); }
        }

        private void BackExecute() =>
            navigationService.GoBack();
        #endregion

        #endregion

    }
}
