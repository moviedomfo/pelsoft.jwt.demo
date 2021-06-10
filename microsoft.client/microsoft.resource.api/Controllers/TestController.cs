using System;
using System.Net;
using microsoft.resource.api.common;
using microsoft.resource.api.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using microsoft.resource.api.service;
using Fwk.Exceptions;
using System.Text;


namespace microsoft.resource.api.Controllers
{
 //   [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ITestService service;

        public TestController(ITestService service)
        {
            this.service = service;

        }

     
        [HttpPost("[action]")]
        public IActionResult SendMessage(SendMessageReq pReq)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    StringBuilder wStr = new StringBuilder();

                    wStr.AppendLine("Some data is invalid");

                    if (string.IsNullOrEmpty(pReq.PhoneNumberTo))
                        wStr.AppendLine("PhoneNumberTo in url param required");

                    if (string.IsNullOrEmpty(pReq.PhoneNumber))
                        wStr.AppendLine("PhoneNumber  required");

                    ApiErrorResponse wResp = new ApiErrorResponse(HttpStatusCode.BadRequest, wStr.ToString());
                    ObjectResult wObjRes = this.StatusCode(400, wResp);
                    return wObjRes;
                }

                service.TestMessage(pReq);

                return Ok();
            }
            catch (Exception ex)
            {
                apiLogServices.LogError_asynk(ex);
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("[action]")]
        public IActionResult test3()
        {
            try
            {
                return Ok("Resource server works successfully test3");
            }
            catch (Exception ex)
            {
               
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("[action]")]
        public IActionResult test4()
        {
            try
            {
                return Ok("Resource server works successfully test4");
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }


    }
}