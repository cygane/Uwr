# Interface Segregation Principle
Klient nie powinien być zmuszany do zależności od metod których nie używa

# Przykład IUserStore<TUser>

```csharp
public interface IUserStore<TUser> : 
    IUserPasswordStore<TUser>, 
    IUserRoleStore<TUser>, 
    IUserClaimStore<TUser>, 
    IUserLoginStore<TUser>, 
    IUserEmailStore<TUser>, 
    IUserPhoneNumberStore<TUser>, 
    IUserTwoFactorStore<TUser>, 
    IUserLockoutStore<TUser>
{
    Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken);
    Task<IdentityResult> UpdateAsync(TUser user, CancellationToken cancellationToken);
    Task<IdentityResult> DeleteAsync(TUser user, CancellationToken cancellationToken);
    Task<TUser> FindByIdAsync(string userId, CancellationToken cancellationToken);
    Task<TUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken);
    Task<string> GetNormalizedUserNameAsync(TUser user, CancellationToken cancellationToken);
    Task<string> GetUserIdAsync(TUser user, CancellationToken cancellationToken);
    Task<string> GetUserNameAsync(TUser user, CancellationToken cancellationToken);
    Task SetNormalizedUserNameAsync(TUser user, string normalizedUserName, CancellationToken cancellationToken);
    Task SetUserNameAsync(TUser user, string userName, CancellationToken cancellationToken);
}
```
problem:
jeśli chcemy implementować przechowywanie tylko prostych użytkowników, którzy nie potrzebują takich funkcji jak dwuskładnikowe uwierzytelnianie, role czy numery telefonów, będziemy zmuszeni zaimplementować niepotrzebne metody

# Refaktoryzacja
```csharp
public interface IUserCreationStore<TUser>
{
    Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken);
    Task<IdentityResult> UpdateAsync(TUser user, CancellationToken cancellationToken);
    Task<IdentityResult> DeleteAsync(TUser user, CancellationToken cancellationToken);
}

public interface IUserIdentificationStore<TUser>
{
    Task<TUser> FindByIdAsync(string userId, CancellationToken cancellationToken);
    Task<TUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken);
    Task<string> GetUserIdAsync(TUser user, CancellationToken cancellationToken);
    Task<string> GetUserNameAsync(TUser user, CancellationToken cancellationToken);
    Task SetUserNameAsync(TUser user, string userName, CancellationToken cancellationToken);
}

public interface IUserPasswordStore<TUser>
{
    Task SetPasswordHashAsync(TUser user, string passwordHash, CancellationToken cancellationToken);
    Task<string> GetPasswordHashAsync(TUser user, CancellationToken cancellationToken);
    Task<bool> HasPasswordAsync(TUser user, CancellationToken cancellationToken);
}

public interface IUserEmailStore<TUser>
{
    Task SetEmailAsync(TUser user, string email, CancellationToken cancellationToken);
    Task<string> GetEmailAsync(TUser user, CancellationToken cancellationToken);
    Task<bool> HasEmailAsync(TUser user, CancellationToken cancellationToken);
}

public interface IUserRoleStore<TUser>
{
    Task AddToRoleAsync(TUser user, string roleName, CancellationToken cancellationToken);
    Task RemoveFromRoleAsync(TUser user, string roleName, CancellationToken cancellationToken);
    Task<IList<string>> GetRolesAsync(TUser user, CancellationToken cancellationToken);
}

```