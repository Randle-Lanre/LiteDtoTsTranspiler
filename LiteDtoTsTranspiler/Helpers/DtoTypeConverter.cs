namespace LiteDtoTsTranspiler.Helpers;

public class DtoTypeConverter
{
    public string ConvertCsTypeToTsType(string propType) => propType switch
    {
        "System.Int32" => "number",
        "System.String" => "string",
        "System.DateTime" => "Date",
        "System.Double" => "number",
        "System.Int64" => "bigint",
        "System.Boolean" => "boolean",
        _ => "any"
    };
}