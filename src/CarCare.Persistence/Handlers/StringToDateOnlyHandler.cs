using Dapper;
using System.Data;

namespace CarCare.Persistence.Handlers;

internal class StringToDateOnlyHandler : SqlMapper.TypeHandler<DateOnly>
{
    public override DateOnly Parse(object value)
    {
        return value switch
        {
            string valueString => DateOnly.Parse(valueString),
            _ => throw new ArgumentException("The provided value must be a string", nameof(value))
        };
    }

    public override void SetValue(IDbDataParameter parameter, DateOnly value)
    {
        parameter.Value = value.ToString("yyyy-MM-dd");
    }
}
