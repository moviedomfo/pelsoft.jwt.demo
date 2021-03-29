using pelsoft.auth.Amazon.be;
using pelsoft.auth.common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace pelsoft.auth.Amazon.dac
{
    /// <summary>
    /// 
    /// </summary>
    public class AmazonDAC
    {
        private static List<DomainsBE> Domains;
        public static SocioBE Get(string usuario, string dominio)
        {

            SocioBE item = null;
            dominio = dominio.Replace("-", ".");
            try
            {
                var connectionString = CommonHelpers.GetCnn(CommonHelpers.cnnStringName_Amazon).ConnectionString;
                using (SqlConnection cnn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("[dbo].[Users_g]", cnn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@usr", usuario);

                    cmd.Parameters.AddWithValue("@domain", dominio);

                    cnn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            item = new SocioBE();
                            item.UserName = usuario;
                            item.Id = (Guid)(reader["Id"]);

                            item.Apellido = (string)(reader["apellido"]);
                            item.Nombre = (string)(reader["nombre"]);

                            if (reader["FechaNacimiento"] != DBNull.Value)
                                item.FechaNacimiento = Convert.ToDateTime(reader["FechaNacimiento"]);

                            if (reader["Documento"] != DBNull.Value)
                                item.Documento = reader["Documento"].ToString();
                            if (reader["Cuit"] != DBNull.Value)
                                item.Cuit = reader["Cuit"].ToString();

                            if (reader["sexo"] != DBNull.Value)
                                item.Sexo = reader["sexo"].ToString();
                            if (reader["nacionalidad"] != DBNull.Value)
                                item.Nacionalidad = reader["nacionalidad"].ToString();

                            if (reader["telefono"] != DBNull.Value)
                                item.Telefono = reader["telefono"].ToString();
                          

                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return item;
        }


      

        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainName">Proviene de la tabla Domains URL y es el SiteDoamin</param>
        /// <returns></returns>
        public  static int GetDimainId(string domainName)
        {
            domainName = domainName.Replace("-", ".");
            var AmazonDomains = RetriveDommains();
            var d = AmazonDomains.Where(p => p.Domain.ToLower().Equals(domainName.ToLower())).FirstOrDefault();
            if (d == null)
                throw new Fwk.Exceptions.FunctionalException("No es posible encontrar informacion configurada sobre el dominio " + domainName.ToLower());
            return d.DomainId;

       
        }



    

        /// <summary>
        /// Retorna dominios de la bd o cacheados
        /// </summary>
        /// <returns></returns>
        public static List<DomainsBE> RetriveDommains()
        {
            //si esta nulo o expiro refresca 
            if (Domains == null || CommonHelpers.Expired_ttl())
            {
                Domains = RetriveDominiosFromDB();
            }

            return Domains;
        }

        public static List<DomainsBE> RetriveDominiosFromDB()
        {
            DomainsBE item;
            List<DomainsBE> list = new List<DomainsBE>();
            var connectionString = CommonHelpers.GetCnn(CommonHelpers.cnnStringName_Amazon).ConnectionString;

            using (SqlConnection cnn = new SqlConnection(connectionString))
    
            using (SqlCommand cmd = new SqlCommand("[dbo].[Users_g]", cnn))
            
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cnn.Open();



                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        item = new DomainsBE();

                        item.DomainId = Convert.ToInt32(reader["DomainId"]);
                        item.Domain = reader["Domain"].ToString();
                        list.Add(item);
                    }
                }

                return list;

            }


        }



    }
}

