﻿/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

namespace AspNet.Security.OAuth.WordPress;

public class WordPressTests : OAuthTests<WordPressAuthenticationOptions>
{
    public WordPressTests(ITestOutputHelper outputHelper)
    {
        OutputHelper = outputHelper;
    }

    public override string DefaultScheme => WordPressAuthenticationDefaults.AuthenticationScheme;

    protected internal override void RegisterAuthentication(AuthenticationBuilder builder)
    {
        builder.AddWordPress(options => ConfigureDefaults(builder, options));
    }

    [Theory]
    [InlineData(ClaimTypes.NameIdentifier, "my-id")]
    [InlineData(ClaimTypes.Name, "john-smith")]
    [InlineData("urn:wordpress:avatarurl", "https://www.wordpress.local/john-smith/avatar.png")]
    [InlineData("urn:wordpress:displayname", "John Smith")]
    [InlineData("urn:wordpress:email", "john@john-smith.local")]
    [InlineData("urn:wordpress:primaryblog", "https://john-smith.wordpress.local")]
    [InlineData("urn:wordpress:profileurl", "https://www.wordpress.local/john-smith")]
    public async Task Can_Sign_In_Using_WordPress(string claimType, string claimValue)
    {
        // Arrange
        using var server = CreateTestServer();

        // Act
        var claims = await AuthenticateUserAsync(server);

        // Assert
        AssertClaim(claims, claimType, claimValue);
    }
}
