using System.Security.Cryptography;
using System.Text;

namespace SBTaskManagement.Application.Helpers
{
    public static class HashingUtility
    {
            public static string HashString(string data)
            {
                using (SHA512 sha512 = SHA512.Create())
                {
                    byte[] hashValue = sha512.ComputeHash(Encoding.UTF8.GetBytes(data));
                    return Convert.ToHexString(hashValue);
                }

            }
            public static PasswordHashDetails HashPassword(AppConfig config, string data, string salt = null)
            {
                if (string.IsNullOrEmpty(salt))
                {
                    var generator = RandomNumberGenerator.Create();
                    byte[] saltByte = new byte[16];
                    generator.GetNonZeroBytes(saltByte);
                    salt = System.Text.Encoding.Default.GetString(saltByte);
                }
                var hashData = data;
                for (int i = 0; i <config.PasswordHashIteration; i++)
                {
                  
                    hashData = HashString($"{salt}{hashData}{config.PepperKey}");
                }

                return new PasswordHashDetails
                {
                    Salt = salt,
                    HashedValue = hashData
                };
            }

		

		//public static string HashString(string data)
		//{
		//    using (SHA512 sha512 = SHA512.Create())
		//    {
		//        byte[] hashValue = sha512.ComputeHash(Encoding.UTF8.GetBytes(data));
		//        return Convert.ToHexString(hashValue);
		//    }

		//}
		//public static PasswordHashDetails HashPassword(string data, AppConfig config, string salt = null)
		//{
		//    if (string.IsNullOrEmpty(salt))
		//    {
		//        var generator = RandomNumberGenerator.Create();
		//        byte[] saltByte = new byte[16];
		//        generator.GetNonZeroBytes(saltByte);
		//        salt = System.Text.Encoding.Default.GetString(saltByte);
		//    }
		//    var hashData = data;
		//    for (int i = 0; i < config.PasswordHashIteration; i++)
		//    {
		//        hashData = HashString($"{salt}{hashData}{config.PepperKey}");
		//    }

		//    return new PasswordHashDetails
		//    {
		//        Salt = salt,
		//        HashedValue = hashData
		//    };
		//}
	}
}
