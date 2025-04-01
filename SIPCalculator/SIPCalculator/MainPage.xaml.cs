
using Syncfusion.Maui.Toolkit.Charts;
using System.Collections.ObjectModel;
using System.Globalization;

namespace SIPCalculator
{
    public partial class MainPage : ContentPage
    {
        private bool isInvestAmount { get; set; }
        private bool isGoalAmount { get; set; }
        private bool isStepUpInvest { get; set; }
        private bool isSIP { get; set; }
        private bool isLumpSum { get; set; }
        private bool isMutualFund { get; set; }
        private bool isMutalFundMonthlySIP { get; set; }
        public MainPage()
        {
            InitializeComponent();
            isInvestAmount = true;
        }

        public void Dynamically_Get_Investment_DataCollection()
        {
            var data = new ObservableCollection<Model>();

            if (isSIP)
            {
                double expectedReturnsValue = viewModel.ExpectedReturns / 100;
                double rateofInterest = Math.Pow(1 + expectedReturnsValue, 1.0 / 12) - 1;

                if (isStepUpInvest)
                {
                    double stepUpRate = viewModel.AnnualStepUp / 100;
                    double monthlyInvestment = viewModel.IntialAmount;
                    double totalInvested = 0;
                    double totalCorpus = 0;
                    double previousYearCorpus = 0;

                    for (int period = 1; period <= viewModel.InvestPeriod; period++)
                    {
                        double yearlyInvestment = monthlyInvestment * 12;
                        totalInvested += yearlyInvestment;
                        double fvFactor = (Math.Pow(1 + rateofInterest, 12) - 1) / rateofInterest;
                        double currentYearFV = monthlyInvestment * fvFactor * (1 + rateofInterest);
                        if (period > 1)
                        {
                            previousYearCorpus *= (1 + expectedReturnsValue);
                        }

                        totalCorpus = previousYearCorpus + currentYearFV;
                        double estimatedReturns = totalCorpus - totalInvested;

                        data.Add(new Model()
                        {
                            Year = $"{period}Yrs",
                            TotalInvested = Math.Round(totalInvested, 0),
                            EstimatedReturns = Math.Round(estimatedReturns, 0),
                        });

                        previousYearCorpus = totalCorpus;
                        monthlyInvestment *= (1 + stepUpRate);
                    }
                }
                else
                {
                    for (int period = 1; period <= viewModel.InvestPeriod; period++)
                    {
                        double monthlyPeriod = period * 12;
                        double totalInvest = viewModel.IntialAmount * monthlyPeriod;
                        double value = Math.Pow((1 + rateofInterest), monthlyPeriod) - 1;
                        double estimatedReturns = (viewModel.IntialAmount * (value / rateofInterest) * (1 + rateofInterest)) - totalInvest;
                        data.Add(new Model() { Year = period.ToString() + "Yrs", TotalInvested = totalInvest, EstimatedReturns = Math.Round(estimatedReturns, 0) });
                    }
                }
            }
            else if (isLumpSum)
            {
                double rateofInterest = viewModel.ExpectedReturns / 100;
                if (isInvestAmount)
                {
                    for (int period = 1; period <= viewModel.InvestPeriod; period++)
                    {
                        double fvData = viewModel.IntialAmount * Math.Pow(1 + rateofInterest, period);
                        double totalGains = fvData - viewModel.IntialAmount;
                        data.Add(new Model() { Year = period.ToString() + "Yrs", TotalInvested = viewModel.IntialAmount, EstimatedReturns = Math.Round(totalGains, 0) });
                    }
                }
            }
            else if(isMutualFund)
            {
                if(isMutalFundMonthlySIP)
                {
                    double expectedReturnsValue = viewModel.ExpectedReturns / 100;
                    double rateofInterest = Math.Pow(1 + expectedReturnsValue, 1.0 / 12) - 1;
                    for (int period = 1; period <= viewModel.InvestPeriod; period++)
                    {
                        double monthlyPeriod = period * 12;
                        double totalInvest = viewModel.IntialAmount * monthlyPeriod;
                        double value = Math.Pow((1 + rateofInterest), monthlyPeriod) - 1;
                        double estimatedReturns = (viewModel.IntialAmount * (value / rateofInterest) * (1 + rateofInterest)) - totalInvest;
                        data.Add(new Model() { Year = period.ToString() + "Yrs", TotalInvested = totalInvest, EstimatedReturns = Math.Round(estimatedReturns, 0) });
                    }
                }
                else
                {
                    double rateofInterest = viewModel.ExpectedReturns / 100;
                    for (int period = 1; period <= viewModel.InvestPeriod; period++)
                    {
                        double fvData = viewModel.IntialAmount * Math.Pow(1 + rateofInterest, period);
                        double totalGains = fvData - viewModel.IntialAmount;
                        data.Add(new Model() { Year = period.ToString() + "Yrs", TotalInvested = viewModel.IntialAmount, EstimatedReturns = Math.Round(totalGains, 0) });
                    }
                }
            }
            viewModel.InvestmentData = data;
        }

