using JwtAuthenticationManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace JwtAuthenticationManager
{
    public class JwtTokenManager
    {
        //RECOMENDADO COLOCA UMA CHAVE EXTREMAMENTE FORTE
        public const string JWT_SECURITY_KEY = "f:'FC9Yx9_M%`&4u[7p!:^'V]kU9p5@%\"({\"2j7S";                
        public const int JWT_TOKEN_VALIDITY_MINS = 3600;
        private readonly List<UserAccount> _userAccountList = new List<UserAccount>();

        public JwtTokenManager()
        {
            _userAccountList = new List<UserAccount>()
            {
                new UserAccount{ UserName = "admin", PassWord = "admin123", Role = "Administrator" },
                new UserAccount{ UserName = "user01", PassWord = "user01", Role = "User" },
            };

        }

        public AuthenticationResponse? GenerateJwtTokenWithSymmetricKey(AuthenticationRequest authenticationRequest)
        {

            if (string.IsNullOrWhiteSpace(authenticationRequest.UserName) || string.IsNullOrWhiteSpace(authenticationRequest.PassWord))
                return null;

            /*Validate from Database*/
            var user = _userAccountList.Where(u => u.UserName == authenticationRequest.UserName && u.PassWord == authenticationRequest.PassWord).FirstOrDefault();
            if (user == null) return null;

            var tokenExpiryTimeStamp = DateTime.Now.AddMinutes(JWT_TOKEN_VALIDITY_MINS);
            var tokenKey = Encoding.ASCII.GetBytes(JWT_SECURITY_KEY);
            var claimsIdentity = new ClaimsIdentity(new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Name, authenticationRequest.UserName),
                new Claim(ClaimTypes.Role, user.Role)
            });

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(tokenKey),
                SecurityAlgorithms.HmacSha256
                );

            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = tokenExpiryTimeStamp,
                SigningCredentials = signingCredentials

            };

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
            var token = jwtSecurityTokenHandler.WriteToken(securityToken);

            return new AuthenticationResponse()
            {
                UserName = authenticationRequest.UserName,
                ExpiresIn = JWT_TOKEN_VALIDITY_MINS,
                JwtToken = token
            };
        }

        public AuthenticationResponse GenerateTokenAsymmetric()
        {
            using RSA rsa = RSA.Create();
            var privateKey = Convert.FromBase64String(JWT_SECURITY_KEY.ToString());


            var tokenKey = Encoding.ASCII.GetBytes(JWT_SECURITY_KEY);

            rsa.ImportRSAPrivateKey( // Convert the loaded key from base64 to bytes.
                source: tokenKey, // Use the private key to sign tokens
                bytesRead: out int _); // Discard the out variable 

            var signingCredentials = new SigningCredentials(
                key: new RsaSecurityKey(rsa),
                algorithm: SecurityAlgorithms.RsaSha256 // Important to use RSA version of the SHA algo 
            );

            DateTime jwtDate = DateTime.Now;

            var jwt = new JwtSecurityToken(
                audience: "account",
                issuer: "efronteiras",
                claims: new Claim[] { new Claim(ClaimTypes.NameIdentifier, "efronteiras") },
                notBefore: jwtDate,
                expires: jwtDate.AddMinutes(JWT_TOKEN_VALIDITY_MINS),
                signingCredentials: signingCredentials
            );

            string token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return new AuthenticationResponse()
            {
                UserName = "efronteiras",
                ExpiresIn = JWT_TOKEN_VALIDITY_MINS,
                JwtToken = token
            };
        }

    }
}
