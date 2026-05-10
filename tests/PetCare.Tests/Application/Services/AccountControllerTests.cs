using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using PetCare.Infrastructure.Configuration;
using PetCare.Web.Controllers;

namespace PetCare.Tests.Application.Services;

public class AccountControllerTests
{
    [Fact]
    public void Login_DeveRetornarViewComConfiguracoesSupabase()
    {
        // Arrange
        var settings = new SupabaseSettings
        {
            Url = "https://xyz.supabase.co",
            AnonKey = "key123"
        };
        var optionsMock = new Mock<IOptions<SupabaseSettings>>();
        optionsMock.Setup(o => o.Value).Returns(settings);

        var controller = new AccountController(optionsMock.Object);

        // Act
        var result = controller.Login() as ViewResult;

        // Assert
        result.Should().NotBeNull();
        result!.ViewData["SupabaseUrl"].Should().Be("https://xyz.supabase.co");
        result!.ViewData["SupabaseKey"].Should().Be("key123");
    }
}
