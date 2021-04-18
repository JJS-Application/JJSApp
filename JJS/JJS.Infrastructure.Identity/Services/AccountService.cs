using JJS.Application.DTOs.Account;
using JJS.Application.DTOs.Account.Response;
using JJS.Application.DTOs.Email;
using JJS.Application.Enums;
using JJS.Application.Exceptions;
using JJS.Application.FileUpload;
using JJS.Application.Interfaces;
using JJS.Application.Wrappers;
using JJS.Domain.Settings;
using JJS.Infrastructure.Identity.Contexts;
using JJS.Infrastructure.Identity.Helpers;
using JJS.Infrastructure.Identity.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;

using Org.BouncyCastle.Ocsp;

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Cache;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace JJS.Infrastructure.Identity.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly JWTSettings _jwtSettings;
        private readonly IDateTimeService _dateTimeService;
        private readonly IdentityContext _context;
        private readonly IDropboxHelper _dropBoxHelper;
        private readonly string _userDropBoxLocation = "/User/";
        private readonly string _avtarExtension = ".png";
        private readonly string _destinationFolder;

        public AccountService(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<JWTSettings> jwtSettings,
            IDateTimeService dateTimeService,
            SignInManager<ApplicationUser> signInManager,
            IEmailService emailService, IdentityContext context,
            IDropboxHelper dropBoxHelper,
            IOptions<DropBoxConfig> dropBoxConfig)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtSettings = jwtSettings.Value;
            _dateTimeService = dateTimeService;
            _signInManager = signInManager;
            this._emailService = emailService;
            _dropBoxHelper = dropBoxHelper;
            _destinationFolder = dropBoxConfig.Value.DestinationFolder;
            var path = (_destinationFolder + _userDropBoxLocation).TrimEnd('/');
            var checkFileExists = _dropBoxHelper.CheckExists(_destinationFolder, path).GetAwaiter().GetResult();
            if (!checkFileExists)
            {
                _dropBoxHelper.CreateFolderAsync(path).GetAwaiter().GetResult();
            }
        }

        public async Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request, string ipAddress)
        {
            var user = _context.Users.Include(x => x.RefreshTokens).FirstOrDefault(x => x.Email == request.Email);
            if (user == null)
            {
                throw new ApiException($"No Accounts Registered with {request.Email}.");
            }
            var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                throw new ApiException($"Invalid Credentials for '{request.Email}'.");
            }
            if (!user.EmailConfirmed)
            {
                throw new ApiException($"Account Not Confirmed for '{request.Email}'.");
            }
            if (user.IsDeleted)
            {
                throw new ApiException($"This account is deleted. Please contact to admin");
            }
            if (!user.IsActive)
            {
                throw new ApiException($"This account is not active. Please contact to admin");
            }
            JwtSecurityToken jwtSecurityToken = await GenerateJWToken(user);
            AuthenticationResponse response = new AuthenticationResponse();
            response.Id = user.Id;
            response.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            response.Email = user.Email;
            response.UserName = user.UserName;
            var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            response.Roles = rolesList.ToList();
            response.IsVerified = user.EmailConfirmed;
            var refreshToken = GenerateRefreshToken(ipAddress);
            response.RefreshToken = refreshToken.Token;

            response.FirstName = user.FirstName;
            response.LastName = user.LastName;
            response.ImageUrl = user.ImageUrl;
            await AddOrUpdateRefreshToken(user, refreshToken);
            return new Response<AuthenticationResponse>(response, $"Authenticated {user.UserName}");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private async Task AddOrUpdateRefreshToken(ApplicationUser user, RefreshToken token)
        {
            user.RefreshTokens.Add(token);
            _context.Update(user);
            await _context.SaveChangesAsync();
        }
        public async Task<Response<string>> RegisterAsync(RegisterRequest request, string origin)
        {
            var userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);
            if (userWithSameUserName != null)
            {
                throw new ApiException($"Username '{request.UserName}' is already taken.");
            }
            var user = new ApplicationUser
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName
            };
            var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
            if (userWithSameEmail == null)
            {
                var result = await _userManager.CreateAsync(user, request.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, Roles.Basic.ToString());
                    var verificationUri = await SendVerificationEmail(user, origin);
                    //TODO: Attach Email Service here and configure it via appsettings
                    await _emailService.SendAsync(new EmailRequest() { From = "vsdecourse@gmail.com", To = user.Email, Body = $"Please confirm your account by visiting this URL {verificationUri}", Subject = "Confirm Registration" });
                    return new Response<string>(user.Id, message: $"User Registered. Please confirm your account by visiting this URL {verificationUri}");
                }
                else
                {
                    throw new ApiException($"{result.Errors}");
                }
            }
            else
            {
                throw new ApiException($"Email {request.Email } is already registered.");
            }
        }

        private async Task<JwtSecurityToken> GenerateJWToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var roleClaims = new List<Claim>();

            for (int i = 0; i < roles.Count; i++)
            {
                roleClaims.Add(new Claim("roles", roles[i]));
            }

            string ipAddress = IpHelper.GetIpAddress();

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id),
                new Claim("ip", ipAddress)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials);
            return jwtSecurityToken;
        }

        private string RandomTokenString()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[60];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            // convert random bytes to hex string
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }

        private async Task<string> SendVerificationEmail(ApplicationUser user, string origin)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var route = "api/account/confirm-email/";
            var _enpointUri = new Uri(string.Concat($"{origin}/", route));
            var verificationUri = QueryHelpers.AddQueryString(_enpointUri.ToString(), "userId", user.Id);
            verificationUri = QueryHelpers.AddQueryString(verificationUri, "code", code);
            //Email Service Call Here
            return verificationUri;
        }

        public async Task<Response<string>> ConfirmEmailAsync(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                return new Response<string>(user.Id, message: $"Account Confirmed for {user.Email}. You can now use the /api/Account/authenticate endpoint.");
            }
            else
            {
                throw new ApiException($"An error occured while confirming {user.Email}.");
            }
        }

        private RefreshToken GenerateRefreshToken(string ipAddress)
        {
            return new RefreshToken
            {
                Token = RandomTokenString(),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,
                CreatedByIp = ipAddress
            };
        }

        public async Task ForgotPassword(ForgotPasswordRequest model, string origin)
        {
            var account = await _userManager.FindByEmailAsync(model.Email);

            // always return ok response to prevent email enumeration
            if (account == null) return;

            var code = await _userManager.GeneratePasswordResetTokenAsync(account);
            var route = "api/account/reset-password/";
            var _enpointUri = new Uri(string.Concat($"{origin}/", route));
            var emailRequest = new EmailRequest()
            {
                Body = $"You reset token is - {code}",
                To = model.Email,
                Subject = "Reset Password",
            };
            await _emailService.SendAsync(emailRequest);
        }

        public async Task<Response<string>> ResetPassword(ResetPasswordRequest model)
        {
            var account = await _userManager.FindByEmailAsync(model.Email);
            if (account == null) throw new ApiException($"No Accounts Registered with {model.Email}.");
            var result = await _userManager.ResetPasswordAsync(account, model.Token, model.Password);
            if (result.Succeeded)
            {
                return new Response<string>(model.Email, message: $"Password Resetted.");
            }
            else
            {
                throw new ApiException($"Error occured while reseting the password.");
            }
        }

        public async Task<Response<AuthenticationResponse>> RefreshToken(string token, string ipAddress)
        {
            var user = _context.Users.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));
            // return null if no user found with token
            if (user == null) return null;
            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

            // return null if token is no longer active
            if (!refreshToken.IsActive) return null;

            // replace old refresh token with a new one and save
            var newRefreshToken = GenerateRefreshToken(ipAddress);
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            refreshToken.ReplacedByToken = newRefreshToken.Token;

            await AddOrUpdateRefreshToken(user, newRefreshToken);

            // generate new jwt
            JwtSecurityToken jwtToken = await GenerateJWToken(user);
            var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            AuthenticationResponse response = new AuthenticationResponse
            {
                Id = user.Id,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                Email = user.Email,
                UserName = user.UserName,
                Roles = rolesList.ToList(),
                IsVerified = user.EmailConfirmed,
                RefreshToken = refreshToken.Token,
                FirstName = user.FirstName,
                LastName = user.LastName,
                ImageUrl = user.ImageUrl
            };
            return new Response<AuthenticationResponse>(response, $"Authenticated {user.UserName}");
        }

        public async Task<Response<bool>> RevokeToken(string token, string ipAddress)
        {
            var user = _context.Users.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));

            // return false if no user found with token
            if (user == null)
            {
                return new Response<bool>(await Task.FromResult(false), "No User Found.");
            }

            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

            // return false if token is not active
            if (!refreshToken.IsActive)
            {
                return new Response<bool>(await Task.FromResult(false), "Token is not active.");
            }

            // revoke token and save
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            await AddOrUpdateRefreshToken(user, refreshToken);
            return new Response<bool>(await Task.FromResult(false), "Revoke Token.");
        }

        public async Task<Response<List<UserResponse>>> GetAll()
        {
            var user = await _userManager.Users.Where(x => x.IsActive && x.IsDeleted)
                .Select(x => new UserResponse
                {
                    Id = x.Id,
                    Email = x.Email,
                    FirstName = x.FirstName,
                    ImageUrl = x.ImageUrl,
                    IsCompany = x.IsCompany,
                    LastName = x.LastName,
                    PhoneNumber = x.PhoneNumber
                }).ToListAsync();
            return new Response<List<UserResponse>>(user);
        }

        public async Task<Response<UserResponse>> GetById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) throw new ApiException($"Invalid Id: {id}.");
            if (!user.IsActive) throw new ApiException($"This user is inactive. Please contact to Admin");
            if (user.IsDeleted) throw new ApiException($"This user is Deleted. Please contact to Admin");
            var userResponse = new UserResponse
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                ImageUrl = user.ImageUrl,
                IsCompany = user.IsCompany,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber
            };
            return new Response<UserResponse>(userResponse);
        }

        public async Task<Response<string>> UploadFileAsync(byte[] file, string id)
        {
            await _dropBoxHelper.Upload(CreateFilePath(id), file);
            return new Response<string>("Uploaded Successfully.");
        }

        public async Task<Response<string>> DownloadAsync(string id)
        {
            byte[] bytes = await _dropBoxHelper.Download(CreateFilePath(id));
            var imageBase64String = $"data:image/{_avtarExtension.TrimStart('.')};base64,{Convert.ToBase64String(bytes)}";
            return new Response<string>(imageBase64String);
        }

        private string CreateFilePath(string userId) =>
             $"{_destinationFolder}{_userDropBoxLocation}{userId}{_avtarExtension}";

    }

}
