// Configure JSON options
using System.Globalization;
using System.Text.Json;

using Forms.Demo.Forms;
using Forms.Demo.Json;
using Forms.Demo.Models;

#pragma warning disable CA1869 // Cache and reuse 'JsonSerializerOptions' instances
var options = new JsonSerializerOptions
{
    WriteIndented = true,
    Converters = { new FormValueConverter() }
};
#pragma warning restore CA1869 // Cache and reuse 'JsonSerializerOptions' instances

// Create form data functionally
var form = new Form(
    Name: "User Registration",
    Version: "1.0.0",
    Address: new Address("Min vej", 24, "8210", CultureInfo.CurrentCulture),
    Fields: new Dictionary<string, FormValue>
    {
        ["firstName"] = "John",
        ["age"] = 30.ToFormValue(),
        ["isActive"] = true.ToFormValue(),
        ["birthDate"] = DateTime.Now.AddYears(-30).ToFormValue(),
        ["hobbies"] = new[] { "reading", "coding", "gaming" }.ToFormValue()
    }
);

// Serialize to JSON
string json = JsonSerializer.Serialize(form, options);
Console.WriteLine("Serialized JSON:");
Console.WriteLine(json);

