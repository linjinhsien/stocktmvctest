using System;
using System.Collections.Generic;

namespace WebApplication2.Models;

public partial class Stockjson
{
    public short Code { get; set; }

    public string Name { get; set; } = null!;

    public double? Peratio { get; set; }

    public double? DividendYield { get; set; }

    public double? Pbratio { get; set; }
}