        public void Dynamically_GetOverall_Investment_DataCollection()
        {
            var data = new ObservableCollection<Model>();
            if (isSIP)
            {
                int count = viewModel.InvestmentData.Count - 1;
                data.Add(new Model() { AmountName = "TotalInvested", Amount = viewModel.InvestmentData[count].TotalInvested });
                data.Add(new Model() { AmountName = "EstimatedReturns", Amount = viewModel.InvestmentData[count].EstimatedReturns });
            }
            else if (isLumpSum)
            {
                int count = viewModel.InvestmentData.Count - 1;
                data.Add(new Model() { AmountName = "TotalInvested", Amount = viewModel.InvestmentData[count].TotalInvested });
                data.Add(new Model() { AmountName = "Gains", Amount = viewModel.InvestmentData[count].EstimatedReturns });
            }
            else if (isMutualFund)
            {
                if (isMutalFundMonthlySIP)
                {
                    int count = viewModel.InvestmentData.Count - 1;
                    data.Add(new Model() { AmountName = "TotalInvested", Amount = viewModel.InvestmentData[count].TotalInvested });
                    data.Add(new Model() { AmountName = "EstimatedReturns", Amount = viewModel.InvestmentData[count].EstimatedReturns });
                }
                else
                {
                    int count = viewModel.InvestmentData.Count - 1;
                    data.Add(new Model() { AmountName = "TotalInvested", Amount = viewModel.InvestmentData[count].TotalInvested });
                    data.Add(new Model() { AmountName = "Gains", Amount = viewModel.InvestmentData[count].EstimatedReturns });
                }
            }
            viewModel.OverallInvestmentData = data;
        }

        public void Dynamically_Investment_GoalAmount_DataCollection()
        {
            var data = new ObservableCollection<Model>();
            if (isSIP)
            {
                double rateofInterest = (viewModel.ExpectedReturns / 12) / 100;
                double totalMonths = viewModel.InvestPeriod * 12;
                double value = Math.Pow((1 + rateofInterest), totalMonths) - 1;
                double denominatorFactor = (value / rateofInterest) * (1 + rateofInterest);
                double principle = viewModel.IntialAmount / denominatorFactor;
                double totalInvest = principle * totalMonths;
                double estimateReturns = viewModel.IntialAmount - totalInvest;
                data.Add(new Model() { AmountName = "TotalInvested", Amount = Math.Round(totalInvest, 0) });
                data.Add(new Model() { AmountName = "InterestEarned", Amount = Math.Round(estimateReturns, 0) });
                data.Add(new Model() { AmountName = "MonthlyInvest", Amount = Math.Round(principle, 0) });
            }
            else if (isLumpSum)
            {
                double rateofInterest = viewModel.ExpectedReturns / 100;
                double initialInvest = viewModel.IntialAmount / Math.Pow(1 + rateofInterest, viewModel.InvestPeriod);
                double totalInterest = viewModel.IntialAmount - initialInvest;
                data.Add(new Model() { AmountName = "TotalInvested", Amount = Math.Round(initialInvest, 0) });
                data.Add(new Model() { AmountName = "InterestEarned", Amount = Math.Round(totalInterest, 0) });
            }
            viewModel.OverallInvestmentData = data;
        }

