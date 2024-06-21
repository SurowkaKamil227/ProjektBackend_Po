using SoapService;

IUserService soapServiceChannel = new UserServiceClient(UserServiceClient.EndpointConfiguration.BasicHttpBinding_IUserService);
var registerUserResponse = await soapServiceChannel.RegisterUserAsync(new User()
{
    FirstName = "Jan",
    LastName = "Kowalski",
    EmailAddress = "jankowalski@wsei.edu.pl",
    Age = 25,
    MarketingConsent = true
});
Console.WriteLine(registerUserResponse);
Console.ReadKey();