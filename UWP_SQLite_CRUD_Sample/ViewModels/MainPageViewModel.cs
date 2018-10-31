using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using DataBase;
using System.Collections.ObjectModel;

namespace UWP_SQLite_CRUD_Sample.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        NorthwindContext context = new NorthwindContext();

        #region Bindable proprties
        public ObservableCollection<Product> Products
        {
            get { return new ObservableCollection<Product>(context.Products); }
        }
        #endregion
    }
}