        public void Dynamically_Inflation_Impacted_Investment_GoalAmount_DataCollection()
        {
            var data = new ObservableCollection<Model>();
            if (isSIP)
            {
                double rateofInterest = (viewModel.ExpectedReturns / 12) / 100;
                double adjustFV = viewModel.IntialAmount * Math.Pow(1 + 0.06, viewModel.InvestPeriod);
                double fvfactor = (Math.Pow(1 + rateofInterest, viewModel.InvestPeriod * 12) - 1) / rateofInterest;
                double monthlySIP = adjustFV / (fvfactor * (1 + rateofInterest));
                double adjusttotalInvest = monthlySIP * viewModel.InvestPeriod * 12;
                double estimatedReturns = adjustFV - adjusttotalInvest;
                data.Add(new Model() { AmountName = "FutureValue", Amount = Math.Round(adjustFV, 0) });
                data.Add(new Model() { AmountName = "TotalInvested", Amount = Math.Round(adjusttotalInvest, 0) });
                data.Add(new Model() { AmountName = "InterestEarned", Amount = Math.Round(estimatedReturns, 0) });
            }
            else if(isLumpSum)
            {
                double rateofInterest = viewModel.ExpectedReturns / 100;
                double adjustfvData = viewModel.IntialAmount * Math.Pow(1 + 0.06, viewModel.InvestPeriod);
                double adjustinitialInvest = adjustfvData / Math.Pow(1 + rateofInterest, viewModel.InvestPeriod);
                double adjusttotalInterest = adjustfvData - adjustinitialInvest;
                data.Add(new Model() { AmountName = "FutureValue", Amount = Math.Round(adjustfvData, 0) });
                data.Add(new Model() { AmountName = "TotalInvested", Amount = Math.Round(adjustinitialInvest, 0) });
                data.Add(new Model() { AmountName = "InterestEarned", Amount = Math.Round(adjusttotalInterest, 0) });
            }
            viewModel.InflationImpactedData = data;
        }

        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (viewModel != null && isInvestAmount || isStepUpInvest || isMutualFund)
            {
                string? input = e.NewTextValue?.Replace("₹", "").Trim();
                if (double.TryParse(input, out double value))
                {
                    viewModel.IntialAmount = value;
                }
                else
                {
                    viewModel.IntialAmount = 1;
                }
                Dynamically_Get_Investment_DataCollection();
                Dynamically_GetOverall_Investment_DataCollection();
            }
            if (viewModel != null && isGoalAmount)
            {
                string? input = e.NewTextValue?.Replace("₹", "").Trim();
                if (double.TryParse(input, out double value))
                {
                    viewModel.IntialAmount = value;
                }
                else
                {
                    viewModel.IntialAmount = 1;
                }
                Dynamically_Investment_GoalAmount_DataCollection();
                Dynamically_Inflation_Impacted_Investment_GoalAmount_DataCollection();
            }
        }

