using System.Collections.ObjectModel;
using System.ComponentModel;
namespace SIPCalculator
{
    public class ViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Model>? investmentData;
        public ObservableCollection<Model>? InvestmentData
        {
            get => investmentData;
            set
            {
                if(investmentData != value)
                {
                    investmentData = value;
                    OnPropertyChanged(nameof(InvestmentData));
                }
            }
        }

        private ObservableCollection<Model>? overallInvestmentData;
        public ObservableCollection<Model>? OverallInvestmentData
        {
            get => overallInvestmentData;
            set
            {
                if(overallInvestmentData != value)
                {
                    overallInvestmentData = value;
                    OnPropertyChanged(nameof(OverallInvestmentData));
                }
            }
        }

        private ObservableCollection<Model>? inflationImpactedData;
        public ObservableCollection<Model>? InflationImpactedData
        {
            get => inflationImpactedData;
            set
            {
                if (inflationImpactedData != value)
                {
                    inflationImpactedData = value;
                    OnPropertyChanged(nameof(InflationImpactedData));
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

            InvestmentData = Get_Investment_DataCollection();

            CustomBrushes = new List<Brush>()
            {
                new SolidColorBrush(Color.FromArgb("#34495E")),  
                new SolidColorBrush(Color.FromArgb("#16A085")),
                new SolidColorBrush(Color.FromArgb("#20B2AA"))
            };

            CustomBrushes1 = new List<Brush>()
            {
                new SolidColorBrush(Color.FromArgb("#6B5B95")),
                new SolidColorBrush(Color.FromArgb("#D3A6D3")),
                new SolidColorBrush(Color.FromArgb("#F4A261")),
            };

            OverallInvestmentData = GetOverall_Investment_DataCollection();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public ObservableCollection<Model> Get_Investment_DataCollection()
        {
            //Future Value = P * ({[1 + r] ^ n – 1} / r) * (1 + r)
            //Where:
            //P - Principal contributions each month
            //r - expected rate of return (per month)
            //n - Number of contributions towards the principal

            //Total Invest = P * n
            //Where:
            //P - Principal contributions each month
            //n - Number of contributions towards the principal

            //Estimated Returns = FV - Total Invest

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

        public ObservableCollection<Model> GetOverall_Investment_DataCollection()
        {
            var data = new ObservableCollection<Model>();
            int count = InvestmentData!.Count - 1;
            data.Add(new Model() { AmountName = "TotalInvested", Amount = InvestmentData[count].TotalInvested });
            data.Add(new Model() { AmountName = "EstimatedReturns", Amount = InvestmentData[count].EstimatedReturns });
            return data;
        }
    }
}
