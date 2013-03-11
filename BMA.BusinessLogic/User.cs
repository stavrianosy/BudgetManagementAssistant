using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;

namespace BMA.BusinessLogic
{
    public class User : BaseItem, INotifyPropertyChanged
    {
        #region Private Members
        string userName;
        string password;
        string email;
        #endregion

        #region Public Properties
        public int UserId { get; set; }

        [Required]
        public string UserName { get { return userName; } set { userName = value; NotifyPropertyChanged("UserName"); } }

        [Required]
        public string Password { get { return password; } set { password = value; NotifyPropertyChanged("Password"); } }

        [Required]
        public string Email { get { return email; } set { email = value; NotifyPropertyChanged("Email"); } }
        #endregion

        #region Constructors
        public User()
        {
            UserId = -1;
        }
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                HasChanges = true;
                ModifiedDate = DateTime.Now;
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
        #endregion

        #region Public Methods
        public List<string> SelfValidation(bool includeEmail)
        {
            List<string> result = new List<string>();

            if (UserName.Trim().Length <= 3)
                if (UserName.Trim().Length == 0)
                    result.Add("Username is required");
                else
                    result.Add("Username must be greater than 3 characters");

            result.AddRange(ValidatePassword(Password));

            if (includeEmail)
            {
                if (Email.Trim().Length == 0)
                    result.Add("Email is required");
                else if(!Regex.IsMatch(Email, @"^([\w\d\-\.]+)@{1}(([\w\d\-]{1,67})|([\w\d\-]+\.[\w\d\-]{1,67}))\.(([a-zA-Z\d]{2,4})(\.[a-zA-Z\d]{2})?)$"))
                    result.Add("Email is not in the appropriate format");
            }   

            if (UserName.Trim().Equals(Password.Trim(), StringComparison.CurrentCulture))
                result.Add("Username and password must be different");


            return result;
        }

        public List<string> ValidatePassword(string password)
        {
            List<string> result = new List<string>();

            if (password.Trim().Length <= 3)
                if (password.Trim().Length == 0)
                    result.Add("Password is required");
                else
                    result.Add("Password must be greater than 3 characters");

            return result;
        }

        #endregion
    }
}
