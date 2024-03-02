using BlazorChat.API.Services;
using BlazorChat.Data.EmailModel;
using BlazorChat.Data.ModelDTO;
using BlazorChat.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlazorChat.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IEmailSender _emailSender;
        private readonly UserManager<User> _userManager;
        public AccountController(IAccountRepository accountRepository, IEmailSender emailSender, UserManager<User> userManager)
        {
            _accountRepository = accountRepository;
            _emailSender = emailSender;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<ActionResult<User>> Register(UserSignupDto model)
        {
            bool checkUser = await _accountRepository.CheckUser(model.Email);
            if (checkUser)
            {
                var CreateUser = await _accountRepository.SignupUser(model);
                if (CreateUser)
                {
                    return Ok(model);
                }
                return BadRequest();
            }
            else
            {
                return Ok("User Already Exist");
            }


        }
        [HttpPost("Login")]
        public async Task<ActionResult<User>> Login(UserLoginDto model)
        {
            bool checkUser = await _accountRepository.LoginUser(model);
            if (checkUser)
            {
                string token = await _accountRepository.CreateToken(model.Email);
                return Ok(token);
            }
            return Ok("Entered Email or Password is incorrect!");
        }


        [HttpPost("ResetPassword")]

        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
                    if (user != null)
                    {
                        var result = await _userManager.ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.Password);
                        if (result.Succeeded)
                        {
                            return Ok("Password reset Successfully");
                        }
                        return BadRequest();
                    }
                    return NotFound("User Doesn't Exist!");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.ToString());
                }
            }
            return BadRequest();
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto forgotPasswordDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    User user = await _userManager.FindByEmailAsync(forgotPasswordDto.Email);
                    if (user == null)
                    {
                        return NotFound("No User found with the provided Email.");
                    }
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    // var callback = Url.Action(nameof(ResetPassword), "Account", new { token, email = user.Email }, Request.Scheme);
                    var callback = Url.Action("ResetPassword", "Account", new { token, email = user.Email }, "http", "localhost:7181");
                    callback = callback.Replace("/api", "");
                    Message message = new Message(user.Email, "Reset Password Token", callback);
                    await _emailSender.SendEmailAsync(message);
                    return Ok("Mail Send Successfully " + token);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.ToString());
                }
            }
            return BadRequest();
        }
    }
}
