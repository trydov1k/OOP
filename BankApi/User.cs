using FluentResults;

namespace BankApi;

public class User
{
    public Guid Id { get; private set; }
    public string Username { get; private set; }
    public string Password { get; private set; }
    public decimal Balance { get; private set; }
    public bool IsBlocked { get; private set; }

    private User(Guid id, string username, string password, decimal balance, bool isBlocked)
    {
        Id = id;
        Username = username;
        Password = password;
    }

    public static Result<User> CreateUser(string username, string password)
    {
        var newUser = new User(Guid.NewGuid(), username, password, 0, false);
        return Result.Ok(newUser);
    }
    
    public Result IncreaseBalance(decimal amount)
    {
        if (IsBlocked)
            return Result.Fail("User is blocked");
        
        if (amount <= 0)
            return Result.Fail("Amount must be greater than zero");
        
        Balance += amount;
        
        return Result.Ok();
    }

    public Result DecreaseBalance(decimal amount)
    {
        if (IsBlocked)
            return Result.Fail("User is blocked");
        
        if (amount <= 0)
            return Result.Fail("Amount must be greater than zero");
        
        if (Balance - amount < 0)
            return Result.Fail("Insufficient funds");
        
        Balance -= amount;
        
        return Result.Ok();
    }

    public void Block()
    {
        IsBlocked = true;
    }
    
    public void Unblock()
    {
        IsBlocked = false;
    }
}