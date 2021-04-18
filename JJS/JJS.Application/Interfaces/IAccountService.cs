using JJS.Application.DTOs.Account;
using JJS.Application.DTOs.Account.Response;
using JJS.Application.Wrappers;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JJS.Application.Interfaces
{
    public interface IAccountService
    {
        Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request, string ipAddress);
        Task<Response<string>> RegisterAsync(RegisterRequest request, string origin);
        Task<Response<string>> ConfirmEmailAsync(string userId, string code);
        Task ForgotPassword(ForgotPasswordRequest model, string origin);
        Task<Response<string>> ResetPassword(ResetPasswordRequest model);
        Task<Response<AuthenticationResponse>> RefreshToken(string token, string ipAddress);
        Task<Response<bool>> RevokeToken(string token, string ipAddress);
        Task<Response<List<UserResponse>>> GetAll();
        Task<Response<UserResponse>> GetById(string id);
        Task<Response<string>> UploadFileAsync(byte[] file, string id);
        Task<Response<string>> DownloadAsync(string id);
    }
}
