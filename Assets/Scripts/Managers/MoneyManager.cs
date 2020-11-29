using UnityEngine;

public class MoneyManager : Singleton<MoneyManager> 
{
    [SerializeField] int totalMoney = 45;

    public int TotalMoney => totalMoney;

    /// <summary>
    /// Adding money to total amount
    /// </summary>
    /// <param name="amount">how much to add</param>
    public void AddMoney(int amount)
    {
        totalMoney += amount;
    }

    /// <summary>
    /// Subtract money from total amount
    /// </summary>
    /// <param name="amount">how much to subtract</param>
    public void SubtractMoney(int amount)
    {
        totalMoney -= amount;
    }

    /// <summary>
    /// Can user afford this operation
    /// </summary>
    /// <param name="price">Price</param>
    /// <returns>Is affordable</returns>
    public bool IsOperationAffordable(int price) => totalMoney >= price;
}