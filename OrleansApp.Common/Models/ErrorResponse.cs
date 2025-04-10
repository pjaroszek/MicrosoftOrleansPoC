using System.Text.Json.Serialization;

namespace OrleansApp.Common.Models;

/// <summary>
/// Reprezentuje standardową odpowiedź błędu wysyłaną przez API
/// </summary>
public class ErrorResponse
{
    /// <summary>
    /// Kod statusu HTTP odpowiedzi
    /// </summary>
    public int StatusCode { get; set; }

    /// <summary>
    /// Komunikat błędu
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Unikalne ID błędu do śledzenia w logach
    /// </summary>
    public string ErrorId { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Znacznik czasu wystąpienia błędu
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Dodatkowe szczegóły błędu, opcjonalne
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<string, object>? Details { get; set; }

    /// <summary>
    /// Ścieżka żądania, które spowodowało błąd
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Path { get; set; }

    /// <summary>
    /// Dodaj szczegóły błędu jako parę klucz-wartość
    /// </summary>
    public void AddDetail(string key, object value)
    {
        Details ??= new Dictionary<string, object>();
        Details[key] = value;
    }

    /// <summary>
    /// Tworzy podstawowy obiekt ErrorResponse
    /// </summary>
    public static ErrorResponse Create(int statusCode, string message)
    {
        return new ErrorResponse
        {
            StatusCode = statusCode,
            Message = message
        };
    }
}