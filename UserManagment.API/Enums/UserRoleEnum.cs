using System.Text.Json.Serialization;

namespace UserManagment.API.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum UserRoleEnum
    {
        Admin = 1,
        User = 2
    }
}
