using System;
using System.Collections.Generic;
using System.Text;

namespace microsoft.resource.api.common
{
    public class ApiEvent
    {

        public Guid ServiceInstanceUnique { get; set; }
        public String AppId { get; set; }
        public String Source { get; set; }
        public EventType Type { get; set; }

        public Guid Id { get; set; }
        public string UserLoginName { get; set; }
        public string Machine { get; set; }
        public string Message { get; set; }
        public DateTime LogDate { get; set; }
    }

    /// <summary>
    /// Tipo de evento (Event).
    /// </summary>
    /// <date>2006/09/02</date>
    /// <author>moviedo</author>
    public enum EventType
    {

        /// <summary>
        /// Representa mensajes de información.
        /// </summary>
        Information,
        /// <summary>
        /// Representa mensajes de advertencia.
        /// </summary>
        Warning,
        /// <summary>
        /// Representa mensajes de error.
        /// </summary>
        Error,
        /// <summary>
        /// Representa la ausencia de tipo de evento.
        /// </summary>
        None,
        /// <summary>
        /// Representa la ausencia de tipo de evento.
        /// </summary>
        Audit
    }
}
