using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SIPCalculator
{
    public class ViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Model> sipInvestmentData;
        public ObservableCollection<Model> SIPInvestmentData
        {
            get => sipInvestmentData;
            set
            {
                if(sipInvestmentData != value)
                {
                    sipInvestmentData = value;
                    OnPropertyChanged(nameof(SIPInvestmentData));
                }
            }
        }

        private ObservableCollection<Model> sipOverallData;
        public ObservableCollection <Model> SIPOverallData
        {
            get => sipOverallData;
            set
            {
                if(sipOverallData != value)
                {
                    sipOverallData = value;
                    OnPropertyChanged(nameof(SIPOverallData));
                }
            }
        }

        private ObservableCollection<Model> adjustedData;
        public ObservableCollection<Model> AdjustedData
        {
            get => adjustedData;
            set
            {
                if (adjustedData != value)
                {
                    adjustedData = value;
                    OnPropertyChanged(nameof(AdjustedData));
                }
            }
        }

        private double intialAmount;
        public double IntialAmount
        {
            get => intialAmount;
            set
            {
                if(intialAmount != value)
                {
                    intialAmount = value;
                    OnPropertyChanged(nameof(IntialAmount));
                }
            }
        }

        private int investPeriod;
        public int InvestPeriod
        {
            get => investPeriod;

            set
            {
                if(investPeriod != value)
                {
                    investPeriod = value;
                    OnPropertyChanged(nameof(InvestPeriod));
                }
            }
        }

        private double expectedReturns;
        public double ExpectedReturns
        {
            get => expectedReturns;
            set
            {
                if(expectedReturns != value)
                {
                    expectedReturns = value;
                    OnPropertyChanged(nameof(ExpectedReturns));
                }
            }
        }

        private double annualStepUp;
        public double AnnualStepUp
        {
            get => annualStepUp;
            set
            {
                if (annualStepUp != value)
                {
                    annualStepUp = value;
                    OnPropertyChanged(nameof(AnnualStepUp));
                }
            }
        }

        public List<Brush> CustomBrushes { get; set; }

        public List<Brush> CustomBrushes1 { get; set; }

        public ViewModel()
        {
            IntialAmount = 5000;
            InvestPeriod = 5;
            ExpectedReturns = 12;

            SIPInvestmentData = GetSIPDataCollection();

            CustomBrushes = new List<Brush>()
            {
                 new SolidColorBrush(Color.FromArgb("#FF5F5D")),
                 new SolidColorBrush(Color.FromArgb("#F4A300")),
                 new SolidColorBrush(Color.FromArgb("#2980B9")),
            };

            CustomBrushes1 = new List<Brush>()
            {
                new SolidColorBrush(Color.FromArgb("#F4A300")),
                new SolidColorBrush(Color.FromArgb("#1F77B4")),
                new SolidColorBrush(Color.FromArgb("#27AE60")),
            };

            SIPOverallData = GetOverallSIPDataCollection();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public ObservableCollection<Model> GetSIPDataCollection()
        {
            var data = new ObservableCollection<Model>();

            double expectedReturnsValue = ExpectedReturns / 100;
            double powerValue = 1.0/12;
            double rateofInterest = Math.Pow((1 + expectedReturnsValue), powerValue) - 1;

            for (int period = 1; period <= InvestPeriod; period++)
            {
                double monthlyPeriod = period * 12;
                double totalInvest = IntialAmount * monthlyPeriod;
                double value = Math.Pow((1 + rateofInterest), monthlyPeriod) - 1;
                double estimatedReturns = (IntialAmount * ( value / rateofInterest) * (1 + rateofInterest))-totalInvest;
                data.Add(new Model() { Year = period.ToString() + "Yrs", TotalInvested = totalInvest, EstimatedReturns = Math.Round(estimatedReturns, 0) });
            }

            return data;
        }

        public ObservableCollection<Model> GetOverallSIPDataCollection()
        {
            var data = new ObservableCollection<Model>();
            int count = SIPInvestmentData.Count - 1;
            data.Add(new Model() { AmountName = "TotalInvested", Amount = SIPInvestmentData[count].TotalInvested });
            data.Add(new Model() { AmountName = "EstimatedReturns", Amount = SIPInvestmentData[count].EstimatedReturns });
            data.Add(new Model() { AmountName = "TotalAmount", Amount = SIPInvestmentData[count].TotalInvested + SIPInvestmentData[count].EstimatedReturns });
            return data;
        }
    }
}
