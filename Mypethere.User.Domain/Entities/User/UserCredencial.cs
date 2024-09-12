using Konscious.Security.Cryptography;

using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Mypethere.User.Domain.Entities.User
{
    public record UserCredencial
    {
        public Guid Id { get; init; }
        public string Password { get; init; }
        public string UserName { get; init; }
        public IEnumerable<CredencialType> CredencialTypes { get; init; }
        public User User { get; init; }

        // Construtor posicional para inicializar o record com validação
        public UserCredencial(Guid id, string password, string userName, IEnumerable<CredencialType> credencialTypes, User user)
        {
            if (!IsValidPassword(password))
            {
                throw new BadRequestException("The password must contain at least one lowercase letter, one uppercase letter, one number and one special character.", HttpStatusCode.UnprocessableEntity);
            }

            Id = id;
            Password = password;
            UserName = userName;
            CredencialTypes = credencialTypes;
            User = user;
        }

        // Método de validação da senha
        private static bool IsValidPassword(string password)
        {
            // Verifica se a senha contém pelo menos:
            // - uma letra minúscula
            // - uma letra maiúscula
            // - um número
            // - um caractere especial
            const string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).+$";

            // Cria a expressão regular com o padrão definido
            var regex = new Regex(pattern);

            // Retorna verdadeiro se a senha corresponder ao padrão, falso caso contrário
            return regex.IsMatch(password);
        }

        public string GetHashPassword()
        {
            var salt = GenerateSalt();

            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(Password))
            {
                Salt = salt,
                DegreeOfParallelism = 8, // número de threads a serem usados
                MemorySize = 65536, // 64 MB
                Iterations = 4 // número de iterações
            };

            return Convert.ToBase64String(argon2.GetBytes(16)) + ":" + Convert.ToBase64String(salt);
        }

        private static byte[] GenerateSalt()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var salt = new byte[32];
                rng.GetBytes(salt);
                return salt;
            }
        }

        public bool IsPasswordHashValid(string password)
        {
            // Divide o hash armazenado em hash e salt
            var parts = Password.Split(':');
            if (parts.Length != 2)
            {
                throw new FormatException("Formato de hash armazenado inválido.");
            }

            var storedPasswordHash = Convert.FromBase64String(parts[0]);
            var salt = Convert.FromBase64String(parts[1]);

            // Configura o Argon2id com a senha fornecida e o salt armazenado
            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
            {
                Salt = salt,
                DegreeOfParallelism = 8,
                MemorySize = 65536,
                Iterations = 4
            };

            // Gera o hash da senha fornecida
            var computedHash = argon2.GetBytes(16);

            // Compara o hash calculado com o hash armazenado
            return CryptographicOperations.FixedTimeEquals(computedHash, storedPasswordHash);
        }
    }
}
