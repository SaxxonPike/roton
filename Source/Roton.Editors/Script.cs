using System.ComponentModel.DataAnnotations;

namespace Roton.Editors;

public class Script()
{
    public Script(string code) : this()
    {
        Code = code;
    }

    [MaxLength(short.MaxValue)]
    public string Code { get; set; } = string.Empty;
}