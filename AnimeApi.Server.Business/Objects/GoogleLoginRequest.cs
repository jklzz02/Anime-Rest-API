using Newtonsoft.Json;

namespace AnimeApi.Server.Business.Objects;

public class GoogleLoginRequest
{
    [JsonProperty("id_token")]
    public string IdToken { get; set; } = string.Empty;
}