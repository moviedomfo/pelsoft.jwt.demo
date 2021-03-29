using System;
using System.Collections.Generic;
using System.Text;

namespace pelsoft.auth.Amazon.be
{
    
    public class ActiveDirectoryGroupBE
    {

        /// <summary>
        /// Nombre del domijio al que pertenece el grupo
        /// </summary>
        public string Domain { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Ejemplos:
        /// CN=GS_Comite_comunicacion_RW,OU=Seguridad,DC=actionlinecba,DC=org
        /// CN=GS_CalidadTP_R,OU=Avanzados,OU=Analistas,OU=Seguridad,DC=actionlinecba,DC=org
        /// </summary>
        public string DistinguishedName { get; set; }
        /// <summary>
        /// Defines the Active Directory Schema category. For example, objectCategory = Person
        /// Ej: CN=Group,CN=Schema,CN=Configuration,DC=actionlinecba,DC=org
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        ///Common Name
        /// </summary>
        public string CN { get; set; }

        /// <summary>
        /// Reprecenta la lista de unidades organizacionales del dominio
        /// OU = Organizational Unit 
        /// </summary>
        public string[] OU { get; set; }


        public ActiveDirectoryGroupBE()
        {
        }
      

    }
}
