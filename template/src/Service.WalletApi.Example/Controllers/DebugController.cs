using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyJetWallet.ApiSecurityManager.ApiKeys;
using MyJetWallet.Domain;
using MyJetWallet.Sdk.Authorization;
using MyJetWallet.Sdk.Authorization.Extensions;
using MyJetWallet.Sdk.Authorization.Http;
using MyJetWallet.Sdk.WalletApi.Common;
using Newtonsoft.Json;
using Service.WalletApi.Example.Controllers.Contracts;
using SimpleTrading.ClientApi.Utils;

namespace Service.WalletApi.Example.Controllers
{
    [ApiController]
    [Route("/api/v1/Example/Debug")]
    public class DebugController: ControllerBase
    {
        private readonly IApiKeyStorage _encryptionKeyStorage;

        public DebugController(IApiKeyStorage encryptionKeyStorage)
        {
            _encryptionKeyStorage = encryptionKeyStorage;
        }
        
        // [HttpPost("token")]
        // public async Task<IActionResult> ParseToken([FromBody] TokenDto request)
        // {
        //     var (res, data) = await _encryptionKeyStorage.ParseToken(Program.Settings.SessionEncryptionApiKeyId, request.Token);
        //
        //     var json = JsonConvert.SerializeObject(data, Formatting.Indented);
        //
        //     return Ok($"Result: {res}.\nData:\n{json}");
        // }

        [HttpGet("hello")]
        public IActionResult HelloWorld()
        {
            return Ok("Hello world!");
        }


        [HttpGet("who")]
        [Authorize()]
        public IActionResult TestAuth()
        {
            var traderId = this.GetClientId();
            return Ok($"Hello, session is good");
        }

        [HttpPost("make-signature")]
        public IActionResult MakeSignatureAsync([FromBody] TokenDto data, [FromHeader(Name = "private-key")] string key)
        {
            return Ok();
        }

        [HttpPost("generate-keys")]
        public IActionResult GenerateKeysAsync()
        {
            var rsa = RSA.Create();

            var publicKey = rsa.ExportRSAPublicKey();
            var privateKey = rsa.ExportRSAPrivateKey();

            var response = new
            {
                PrivateKeyBase64 = Convert.ToBase64String(privateKey),
                PublicKeyBase64 = Convert.ToBase64String(publicKey)
            };

            return Ok(response);
        }

        [HttpPost("validate-signature")]
        [Authorize]
        public IActionResult ValidateSignatureAsync([FromBody] TokenDto data, [FromHeader(Name = AuthorizationConst.SignatureHeader)] string signature)
        {
            return Ok();
        }

        [HttpGet("my-ip")]
        [Authorize]
        public IActionResult GetMyApiAsync()
        {
            var ip = this.HttpContext.GetIp();
            
            var xff = HttpContext.Request.Headers.TryGetValue("X-Forwarded-For", out var xffheader) ? xffheader.ToString() : "none";
            var cf = HttpContext.Request.Headers.TryGetValue("CF-Connecting-IP", out var cfheader) ? cfheader.ToString() : "none";
            
            return Ok(new {IP = ip, XFF = xff, CF = cf});
        }
    }
}
