using Fwk.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace microsoft.resource.api.common
{
    public class apiLogServices
    {



        /// <summary>
        /// Almacena el request en bruto en la BD. Si no lo logra por error de SQL. Lo guarda en cache para luego ser reintentado de inserción
        /// 
        /// </summary>
        /// <param name="ex"></param>
        public static Task LogError_asynk(Exception ex)
        {
            return Task.Run(() =>
            {
                apiLogServices.TryInsertLog(ex);
            });
        }


        /// <summary>
        /// Siempre enviara mail si el hilo está configurado para eso.
        /// </summary>
        /// <param name="ex"></param>
        static void TryInsertLog(Exception ex)
        {
            ApiEvent apiEvent = new ApiEvent();

            apiEvent.Source = apiAppSettings.apiConfig.api_InstanceName;
            apiEvent.LogDate = DateTime.Now;
            apiEvent.Message = ExceptionHelper.GetAllMessageException(ex);
            apiEvent.Machine = Environment.MachineName;
            apiEvent.Type = EventType.Error;

            Insert(apiEvent);
        }

        static void Insert(ApiEvent pEvent)
        {

            var connectionString = CommonHelpers.GetCnn(CommonHelpers.cnnStringName_microsoft).ConnectionString;
            using (SqlConnection wCnn = new SqlConnection(connectionString))
            using (SqlCommand wCmd = new SqlCommand())
            {
                try
                {
                    wCnn.Open();
                    wCmd.Connection = wCnn;
                    wCmd.CommandType = CommandType.StoredProcedure;
                    wCmd.CommandText = "fwk_Logs_i";
                    SqlParameter wParam = null;

                    wParam = wCmd.Parameters.Add("Id", SqlDbType.UniqueIdentifier);
                    wParam.Value = pEvent.Id;

                    wParam = wCmd.Parameters.Add("Message", SqlDbType.VarChar);

                    wParam.Value = pEvent.Message;

                    wParam = wCmd.Parameters.Add("Source", SqlDbType.VarChar);
                    wParam.Value = pEvent.Source;

                    wParam = wCmd.Parameters.Add("LogType", SqlDbType.VarChar);
                    wParam.Value = pEvent.Type;

                    wParam = wCmd.Parameters.Add("Machine", SqlDbType.VarChar);
                    wParam.Value = pEvent.Machine;

                    wParam = wCmd.Parameters.Add("LogDate", SqlDbType.DateTime);
                    wParam.Value = pEvent.LogDate;

                    wParam = wCmd.Parameters.Add("UserLoginName", SqlDbType.VarChar);
                    wParam.Value = "";

                    wParam = wCmd.Parameters.Add("AppId", SqlDbType.VarChar);
                    wParam.Value = pEvent.AppId;

                    wParam = wCmd.Parameters.Add("ServiceInstanceUnique", SqlDbType.UniqueIdentifier);
                    wParam.Value = pEvent.ServiceInstanceUnique;

            

                    wCmd.ExecuteNonQuery();
                    wCnn.Close();
                }
                catch (Exception ex)
                {
                    TechnicalException te = new TechnicalException("Error de Fwk.Logging al intentar insertar log en base de datos", ex);
                    te.ErrorId = "9004";
           
                    throw te;
                }
            }
        }
    }
}
