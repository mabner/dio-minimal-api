using System;
using System.Collections.Generic;

namespace Test.Models;

public partial class Administrator
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Profile { get; set; } = null!;
}
