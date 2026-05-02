using System;
using WPF;

namespace WPF
{

    public enum UserRole
    { 
        User,
        Administrator
    }

    public class User
	{
        public int User_id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        private string Password_hash { get; set; }
        public string Profile_picture_url { get; set; }
        public DateTime Created_at { get; set; }

        public User(string username, string email, string password_hash, string profile_picture_url, DateTime created_at )
        {
            Username = username;
            Email = email;
            Password_hash = password_hash;
            Profile_picture_url = profile_picture_url;
            Created_at = created_at;

        }

        public void Register()
        {
            // upload a photo, enter full name, email, password, agree to terms and conditions, click on sign up 
            // use continue with google
            // use continue with github
            // after successfully registering, user is taken to the login page
        }

        public void Login()
        {
            // starts with this page and if user has no account, he/ she is directed to the register screen
            // enter email, enter password, click on remember me to show password
            // click on forgot password to receive an email with a 6 digit code to bypass login
            // or forgot password to display other options like signing in with phonenumber and a 6 digit code to bypass login
            // or chatgpt account
        }

        public void Logout()
        {
            //clear cookies or session tokens

        }

        public void ChangePassword()
        {

           
            // in the admin panel user will be able to change password easily.
        }

        public void  UploadProfilePicture()
        {
            // user should be able to upload a picture from gallery or from a website
            // or to select from a bunch of in-built app profile pictures
            // should be able to create a normal profile picture with random colour backgrounds and the initials of the user

        }

        public void RemoveProfilePicture()
        {
            // should be able to remove profile picture as well
        }

        public void DeleteAccount()
        {
            // user should be able to delete his or her account
            // this is only possible in the admin panel 
            // by doing this, his/her details/name gets wiped out from the database
            // he or she would first get a warning message before proceeding or stopping

        }

        public void  RequestPasswordReset()
        {
            // only poasible when the user forgets his or her password
        }


        public class Administrator: User
        {
            public UserRole Role { get; set;}
            public Administrator(string username, string email, string password_hash, string profile_picture_url, DateTime created_at, UserRole role): base(username, email, password_hash, profile_picture_url,created_at)
            {
                Role = role;
            }

            public void AdminLogin()
            {
                // there should be an option before logging to  either login as an admin or a regular player

            }


            public override string ToString() => "Hello World";// should give an overview of all users

            public void ManageUsers()
            {
                //Create, edit and delete user profiles
            }

            public void ExportData()
            {
                // should be able to export data into a csv/ excel of pdf format
            }


        }

    }
}