        private void Entry_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            if (viewModel != null && isInvestAmount || isStepUpInvest || isMutualFund)
            {
                string? input = e.NewTextValue?.Replace("Yrs", "").Trim();
                if (int.TryParse(input, out int value))
                {
                    viewModel.InvestPeriod = value;
                }
                else
                {
                    viewModel.InvestPeriod = 1;
                }
                Dynamically_Get_Investment_DataCollection();
                Dynamically_GetOverall_Investment_DataCollection();
            }
            if (viewModel != null && isGoalAmount)
            {
                string? input = e.NewTextValue?.Replace("Yrs", "").Trim();
                if (int.TryParse(input, out int value))
                {
                    viewModel.InvestPeriod = value;
                }
                else
                {
                    viewModel.InvestPeriod = 1;
                }
                Dynamically_Investment_GoalAmount_DataCollection();
                Dynamically_Inflation_Impacted_Investment_GoalAmount_DataCollection();
            }
        }

        private void Entry_TextChanged_2(object sender, TextChangedEventArgs e)
        {
            if (viewModel != null && isInvestAmount || isStepUpInvest || isMutualFund)
            {
                string? input = e.NewTextValue?.Replace("%", "").Trim();
                if (double.TryParse(input, out double value))
                {
                    viewModel.ExpectedReturns = value;
                }
                else
                {
                    viewModel.ExpectedReturns = 1;
                }
                Dynamically_Get_Investment_DataCollection();
                Dynamically_GetOverall_Investment_DataCollection();
            }
            if (viewModel != null && isGoalAmount)
            {
                string? input = e.NewTextValue?.Replace("%", "").Trim();
                if (double.TryParse(input, out double value))
                {
                    viewModel.ExpectedReturns = value;
                }
                else
                {
                    viewModel.ExpectedReturns = 1;
                }
                Dynamically_Investment_GoalAmount_DataCollection();
                Dynamically_Inflation_Impacted_Investment_GoalAmount_DataCollection();
            }
        }

        private void Entry_TextChanged_3(object sender, TextChangedEventArgs e)
        {
            if (viewModel != null && isStepUpInvest)
            {
                string? input = e.NewTextValue?.Replace("%", "").Trim();
                if (double.TryParse(input, out double value))
                {
                    viewModel.AnnualStepUp = value;
                }
                else
                {
                    viewModel.AnnualStepUp = 1;
                }
                Dynamically_Get_Investment_DataCollection();
                Dynamically_GetOverall_Investment_DataCollection();
            }
        }

        private void SfSegmentedControl_SelectionChanged(object sender, Syncfusion.Maui.Toolkit.SegmentedControl.SelectionChangedEventArgs e)
        {
            switch (e.NewIndex)
            {
                case 0:
                    isSIP = true;
                    isLumpSum = false;
                    isMutualFund = false;
                    radioButton3.IsVisible = true;
                    isMutalFundMonthlySIP = false;
                    radioButton1.Content = "Investment Amount";
                    radioButton2.Content = "Goal Amount";
                    radioButton1.IsChecked = true;
                    isInvestAmount = true;
                    isGoalAmount = false;
                    initialamountLabel.Text = "Montly Investment";
                    stackSeries2.Label = "EstimatedReturns";
                    Dynamically_Get_Investment_DataCollection();
                    Dynamically_GetOverall_Investment_DataCollection();
                    break;
                case 1:
                    isSIP = false;
                    isLumpSum = true;
                    isMutualFund = false;
                    radioButton3.IsVisible = false;
                    annualSetUpBox.IsVisible = false;
                    isMutalFundMonthlySIP = false;
                    radioButton1.Content = "Investment Amount";
                    radioButton2.Content = "Goal Amount";
                    radioButton1.IsChecked = true;
                    isInvestAmount = true;
                    isGoalAmount = false;
                    initialamountLabel.Text = "Investment Amount";
                    stackSeries2.Label = "Gains";
                    Dynamically_Get_Investment_DataCollection();
                    Dynamically_GetOverall_Investment_DataCollection();
                    break;
                case 2:
                    isSIP = false;
                    isLumpSum = false;
                    isMutualFund = true;
                    radioButton3.IsVisible = false;
                    annualSetUpBox.IsVisible = false;
                    radioButton1.Content = "Monthly SIP";
                    radioButton2.Content = "LumpSum";
                    radioButton1.IsChecked = true;
                    isMutalFundMonthlySIP = true;
                    isInvestAmount = false;
                    isGoalAmount = false;
                    initialamountLabel.Text = "Montly Investment";
                    stackSeries2.Label = "EstimatedReturns";
                    Dynamically_Get_Investment_DataCollection();
                    Dynamically_GetOverall_Investment_DataCollection();
                    break;
            }
        }

        private void radioButton1_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (viewModel != null && radioButton1.IsChecked && isSIP || isLumpSum)
            {
                initialamountLabel.Text = "Montly Investment";
                isInvestAmount = true;
                isGoalAmount = false;
                isStepUpInvest = false;
                cartesianChart1.IsVisible = true;
                cartesianChart2.IsVisible = false;
                annualSetUpBox.IsVisible = false;
                isMutalFundMonthlySIP = false;
                stackSeries2.Label = "EstimatedReturns";
                if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
                {
                    interactionLayout.Spacing = 50;
                }
                else if(DeviceInfo.Platform == DevicePlatform.MacCatalyst)
                {
                    interactionLayout.Spacing = 150;
                }
                else
                {
                    interactionLayout.Spacing = 100;
                }
                viewModel.IntialAmount = 5000;
                viewModel.InvestPeriod = 5;
                viewModel.ExpectedReturns = 12;
                if(isLumpSum)
                {
                    initialamountLabel.Text = "Investment Amount";
                }
            }
            if (viewModel != null && radioButton1.IsChecked && isMutualFund)
            {
                initialamountLabel.Text = "Montly Investment";
                isMutalFundMonthlySIP = true;
                stackSeries2.Label = "EstimatedReturns";
                cartesianChart1.IsVisible = true;
                cartesianChart2.IsVisible = false;
                viewModel.IntialAmount = 5000;
                viewModel.InvestPeriod = 5;
                viewModel.ExpectedReturns = 12;
            }
        }

        private void radioButton2_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (viewModel != null && radioButton2.IsChecked && isSIP || isLumpSum)
            {
                initialamountLabel.Text = "Goal Amount";
                isGoalAmount = true;
                isInvestAmount = false;
                isStepUpInvest = false;
                cartesianChart1.IsVisible = false;
                cartesianChart2.IsVisible = true;
                annualSetUpBox.IsVisible = false;
                isMutalFundMonthlySIP = false;
                if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
                {
                    interactionLayout.Spacing = 50;
                }
                else if (DeviceInfo.Platform == DevicePlatform.MacCatalyst)
                {
                    interactionLayout.Spacing = 150;
                }
                else
                {
                    interactionLayout.Spacing = 100;
                }    
                viewModel.IntialAmount = 1000000;
                viewModel.InvestPeriod = 5;
                viewModel.ExpectedReturns = 12;
            }
            if(viewModel != null && radioButton2.IsChecked && isMutualFund)
            {
                initialamountLabel.Text = "Investment Amount";
                isMutalFundMonthlySIP = false;
                cartesianChart1.IsVisible = true;
                cartesianChart2.IsVisible = false;
                stackSeries2.Label = "Gains";
                viewModel.IntialAmount = 1000000;
                viewModel.InvestPeriod = 5;
                viewModel.ExpectedReturns = 12;
            }
        }

        private void radioButton3_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (viewModel != null && radioButton3.IsChecked)
            {
                initialamountLabel.Text = "Monthly Investment";
                isStepUpInvest = true;
                isInvestAmount = false;
                isGoalAmount = false;
                isMutalFundMonthlySIP = false;
                cartesianChart1.IsVisible = true;
                cartesianChart2.IsVisible = false;
                annualSetUpBox.IsVisible = true;
                if(DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
                {
                    interactionLayout.Spacing = 25;
                }
                else if (DeviceInfo.Platform == DevicePlatform.MacCatalyst)
                {
                    interactionLayout.Spacing = 150;
                }
                else
                {
                    interactionLayout.Spacing = 50;
                }    
                viewModel.IntialAmount = 10000;
                viewModel.InvestPeriod = 5;
                viewModel.ExpectedReturns = 12;
                viewModel.AnnualStepUp = 10;
            }
        }

        private void NumericalAxis_LabelCreated(object sender, ChartAxisLabelEventArgs e)
        {
            double value = Convert.ToDouble(e.Label);
            if (value >= 1000000000)
            {
                e.Label = $"{value / 1000000000:0.#}B";
            }
            else if (value >= 1000000)
            {
                e.Label = $"{value / 1000000:0.#}M";
            }
            else if (value >= 1000)
            {
                e.Label = $"{value / 1000:0.#}K";
            }
            else
            {
                e.Label = value.ToString("0.#");
            }
        }
    }

    public class SegmentColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string stage)
            {
                return stage switch
                {
                    "TotalInvested" => Color.FromArgb("#F4A300"),
                    "EstimatedReturns" or "Gains" or "InterestEarned" => Color.FromArgb("#1F77B4"),
                    "TotalAmount" or "MonthlyInvest" or "FutureValue" => Color.FromArgb("#27AE60"),
                    _ => Colors.Gray
                };
            }
            return Colors.Gray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class SegmentColorConverter1 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string stage)
            {
                return stage switch
                {
                    "TotalInvested" => Color.FromArgb("#F4A300"),
                    "EstimatedReturns" or "InterestEarned" => Color.FromArgb("#1F77B4"),
                    "FutureValue"=> Color.FromArgb("#FF5F5D"),
                    _ => Colors.Gray
                };
            }
            return Colors.Gray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class NumberFormatterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double number)
            {
                return FormatNumber(number);
            }
            if (value is int intValue)
            {
                return FormatNumber(intValue);
            }
            return value;
        }

        private string FormatNumber(double num)
        {
            if (num >= 1000000)
                return $"₹ {num / 1_000_000:0.#}M";
            else if (num >= 1000)
                return $"₹ {num / 1000:0.#}K";
            else
                return $"₹ {num:0}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

}
