using FluentResults;

namespace BankApi;

public class MyBank
{
    private readonly List<User> _users = [];
    
    public Result<Guid> RegisterUser(string username, string password)
    {
        var existUsername = _users.FirstOrDefault(x => x.Username == username);

        if (existUsername != null)
            return Result.Fail("Username already exists");

        var createdUserResult = User.CreateUser(username, password);

        if (createdUserResult.IsFailed)
            return Result.Fail(createdUserResult.Errors);
        
        _users.Add(createdUserResult.Value);
        
        return Result.Ok(createdUserResult.Value.Id);
    }

    public Result IncreaseBalance(Guid userId, decimal amount)
    {
        var user = _users.FirstOrDefault(x => x.Id == userId);
        
        if (user == null)
            return Result.Fail("User not found");
        
        return user.IncreaseBalance(amount);
    }
    
    public Result DecreaseBalance(Guid userId, decimal amount)
    {
        var user = _users.FirstOrDefault(x => x.Id == userId);
        
        if (user == null)
            return Result.Fail("User not found");
        
        return user.DecreaseBalance(amount);
    }
    
    public Result<decimal> GetBalance(Guid userId)
    {
        var user = _users.FirstOrDefault(x => x.Id == userId);
        
        if (user == null)
            return Result.Fail("User not found");
        
        return Result.Ok(user.Balance);
    }

    public Result BlockAccount(Guid userId)
    {
        var user = _users.FirstOrDefault(x => x.Id == userId);
        
        if (user == null)
            return Result.Fail("User not found");
        
        user.Block();
        return Result.Ok();
    }

    public Result<bool> IsBlocked(Guid userId)
    {
        var user = _users.FirstOrDefault(x => x.Id == userId);
        
        if (user == null)
            return Result.Fail("User not found");
        
        return user.IsBlocked;
    }
}