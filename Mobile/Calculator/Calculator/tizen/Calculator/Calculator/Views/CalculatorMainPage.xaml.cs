﻿using System.Threading;
using Calculator.ViewModels;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace Calculator.Views
{
    /// <summary>
    /// A Calculator portrait layout class. </summary>
    public partial class CalculatorMainPage : ContentPage
    {
        private Mutex CounterMutex = new Mutex(false, "counter_mutex");
        private int counter;

        public CalculatorMainPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();

            MessagingCenter.Subscribe<MainPageViewModel, string>(this, "alert", (sender, arg) =>
            {
                CounterMutex.WaitOne();
                counter++;
                CounterMutex.ReleaseMutex();

                AlertToast.IsVisible = true;
                AlertToast.Text = arg.ToString();
                CloseAlertToast();
            });
        }

        /// <summary>
        /// A method close alert toast after 1.5 seconds later. </summary>
        async void CloseAlertToast()
        {
            await Task.Delay(1500);
            CounterMutex.WaitOne();
            if (--counter <= 0)
            {
                counter = 0;
                AlertToast.IsVisible = false;
            }

            CounterMutex.ReleaseMutex();
        }

        /// <summary>
        /// A event handler for updating scroll position of the expression label's scroll view
        /// </summary>
        /// <param name="sender">A expression label</param>
        /// <param name="e">A event handing argument</param>
        private void ExpressionLabelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (ExpressionScrollView == null ||
                ExpressionLabel == null)
            {
                return;
            }

            ExpressionScrollView.ScrollToAsync(0, ExpressionScrollView.ContentSize.Height, true);
        }
    }
}
