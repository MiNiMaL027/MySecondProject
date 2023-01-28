using AcumaticaExternalAppServer;
using List_Dal.Interfaces;
using List_Domain.Models;
using List_Domain.Models.NotDbEntity;
using List_Service.Interfaces;
using List_Service.Services.ValidOptions;

namespace List_Service.Services
{
    public class AutorizeService : IAutorizeService
    {
        private readonly IAutorizeRepository _repository;
        public AutorizeService(IAutorizeRepository repository)
        {
            _repository = repository;
        }

        public async Task<UserDTO> Login(LoginModel model)
        {
            string hashed = AuthOptions.PasswordHashing.GetHashedPassword(model.Password);
            User u = await _repository.FindLoginModel(model,hashed);
            if (u == null)
                throw new Exception("User does not exist");
            UserDTO uDTO = new UserDTO(u);
            uDTO.EncodedJwt = AuthOptions.GetUserToken(u);
            return uDTO;                      
        }

        public async Task<string> Register(RegisterModel model)
        {
            User? user = await _repository.FindRegisteModel(model);
            if (user != null)
                throw new Exception("User with such email is already registered");
            string hashed = AuthOptions.PasswordHashing.GetHashedPassword(model.Password);
            EmailConfirmationCode existingEmailConfirmation = await _repository.GetEmailConfirmationCode(model, hashed);
            if(existingEmailConfirmation != null)
            {
                await _repository.RemoveCode(existingEmailConfirmation);
            }
            string confirmationCode = AuthOptions.GetRandomEmailConfirmationCode();
            bool mailResult = Emailer.SendEmail(model.Email, "Email confirmation", $"Confirmation code: {confirmationCode}");
            if (mailResult == false)
            {
                throw new Exception("Can't send confirmation code to your email. Try again later");
            }
            EmailConfirmationCode emailConfirmationCode = new EmailConfirmationCode()
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
            EmailConfirmationCode emailConfirmationCode = await _repository.GetEmailConfirmationCode(pass, email, confirmationCode);
            if(emailConfirmationCode == null)
            {
                List<EmailConfirmationCode> emailConfirmationCodes = await _repository.GetAllEmailConfirmationCodes(pass, email);
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
            bool isExpired = DateTime.Now - emailConfirmationCode.DateCreation > TimeSpan.FromMinutes(30);
            if(isExpired)
            {
                 await _repository.RemoveCode(emailConfirmationCode);
                throw new Exception("Your confirmation code has expired. Pass registration again");
            }
            User newUser = new User()
            {
                Email = email,
                Password = pass,
                Name = emailConfirmationCode.Name,
            };
            await _repository.AddUser(newUser);
            List<EmailConfirmationCode> emailConfirmationCodesToDelete = await _repository.GetAllEmailConfirmationCodes(pass, email);
            await _repository.RemoveFewCode(emailConfirmationCodesToDelete);
            UserDTO userDTO = new UserDTO(newUser);
            userDTO.EncodedJwt = AuthOptions.GetUserToken(newUser);
            return userDTO;

        }
    }
}
