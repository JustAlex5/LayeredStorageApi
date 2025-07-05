using System.Text.Json.Serialization;

namespace Project.Common.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum UserRoleEnum
    {
        Admin = 1,
        User = 2
    }
}
