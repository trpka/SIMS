/***********************************************************************
 * Module:  User.cs
 * Author:  Wombat
 * Purpose: Definition of the Class User
 ***********************************************************************/

using System;

namespace Model
{
   public class User
   {
      private int id;
      private String firstName;
      private String lastName;
      private String password;
      private String username;

        public User(int id, string firstName, string lastName, string password, string username)
        {
            this.id = id;
            this.firstName = firstName;
            this.lastName = lastName;
            this.password = password;
            this.username = username;
        }

        public String GetFirstName(int id)
      {
         throw new NotImplementedException();
      }
      
      public String GetLastName(int id)
      {
         throw new NotImplementedException();
      }
   
   }
}