public class MoneyManager : Singleton<MoneyManager> 
{
    int _totalMoney = 45; // текущее колличесво денег

    public int TotalMoney 
    {
        get => _totalMoney;
        set => _totalMoney = value >= 0 ? value : 0;
    }

    public void AddMoney(int amount)
    {
        TotalMoney += amount; // добавляем деньги
    }

    public void SubtractMoney(int amount)
    {
        TotalMoney -= amount; // вычитаем деньги
    }

    public bool IsOperationAffordable(int price) => _totalMoney >= price;
}