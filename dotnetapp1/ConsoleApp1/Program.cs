using PasswordGenerator;

var pwd = new Password();
var password = pwd.Next();

Console.WriteLine($"Hello, here is your password: {password.ToString()}");
// Console.WriteLine(password);
