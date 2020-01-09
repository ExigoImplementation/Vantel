using ReplicatedSite.Models;
using ReplicatedSite.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ReplicatedSite.Factories
{
    public static class ShoppingViewModelFactory
    {
        /// <summary>
        /// Creates a view model for shopping carts with the base class' properties populated.
        /// This is used to avoid having to populate the party and other common data for each shopping view model.
        /// </summary>
        /// <typeparam name="T">The type of view model needed.</typeparam>
        /// <param name="propertyBag">The cart's property bag.</param>
        /// <returns>Your pre-populated view model, ready for further population.</returns>
        public static T Create<T>(ShoppingCartCheckoutPropertyBag propertyBag) where T : IShoppingViewModel
        {
            var viewModel = Activator.CreateInstance<T>();

            viewModel.PropertyBag = propertyBag;

            return viewModel;
        }
    }
}