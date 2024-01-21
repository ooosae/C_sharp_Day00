static double CalculateMonthlyPayment(double loanAmount, double annualInterestRate, int numberOfMonths)
{
    double monthlyRate = annualInterestRate / 12 / 100;
    return loanAmount * (monthlyRate * Math.Pow(1 + monthlyRate, numberOfMonths)) /
           (Math.Pow(1 + monthlyRate, numberOfMonths) - 1);
}


static double CalculateInterestPayment(double remainingLoanAmount, double annualInterestRate, int daysInPeriod, int daysInYear)
{
    double monthlyRate = annualInterestRate / 12 / 100;
    return remainingLoanAmount * monthlyRate * daysInPeriod / daysInYear;
}

if (args.Length != 3)  {
    Console.WriteLine("Something went wrong. Check your input and retry.");
    return 1;
}

if (!double.TryParse(args[0], out double loanAmount) || loanAmount <= 0)  {
    Console.WriteLine("Something went wrong. Check your input and retry.");
    return 1;
}

if (!double.TryParse(args[1], out double annualInterestRate) || annualInterestRate <= 0)  {
    Console.WriteLine("Something went wrong. Check your input and retry.");
    return 1;
}

if (!int.TryParse(args[2], out int loanTerm) || loanTerm <= 0)  {
    Console.WriteLine("Something went wrong. Check your input and retry.");
    return 1;
}

DateTime currentDate = DateTime.Now;
DateTime previousPaymentDate = currentDate.AddMonths(-1).AddDays(-currentDate.Day + 1);
double monthlyPayment = CalculateMonthlyPayment(loanAmount, annualInterestRate, loanTerm);

Console.WriteLine($"{"Payment no.",-15}{"Payment date",-20}{"Payment",-20}{"Principal debt",-20}{"Interest",-20}{"Remaining debt",-20}");

for (int i = 0; i < loanTerm; i++)
{
    int daysInMonth = DateTime.DaysInMonth(currentDate.Year, currentDate.Month);
    int daysInPeriod = (int)(currentDate - previousPaymentDate).TotalDays;
    double interestPayment = CalculateInterestPayment(loanAmount, annualInterestRate, daysInPeriod, daysInMonth);
    double principalPayment = monthlyPayment - interestPayment;
    
    if (i == loanTerm - 1)
    {
        monthlyPayment = loanAmount + interestPayment;
        principalPayment = loanAmount;
        loanAmount = 0;
    }
    else
    {
        if (loanAmount - principalPayment < 0)
        {
            principalPayment = loanAmount;
            monthlyPayment = principalPayment + interestPayment;
        }

        loanAmount -= principalPayment;
    }

    Console.WriteLine($"{i + 1,-15}{currentDate,-20:MM/dd/yyyy}{monthlyPayment,-20:C}{principalPayment,-20:C}{interestPayment,-20:C}{loanAmount,-20:C}");

    previousPaymentDate = currentDate;
    currentDate = currentDate.AddMonths(1);
}

return 0;