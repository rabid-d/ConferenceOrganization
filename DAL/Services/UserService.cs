﻿using DAL.Model;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace DAL.Services
{
    public class UserService
    {
        private ConferenceDatabaseContext confDbContext = new();

        public async Task AddUser(Guid id, string fullName, string degree, string work, string position, string biography)
        {
            User newUser = new()
            {
                UserId = id,
                FullName = fullName,
                Degree = degree,
                Work = work,
                Position = position,
                ProfessionalBiography = biography,
            };
            await confDbContext.Users.AddAsync(newUser);
            await confDbContext.SaveChangesAsync();
        }

        public async Task<bool> IsUserExists(string id)
        {
            return confDbContext.Users.Any(u => u.UserId.ToString().ToLower() == id.ToLower());
        }

        public bool IsUsernameTaken(string username)
        {
            return confDbContext.AppUsers.Any(u => u.Email.ToLower() == username.ToLower());
        }

        public async Task UpdateUserPhoto(string id, string pathToPhoto)
        {
            User? user = confDbContext.Users.FirstOrDefault(u => u.UserId.ToString().ToLower() == id.ToLower());
            if (user != null)
            {
                user.PathToPhoto = pathToPhoto;
                await confDbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> AddAppUser(AppUser dalAppUser)
        {
            dalAppUser.Password = GetPasswordHash(dalAppUser.Password);
            await confDbContext.AppUsers.AddAsync(dalAppUser);
            int rowsAdded = await confDbContext.SaveChangesAsync();
            return rowsAdded > 0;
        }

        public async Task<bool> IsLoginCredentialsValid(AppUser dalAppUser)
        {
            if (string.IsNullOrEmpty(dalAppUser.Password) || string.IsNullOrEmpty(dalAppUser.Email))
            {
                return false;
            }
            string pswdHash = GetPasswordHash(dalAppUser.Password);
            AppUser? user = await confDbContext.AppUsers.FirstOrDefaultAsync(
                u => u.Email == dalAppUser.Email && u.Password == pswdHash
            );
            return user != null;
        }

        public async Task<Guid?> GetAppUserGuid(string email)
        {
            if (email == null)
            {
                return null;
            }

            AppUser? user = await confDbContext.AppUsers.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return null;
            }
            return user.AppUserId;
        }

        private string GetPasswordHash(string password)
        {
            using SHA512 alg = SHA512.Create();
            alg.ComputeHash(Encoding.ASCII.GetBytes(password));
            return BitConverter.ToString(alg.Hash ?? throw new ArgumentException());
        }
    }
}
