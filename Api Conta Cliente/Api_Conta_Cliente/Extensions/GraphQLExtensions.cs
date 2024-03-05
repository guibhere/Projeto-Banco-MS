using GraphQL;
using Newtonsoft.Json.Linq;

public static class GraphQLExtensions
{
    public static Inputs ToInputs(this JObject variables)
    {
        if (variables == null) return null;

        var dictionary = new Dictionary<string, object>();
        foreach (var property in variables)
        {
            var value = CoerceValue(property.Value);
            dictionary.Add(property.Key, value);
        }

        return new Inputs(dictionary);
    }
    private static object CoerceValue(JToken value)
    {
        switch (value.Type)
        {
            case JTokenType.Object:
                var obj = value as JObject;
                var dict = new Dictionary<string, object>();
                foreach (var property in obj)
                {
                    dict.Add(property.Key, CoerceValue(property.Value));
                }
                return dict;

            case JTokenType.Array:
                var array = value as JArray;
                var list = new List<object>();
                foreach (var item in array)
                {
                    list.Add(CoerceValue(item));
                }
                return list;

            case JTokenType.Integer:
                return (int)value;

            case JTokenType.Float:
                return (double)value;

            case JTokenType.Boolean:
                return (bool)value;

            case JTokenType.String:
                return (string)value;

            default:
                return null;
        }

    }
}