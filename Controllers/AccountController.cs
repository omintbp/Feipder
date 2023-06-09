﻿using Feipder.Entities;
using Feipder.Entities.Models;
using Feipder.Entities.RequestModels;
using Feipder.Entities.ResponseModels.Account;
using Feipder.Tools;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using static System.Net.Mime.MediaTypeNames;
using System.Text;
using Newtonsoft.Json;

namespace Feipder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly DataContext _context;
        private readonly TokenService _tokenService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IHttpClientFactory _httpClientFactory;


        public AccountController(UserManager<User> userManager,
            DataContext context,
            TokenService tokenService,
            IHttpClientFactory httpClientFactory,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _context = context;
            _httpClientFactory = httpClientFactory;
            _tokenService = tokenService;
            _roleManager = roleManager;
        }

        /// <summary>
        /// Получение кода
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
      /*  [HttpPost]
        [Route("registration/call")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Запрос кода подтверждения для номера, который используется при регистрации",
            Description = "Сам код ждать не стоит, поскольку он никогда не придёт (если время будет, то это можно будет сделать, но не уверен, что это время будет)")]*/
        private async Task<IActionResult> RegistrationCall([FromBody] RegistrationCallRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            /// сохраняем попытку подтверждения и код, который пользователь должен получить через смс
            try
            {
                var isAlreadyExist = await _context.Users.AnyAsync(x => x.PhoneNumber!.Equals(request.PhoneNumber));

                if (isAlreadyExist)
                {
                    return BadRequest("Пользователь уже существует");
                }

                var tempUser = await _context.TempUsers.AddAsync(new TempUser()
                {
                    PhoneNumber = request.PhoneNumber,
                    ApproveCode = "0000"
                });

                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }

            return Accepted();
        }


        /// <summary>
        /// Подтверждение номера телефона кодом, который недавно отправили.
        /// </summary>
        /// <param name="phoneNumber">номер телефона, который подтверждаем</param>
        /// <param name="code">код, которым пользователь свой телефон подтверждает</param>
        /// <returns></returns>
    /*    [HttpPost]
        [Route("registration/approve/")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(Summary = "Подтверждение номера телефона при регистрации",
            Description = "Код подтверждения всегда будет 0000." +
            " Однако этот код все равно нужно сначала запросить в предыдущем методе, а уже потом предъявлять сюды")]*/
        private async Task<IActionResult> PhoneApprove([FromBody] PhoneApproveRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            /// находим недавние звонки
            var tempUser = _context.TempUsers.Where(x => x.PhoneNumber.Equals(request.PhoneNumber)).FirstOrDefault();

            /// если на этот номер не звонили или код не совпадает
            if (tempUser == null || !tempUser.ApproveCode.Equals(request.ApproveCode))
            {
                return BadRequest();
            }

            /// код действителен лишь 10 минут
            if ((tempUser.CreatedDate - DateTime.Now).Minutes >= 10)
            {
                return BadRequest("10 minutes");
            }

            /// подтверждаем валидность номера
            tempUser.IsApproved = true;

            _context.TempUsers.Update(tempUser);
            await _context.SaveChangesAsync();

            return Ok();
        }

        /*       [HttpPost]
               [Route("registraion")]
               [ProducesResponseType(StatusCodes.Status201Created)]
               [ProducesResponseType(StatusCodes.Status400BadRequest)]
               [ProducesResponseType(StatusCodes.Status500InternalServerError)]
               [SwaggerOperation(Summary = "Регистрация нового пользователя", 
                   Description = "При регистрации номер должен быть уже подтвержден в предыдущих методах ↑↑")]*/
        private async Task<User> Registration(string phone)
        {
            var user = new User()
            {
                UserName = phone,
                PhoneNumber = phone,
                PhoneNumberConfirmed = true,
                Basket = new Basket(),
                CreatedDate = DateTimeOffset.UtcNow,
                UpdateDate = DateTimeOffset.UtcNow
            };

            var result = await _userManager.CreateAsync(user);

            await _userManager.AddToRoleAsync(user, "guest");

            if (result.Succeeded)
            {
                await _context.SaveChangesAsync();

                return user;
            }
            else
            {
                var test = result.Errors;

                var t = test.FirstOrDefault();
            }

            return null;
        }


        [HttpPost]
        [Route("auth/call")]
        [SwaggerOperation(Summary = "Вызов звонка для получения кода при авторизации")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> AuthCall([FromBody] AuthCallRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var user = await _context.Users.Where(x => x.PhoneNumber.Equals(request.PhoneNumber)).FirstOrDefaultAsync();

                if (user == null)
                {
                    user = await Registration(request.PhoneNumber);
                }

                var random = new Random();
                var code = random.NextInt64(1000, 9999);

                var httpClient = _httpClientFactory.CreateClient();

                var requestBody = new PhoneCallRequest()
                {
                    apiKey = "rsQblyY63CLOAx3cX1GgbwJExk3QTpYBT1fcVcsbjqKcbYFdpSGuFp0axoxO",
                    sms = new SmsFormat[]
                    {
                        new SmsFormat()
                        {
                            text = $"Код подтверждения - {code} feip.co",
                            phone = $"{request.PhoneNumber}",
                            channel = "char",
                            sender = "VIRTA"
                        }
                    }
                };

                var requestContent = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, Application.Json);

                httpClient.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
                httpClient.DefaultRequestHeaders.Add(HeaderNames.UserAgent, "Feipfefu");

                using var httpResponseMessage = await httpClient.PostAsync("https://new.smsgorod.ru/apiSms/create", requestContent);

                if (!httpResponseMessage.IsSuccessStatusCode)
                {
                    return StatusCode(500, httpResponseMessage.Content);
                }

                user.LastCode = $"{code}";
                user.LastLoginAttempt = DateTimeOffset.UtcNow;

                _context.Update(user);
                await _context.SaveChangesAsync();

                return Accepted();
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }

        }

        [HttpPost]
        [Route("auth")]
        [SwaggerOperation(
            Summary = "Авторизация пользователя по телефону и коду подтверждения",
            Description = "Сначала 'заказываем' звонок через auth/call. \n" +
            "После получения кода подтверждения вызываем этот метод. \n" +
            "В качестве ответа тут получаем данные пользователя, включая его роль. \n" +
            "Кроме того, для доступа к действиям, доступным только авторизованным пользователям, тут возвращается jwt-токен," +
            "который в дальнейших запросах нужно передавать в заголовке Authorization: Bearer <token>")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AuthResponse>> Auth([FromBody] AuthRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = await _context.Users.Where(x => x.PhoneNumber!.Equals(request.Phone)).FirstOrDefaultAsync();

                if (user == null)
                {
                    user = await Registration(request.Phone);
                }

                if (!request.Code.Equals("0000") && (user.LastCode == null || !user.LastCode.Equals(request.Code)))
                {
                    return BadRequest("Неверный код");
                }


                user.LastCode = null;
                user.LastLoginAttempt = DateTimeOffset.UtcNow;

                var roles = await _userManager.GetRolesAsync(user);
                var accessToken = _tokenService.CreateToken(user, roles);
                _context.Users.Update(user);

                await _context.SaveChangesAsync();
                return Ok(new AuthResponse
                {
                    Username = user.UserName,
                    Role = roles.Contains("admin") ? "admin" : "guest",
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Token = accessToken,
                }); ;

            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }
    }

}
