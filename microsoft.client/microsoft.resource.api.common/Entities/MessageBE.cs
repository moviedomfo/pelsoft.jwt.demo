using Fwk.Bases;
using System;
using System.Collections.Generic;
using System.Text;

namespace microsoft.resource.api.common.Entities
{
 
  


    public class MessageBE 
    {
     
        public System.String Body { get; set; }


        /// <summary>
        /// Nombre Completo del Usuario. Campo  FirstName  del JSON
        /// </summary>
        public System.String UserName { get; set; }

        /// <summary>
        /// Dirección Email del usuario. Campo  Mail  del JSON
        /// </summary>
        public System.String Mail { get; set; }

       

        /// <summary>
        /// Fecha/hora de creación 
        /// </summary>
        public long? CreatedDate { get; set; }


   

    }
}
