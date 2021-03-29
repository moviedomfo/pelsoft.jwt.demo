using Fwk.Exceptions;
using pelsoft.auth.common.storage;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;

namespace pelsoft.auth.common
{
    public interface IRefreshTokenProvider
    {
        RefreshToken FetchToken(string refreshToken);
        RefreshToken GenerateRefreshToken(string userId, string ipAddress);
        RefreshToken RefreshToken(RefreshToken refreshToken, string ipAddress);
        bool Revoke(RefreshToken refreshToken, string ipAddress, RefreshToken revokingToken = null);
    }

    /// <summary>
    /// Clase para manejar tokens de refresco.
    /// Provee métodos para generar un token nuevo, renovar un token existente
    /// y revocar un token existente.
    /// Todos los métodos almacenan el resultado mediante ApiTokenStorage
    /// </summary>
    public class RefreshTokenProvider : IRefreshTokenProvider

    {
        IStore _store;
        public RefreshTokenProvider(IStore store)
        {
            _store = store;
        }

        /// <summary>
        /// Genera un nuevo refresh token
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public RefreshToken GenerateRefreshToken(string userId, string ipAddress)
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[64];
            rngCryptoServiceProvider.GetBytes(randomBytes);

            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(randomBytes),
                // Este valor en un caso real debe ser configurado. En este caso lo dejamos harcodeado a una jornada laboral
                Expires = DateTime.UtcNow.AddHours(9), 
                Created = DateTime.UtcNow,
                CreatedByIp = ipAddress,
                UserID = userId
            };

            _store.Set(refreshToken.Token, refreshToken);
            return refreshToken;
        }
      
        /// <summary>
        /// Genera un nuevo token de refresco y revoca el viejo.
        /// </summary>
        /// <param name="refreshToken">Token a refrescar</param>
        /// <param name="ipAddress">IP del generador</param>
        /// <returns>Nuevo token</returns>
        public RefreshToken RefreshToken(RefreshToken old_refresh_token, string ipAddress)
        {
            var refresh_token = GenerateRefreshToken(old_refresh_token.UserID, ipAddress);
            Revoke(old_refresh_token, ipAddress, refresh_token);

            return refresh_token;
        }

        /// <summary>
        /// Obtener los datos del Token de almacenamiento.
        /// </summary>
        /// <param name="refresh_token">El código del Token</param>
        /// <returns><c>null</c> si el token está vencido o revocado; la información del token si es válido.</returns>
        public RefreshToken FetchToken(string refreshtoken)
        {
            var refresh_token = _store.Get(refreshtoken);

            if (refresh_token == null || !refresh_token.IsActive)
            {
                var fe = new FunctionalException("Token invalido o no existe");
                fe.ErrorId = "460";
                throw fe;
            }

            return refresh_token;
        }

        /// <summary>
        /// Revoca el token de refresco.
        /// </summary>
        /// <param name="refresh_token">Token a refrescar</param>
        /// <param name="ipAddress">IP del generador</param>
        /// <returns><c>true</c> si es revocado correctamente; <c>false</c> si estaba revocado o no existe</returns>
        public bool Revoke(RefreshToken refreshToken, string ipAddress, RefreshToken revokingToken = null)
        {
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            if (revokingToken != null)
            {
                refreshToken.ReplacedByToken = revokingToken.Token;
            }
            _store.Set(refreshToken.Token, refreshToken);

            return true;
        }       
    }
}
