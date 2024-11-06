using System;
using System.Collections.Generic;

namespace Test.Models;

public partial class Vehicle
{
    public int Id { get; set; }

    public string Model { get; set; } = null!;

    public string Make { get; set; } = null!;

    public int Year { get; set; }
}
