using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Nager.Date;

namespace payment
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Count(object sender, RoutedEventArgs e)
        {
            Calculation.Content = "";

            var dyzury = new List<Dyzur>();
            foreach (var date in Calendar.SelectedDates)
            {
                dyzury.Add(new Dyzur(date));
            }

            Hours20.Content = dyzury.Sum(d => d._20Hours());
            Hours50.Content = dyzury.Sum(d => d._50Hours());
            Hours100.Content = dyzury.Sum(d => d._100Hours());
            Hours0.Content = (dyzury.Sum(d => d._0Hours()) - dyzury.Sum(d => d._ZejsciaHours()));
            HoursZejscia.Content = dyzury.Sum(d => d._ZejsciaHours());

            Dates.Text = "Zaznaczone daty: " + string.Join(", ", dyzury.Select(x => x.StartTime.ToString("M")));

            double perhour;
            if (double.TryParse(PerHour.Text, out perhour))
            {
                var result = 2 * dyzury.Sum(d => d._100Hours()) + 1.5 * dyzury.Sum(d => d._50Hours()) +
                             .2 * dyzury.Sum(d => d._20Hours()) - dyzury.Sum(d => d._ZejsciaHours());

                Result.Content = result * perhour;

                Calculation.Content =
                    $"(2 * {dyzury.Sum(d => d._100Hours()):0.##} + 1.5 * {dyzury.Sum(d => d._50Hours()):0.##} + .2 * {dyzury.Sum(d => d._20Hours()):0.##} - {dyzury.Sum(d => d._ZejsciaHours()):0.##}) * {perhour}";
            }
        }

        private void Reset(object sender, RoutedEventArgs e)
        {
            Calendar.DisplayDate = DateTime.Today;
            Calendar.SelectedDates.Clear();
        }

        private void CountStawki()
        {
            var startDate = CalendarStawki.DisplayDate.AddDays(-CalendarStawki.DisplayDate.Day+1);
            var endTime = startDate.AddMonths(1).AddDays(-1);
            var currentMonth = DateSystem.GetPublicHoliday(CountryCode.PL, startDate,
                endTime);
            
            Stawki.Text = "Urlopy: " + string.Join(", ", CalendarStawki.SelectedDates.Select(x => x.ToString("M")));   
            Swieta.Text = "Swieta: " + string.Join(", ", currentMonth.Select(x => x.Date.ToString("M")));   
            
            double payment;
            var workingDays = 0;
            if (double.TryParse(Payment.Text, out payment))
            {
                foreach (var day in EachDay(startDate, endTime))
                {
                    if (day.DayOfWeek != DayOfWeek.Saturday && day.DayOfWeek != DayOfWeek.Sunday)
                    {
                        if (!DateSystem.IsPublicHoliday(day, CountryCode.PL))
                            if(!CalendarStawki.SelectedDates.Any(x=>x.Date==day.Date)) workingDays++;
                    }
                }

                var perHour = payment/(workingDays*(7 + ((double) 35 / (double) 60)));
                ResultStawki.Text = $"Dni pracujące w miesiącu {startDate:MMMM}: {workingDays}\r\nStawka godzinowa: {perHour:0.##}";
            }
        }
        
        private IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for(var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }

        private void ResetStawki(object sender, RoutedEventArgs e)
        {
            CalendarStawki.DisplayDate = DateTime.Today;
            CalendarStawki.SelectedDates.Clear();
        }

        private void CountStawki(object sender, SelectionChangedEventArgs e)
        {
            CountStawki();
        }

        private void CountStawki(object sender, TextChangedEventArgs e)
        {
            CountStawki();
        }
    }
}