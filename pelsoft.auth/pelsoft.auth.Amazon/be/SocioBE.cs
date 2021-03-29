using System;
using System.Collections.Generic;
using System.Text;

namespace pelsoft.auth.Amazon.be
{


  

    public class SocioBE
    {
       
        
        public string UserName { get; set; }
        /// <summary>
        /// EMP_ID
        /// </summary>
        public Guid Id { get; set; }

        public string Apellido { get; set; }

        public string Nombre { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Documento { get; set; }
        public string Cuit { get; set; }
        public string Sexo { get; set; }

        public string Profesion { get; set; }

        public string Nacionalidad { get; set; }
        public string Telefono;




    }

  


    public class WindosUserBE
    {
        public int Id { get; set; }
        public string Domain { get; set; }
        public string WindowsUser { get; set; }
        public int DomainId { get; set; }

    }


    public class DomainsBE
    {
        public string Domain { get; set; }
        public int DomainId { get; set; }
    }
}
