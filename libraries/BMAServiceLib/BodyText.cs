using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMAServiceLib
{
    public static class BodyText
    {
        public static string NewAccount()
        {
            var result = new StringBuilder();

            result.AppendLine("Dear new user,");
            result.AppendLine("\n");
            result.AppendLine("Thank you for registering with Money Saver.");
            result.AppendLine();
            result.AppendLine("Please find your login details below.");
            result.AppendLine();
            result.AppendLine("Username: {0}");
            result.AppendLine("Password: {1}");
            result.AppendLine();
            result.AppendLine("As a registered candidate you can use the app in order to assist your everyday cash transactions.");
            result.AppendLine();
            result.AppendLine("For any questions or suggestion, feel free and send us an email at ");
            result.AppendLine("info@softcarbongear.com");
            result.AppendLine();
            result.AppendLine();
            result.AppendLine("You have received this email because you registed in Money Saver app.");
            result.AppendLine("To update your details, please login to your account. ");

            return result.ToString();
        }

        public static string ResetPassword()
        {
            var result = new StringBuilder();

            result.AppendLine("Automated mail from Money Saver app.");
            result.AppendLine();
            result.AppendLine("Password reset successfully.");
            result.AppendLine("Your new password is {0}");
            result.AppendLine();
            result.AppendLine();
            result.AppendLine("You have received this email because you requested to change your password.");
            result.AppendLine("To update your details, please login to your account. ");
            
            return result.ToString();
        }

        public static string SendPassword()
        {
            var result = new StringBuilder();

            result.AppendLine("Automated mail from Money Saver app.");
            result.AppendLine();
            result.AppendLine("Your password: {0}");
            result.AppendLine();
            result.AppendLine();
            result.AppendLine("You have received this email because you requested to send your password.");
            result.AppendLine("To update your details, please login to your account. ");
            
            return result.ToString();
        }
    }
}
