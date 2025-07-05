using Forms.Demo.Models;

namespace Forms.Demo.Forms;

public record Form(
    string Name,
    string Version,
    Address Address,
    Dictionary<string, FormValue> Fields
);