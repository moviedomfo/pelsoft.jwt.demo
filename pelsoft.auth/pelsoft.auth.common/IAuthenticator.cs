using pelsoft.auth.models;

namespace pelsoft.auth.common
{
    public interface IAuthenticator
    {
        /// <summary>
        /// Identifica un usuario según las credenciales provistas en <c>req.AdditionalParameters</c>req>
        /// </summary>
        /// <param name="req">La solicitud recibida por el Controller. Los datos de identificación se almacenan en AdditionalData</param>
        /// <returns>Un par conteniendo el ID del usuario para identificarlo en FetchUser, y la lista de claims generados.</returns>
        public UserEntities Authenticate(AuthenticationRequest req);

        /// <summary>
        /// Obtiene los datos asociados al usuario identificado en el refresh_token, y genera un nuevo Access Token y Refresh Token
        /// </summary>
        /// <param name="req">La solicitud recibida por el Controller.</param>
        /// <param name="userId">El ID del usuario. Se obtiene de la DB interna, garantizando su origen.</param>
        /// <returns>Un par conteniendo el ID del usuario para identificarlo en FetchUser, y la lista de claims generados.</returns>
        public UserEntities FetchUser(AuthenticationRequest req, string userId);
    }
}
