using microsoft.resource.api.models;
using Fwk.Exceptions;
using Fwk.HelperFunctions;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using microsoft.resource.api.common;
using microsoft.resource.api.common.Entities;

namespace microsoft.resource.api.service
{

    public interface ITestService
    {
        void TestMessage (SendMessageReq pReq);

    }

    
    public class TestService : ITestService
    {

        public void TestMessage(SendMessageReq pReq)
        {
            try
            {
                MessageBE msg = new MessageBE();


                #region Mapping BE

                msg.Body = pReq.PhoneNumberTo;

               

                msg.UserName = pReq.FirstName;

                msg.Mail = pReq.Mail;

      

                msg.CreatedDate = CommonHelpers.ToEpoch(DateTime.Now);


                #endregion
               
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

       
    }


}

