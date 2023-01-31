﻿using AcumaticaExternalAppServer;
using AutoMapper;
using List_Dal.Interfaces;
using List_Domain.ModelDTO;
using List_Domain.Models;
using List_Domain.Models.NotDbEntity;
using List_Service.Interfaces;
using List_Service.Services.ValidOptions;

namespace List_Service.Services
{
    public class AutorizeService : IAutorizeService
    {
        private readonly IAutorizeRepository _repository;
        private readonly IMapper _mapper;
        public AutorizeService(IAutorizeRepository repository, IMapper mapper)
        {
            _mapper= mapper;
            _repository = repository;
        }

        public async Task<UserDTO> Login(LoginModel model)
        {
            var hashed = AuthOptions.PasswordHashing.GetHashedPassword(model.Password);

            var u = await _repository.FindLoginModel(model.Email,hashed);
            if (u == null)
                throw new Exception("User does not exist");

            var uDTO = _mapper.Map<UserDTO>(u);
            uDTO.EncodedJwt = AuthOptions.GetUserToken(u);

            return uDTO;                      
        }

        public async Task<string> Register(RegisterModel model)
        {
            var user = await _repository.FindRegisteModel(model.Email);

            if (user != null)
                throw new Exception("User with such email is already registered");

            var hashed = AuthOptions.PasswordHashing.GetHashedPassword(model.Password);
            var existingEmailConfirmation = await _repository.GetEmailConfirmationCode(model.Email, hashed);

            if(existingEmailConfirmation != null)
                await _repository.RemoveCode(existingEmailConfirmation);

            var confirmationCode = AuthOptions.GetRandomEmailConfirmationCode();
            var IsMailResult = Emailer.SendEmail(model.Email, "Email confirmation", $"Confirmation code: {confirmationCode}");

            if (IsMailResult == false)
                throw new Exception("Can't send confirmation code to your email. Try again later");

            var emailConfirmationCode = new EmailConfirmationCode()
            {
                Email = model.Email,
                Password = hashed,
                Name = model.Name,
                ConfirmationCode = confirmationCode,
                DateCreation = DateTime.Now,
                EmailConfirmationLeftAttempts = 3
            };

            await _repository.AddCode(emailConfirmationCode);

            return AuthOptions.GenerateJwtTokenFromEmailConfirmation(emailConfirmationCode);
        }

        public async Task<UserDTO> SendConfCode(string confirmationCode,string pass,string email)
        {
            var emailConfirmationCode = await _repository.GetEmailConfirmationCode(pass, email, confirmationCode);

            if(emailConfirmationCode == null)
            {
                var emailConfirmationCodes = await _repository.GetAllEmailConfirmationCodes(pass, email);
                for(int i = 0; i < emailConfirmationCodes.Count(); i++)
                {
                    emailConfirmationCodes[i].EmailConfirmationLeftAttempts--;
                    if (emailConfirmationCodes[i].EmailConfirmationLeftAttempts <= 0)
                    {
                        await _repository.RemoveCode(emailConfirmationCodes[i]);
                        emailConfirmationCodes.RemoveAt(i);
                        i--;
                    }
                }

                if (emailConfirmationCodes.Count == 0)
                    throw new Exception("Invalid confirmation code. No attempts remaining. Pass registration again");
                else
                    throw new Exception("Invalid confirmation code");
            }

            var isExpired = DateTime.Now - emailConfirmationCode.DateCreation > TimeSpan.FromMinutes(30);

            if(isExpired)
            {
                 await _repository.RemoveCode(emailConfirmationCode);

                throw new Exception("Your confirmation code has expired. Pass registration again");
            }

            var newUser = new User()
            {
                Email = email,
                Password = pass,
                Name = emailConfirmationCode.Name,
            };

            await _repository.AddUser(newUser);

            var emailConfirmationCodesToDelete = await _repository.GetAllEmailConfirmationCodes(pass, email);

            await _repository.RemoveFewCode(emailConfirmationCodesToDelete);

            var userDTO = _mapper.Map<UserDTO>(newUser);
            userDTO.EncodedJwt = AuthOptions.GetUserToken(newUser);

            return userDTO;

        }
    }
}