namespace Incapsulation.EnterpriseTask;

public class Enterprise(Guid guid)
{
	public readonly Guid Guid = guid;
	public string Name { get; set; } = string.Empty;
	private string inn = string.Empty;
	public string Inn
	{
		get => inn;
		set
		{
			if (value.Length != 10 || !value.All(z => char.IsDigit(z)))
				throw new ArgumentException();
			inn = value;
		}
	}
	public DateTime EstablishDate { get; set; }
	public TimeSpan ActiveTimeSpan => DateTime.Now - EstablishDate;

	public double GetTotalTransactionsAmount()
	{
		DataBase.OpenConnection();
		var amount = 0.0;
		foreach (Transaction t in DataBase.Transactions().Where(z => z.EnterpriseGuid == this.Guid))
			amount += t.Amount;
		return amount;
	}
}