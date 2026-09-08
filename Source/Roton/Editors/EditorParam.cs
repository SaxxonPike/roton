using System.Reflection;
using Roton.Emulation.Data;

namespace Roton.Editors;

public sealed class EditorParam(
    string paramName,
    string propertyName,
    EditorParamType type = EditorParamType.Value)
{
    public string Name { get; } = paramName;
    public PropertyInfo Property { get; } = typeof(IActor).GetProperty(propertyName)!;
    public EditorParamType Type { get; } = type;
}