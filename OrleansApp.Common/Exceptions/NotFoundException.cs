using System;
using System.Runtime.Serialization;

namespace OrleansApp.Common.Exceptions
{
    /// <summary>
    /// Wyjątek rzucany, gdy żądany zasób nie został znaleziony
    /// </summary>
    [Serializable]
    public class NotFoundException : Exception
    {
        /// <summary>
        /// Inicjalizuje nową instancję klasy <see cref="NotFoundException"/> bez wiadomości błędu
        /// </summary>
        public NotFoundException()
            : base("Żądany zasób nie został znaleziony.")
        {
        }

        /// <summary>
        /// Inicjalizuje nową instancję klasy <see cref="NotFoundException"/> z określoną wiadomością błędu
        /// </summary>
        /// <param name="message">Komunikat błędu wyjaśniający przyczynę wyjątku</param>
        public NotFoundException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Inicjalizuje nową instancję klasy <see cref="NotFoundException"/> z określoną wiadomością błędu
        /// i referencją do wewnętrznego wyjątku, który jest przyczyną tego wyjątku
        /// </summary>
        /// <param name="message">Komunikat błędu wyjaśniający przyczynę wyjątku</param>
        /// <param name="innerException">Wyjątek, który jest przyczyną obecnego wyjątku</param>
        public NotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Inicjalizuje nową instancję klasy <see cref="NotFoundException"/> z typem zasobu i identyfikatorem
        /// </summary>
        /// <param name="resourceType">Typ zasobu, który nie został znaleziony</param>
        /// <param name="id">Identyfikator zasobu, który nie został znaleziony</param>
        public NotFoundException(string resourceType, object id)
            : base($"Zasób typu {resourceType} o identyfikatorze '{id}' nie został znaleziony.")
        {
            ResourceType = resourceType;
            ResourceId = id;
        }

        /// <summary>
        /// Konstruktor deserializacji
        /// </summary>
        protected NotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            ResourceType = info.GetString(nameof(ResourceType));
            ResourceId = info.GetValue(nameof(ResourceId), typeof(object));
        }

        /// <summary>
        /// Typ zasobu, który nie został znaleziony
        /// </summary>
        public string? ResourceType { get; }

        /// <summary>
        /// Identyfikator zasobu, który nie został znaleziony
        /// </summary>
        public object? ResourceId { get; }

        /// <summary>
        /// Nadpisuje GetObjectData w celu serializacji dodatkowych właściwości
        /// </summary>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            
            if (ResourceType != null)
            {
                info.AddValue(nameof(ResourceType), ResourceType);
            }
            
            if (ResourceId != null)
            {
                info.AddValue(nameof(ResourceId), ResourceId);
            }
        }
    }
}