namespace LiteDtoTsTranspiler.Helpers;

public class DtoTypeConverter
{
    public string ConvertCsTypeToTsType(string propType) => propType switch
    {
        "System.Int32" or "System.Double" or "System.UInt64" or "System.UInt16" or "System.UInt32"
            or "System.Int16" => "number",
        "System.String" => "string",
        "System.DateTime" => "Date",
        "System.Int64" => "bigint",
        "System.Boolean" => "boolean",
        "System.Object" => "any",
        "System.Enum" => "string",
        _ => "any"
    };
}