using System.Globalization;

namespace Forms.Demo.Models;

public record Address(string Street, int Number, string ZipCode, CultureInfo Country);