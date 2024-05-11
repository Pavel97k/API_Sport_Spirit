namespace API_Sport_Spirit.Model;

public partial class Administrator
{
    public int IdAdministrator { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;
}
