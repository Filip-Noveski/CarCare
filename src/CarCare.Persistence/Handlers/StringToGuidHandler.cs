using Dapper;
using System.Data;

namespace CarCare.Persistence.Handlers;

internal class StringToGuidHandler : SqlMapper.TypeHandler<Guid>
{
    public override Guid Parse(object value)
    {
        return value switch
        {
            string valueString => Guid.Parse(valueString),
            _ => throw new ArgumentException("The provided value must be a string")
        };
    }

    public override void SetValue(IDbDataParameter parameter, Guid value)
    {
        parameter.Value = value.ToString();
    }
}
