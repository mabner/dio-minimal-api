﻿using MinimalApi.Domain.DTOs;
using MinimalApi.Domain.Entities;
using MinimalApi.Domain.Interfaces;

namespace Test.Mocks;

internal class AdministratorServiceMock : IAdministratorService
{
    private static List<Administrator> admins = new List<Administrator>()
    {
        new Administrator
        {
            Id = 1,
            Email = "admin@test.com",
            Password = "123456",
            Profile = "Admin"
        },
        new Administrator
        {
            Id = 2,
            Email = "super@user.com",
            Password = "1234",
            Profile = "Superuser",
        }
    };

    public Administrator Add(Administrator administrator)
    {
        administrator.Id = admins.Count() + 1;
        admins.Add(administrator);

        return administrator;
    }

    public Administrator? GetAdministratorById(int id)
    {
        return admins.Find(a => a.Id == id);
    }

    public List<Administrator> GetAdministrators(int? page)
    {
        return admins;
    }

    public Administrator? Login(LoginDTO loginDTO)
    {
        return admins.Find(a => a.Email == loginDTO.Email && a.Password == loginDTO.Password);
    }
}
