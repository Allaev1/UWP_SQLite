using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using DataBase;
using UWP_SQLite_CRUD_Sample.Views;

namespace UWP_SQLite_CRUD_Sample.ViewModels
{
    public class AddEditPageViewModel : ViewModelBase
    {
        DelegateCommand _saveCommand;
        NorthwindContext context = new NorthwindContext();
        public AddEditPageViewModel()
        {
            _saveCommand = new DelegateCommand(SaveExecute);
        }

        //TODO: Rename propertie
        public Product Product { set; get; }

        #region Commands
        #region SaveCommand
        public DelegateCommand Save
        {
            get { return _saveCommand ?? (_saveCommand = new DelegateCommand(SaveExecute)); }
        }

        private void SaveExecute()
        {
            context.Products.Add(Product);
            context.SaveChanges();
            NavigationService.Navigate(typeof(MainPage));
        }
        #endregion
        #endregion

    }
}
