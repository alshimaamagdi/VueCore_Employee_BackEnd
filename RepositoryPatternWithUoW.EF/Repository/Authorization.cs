using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RepositoryPatternWithUoW.Core.Interface;
using RepositoryPatternWithUoW.Core.Models.Domain;
using RepositoryPatternWithUoW.Core.Models.Dto.Authorization;
using RepositoryPatternWithUoW.Core.Models.Helper;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUoW.EF.Repository
{
    public class Authorization : IAuthorization
    {
        private readonly UserManager<Users> _userManager;
        private readonly JWT _jwt;

        public Authorization(UserManager<Users> userManager,  IOptions<JWT> jwt)
        {
            _userManager = userManager;
            _jwt = jwt.Value;
        }
  
        public async Task<AuthorizeReturnModel> RegisterAsync(RegisterModel model)
        {
            try
            {
                if (await _userManager.FindByEmailAsync(model.Email) is not null)
                    return new AuthorizeReturnModel { Message = "Email is already registered!" };

                if (await _userManager.FindByNameAsync(model.Username) is not null)
                    return new AuthorizeReturnModel { Message = "Username is already registered!" };

                var user = new Users
                {
                    UserName = model.Username,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (!result.Succeeded)
                {
                    var errors = string.Empty;

                    foreach (var error in result.Errors)
                        errors += $"{error.Description},";

                    return new AuthorizeReturnModel { Message = errors };
                }

                await _userManager.AddToRoleAsync(user, "User");

                var jwtSecurityToken = await CreateJwtToken(user);

                return new AuthorizeReturnModel
                {
                    Email = user.Email,
                    ExpiresOn = jwtSecurityToken.ValidTo,
                    IsAuthenticated = true,
                    Roles = new List<string> { "User" },
                    Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                    Username = user.UserName
                };
            }
            catch (Exception ex)
            {
                // Handle the exception
                return new AuthorizeReturnModel { Message = $"An error occurred: {ex.Message}" };
            }
            finally
            {
                // Optional: Code that will be executed regardless of whether an exception occurred
            }

        }

        public async Task<AuthorizeReturnModel> LoginAsync(LoginModel model)
        {
            try
            {
                var authModel = new AuthorizeReturnModel();

                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    authModel.Message = "Email or Password is incorrect!";
                    return authModel;
                }

                var jwtSecurityToken = await CreateJwtToken(user);
                var rolesList = await _userManager.GetRolesAsync(user);

                authModel.IsAuthenticated = true;
                authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                authModel.Email = user.Email;
                authModel.Username = user.UserName;
                authModel.ExpiresOn = jwtSecurityToken.ValidTo;
                authModel.Roles = rolesList.ToList();

                return authModel;
            }
            catch (Exception ex)
            {
                // Handle the exception
                return new AuthorizeReturnModel { Message = $"An error occurred: {ex.Message}" };
            }

        }
        private async Task<JwtSecurityToken> CreateJwtToken(Users user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.DurationInDays),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }
    }
}
