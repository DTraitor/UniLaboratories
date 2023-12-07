namespace Delegates;

public class Account
{
    public delegate void OnAccountEmpty();
    public event OnAccountEmpty Notify;

    public int Sum { get; private set; }
    public int HoursTalked { get; private set; } = 0;
    public int TariffCost { get; private set; }

    public Account(int sum, int tariffCost)
    {
        Sum = sum;
        TariffCost = tariffCost;
    }

    public void PayTariff()
    {
        Take(TariffCost);
    }

    public void Put(int sum)
    {
        Sum += sum;
    }

    public void Take(int sum)
    {
        if (Sum >= sum)
            Sum -= sum;
        if (Sum == 0)
            Notify?.Invoke();
    }

    public void Talk(int hours)
    {
        HoursTalked += hours;
    }
}