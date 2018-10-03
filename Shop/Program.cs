using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Shop
{
    class Program
    {
        public static int usersId = 0;
        public static int cartProductsId = 0;
        public static int productsMaxCount = 5;
        public static int usersMaxCount = 5;
        public static List<Product> products = new List<Product>(productsMaxCount);
        public static List<User> users = new List<User>(usersMaxCount);
        public static ShoppingCart cart = new ShoppingCart();
        public static int cartUserId = 0;
        static void Main(string[] args)
        {
            products.Add(new Product() { Id = 1, ProductName = "Red Pen", ProductCount = 10, ProductPrice = 90 });
            products.Add(new Product() { Id = 2, ProductName = "Green Pen", ProductCount = 12, ProductPrice = 105 });
            products.Add(new Product() { Id = 3, ProductName = "Blue Pen", ProductCount = 15, ProductPrice = 110 });
            products.Add(new Product() { Id = 4, ProductName = "Black Pen", ProductCount = 10, ProductPrice = 95 });
            products.Add(new Product() { Id = 5, ProductName = "Yellow Pen", ProductCount = 14, ProductPrice = 100 });

            Menu();
        }
        public static void PhoneVerification(long number, int code)
        {
            string stringCode = code.ToString();
            string stringNumber = "+7" + number.ToString();
            const string accountSid = "AC077c00839970a717c5e527238f9b8cd8";
            const string authToken = "1a3c7809c9c368969275841f4f46ebe1";

            TwilioClient.Init(accountSid, authToken);

            var message = MessageResource.Create(
                body: stringCode,
                from: new Twilio.Types.PhoneNumber("+18125779858"),
                to: new Twilio.Types.PhoneNumber(stringNumber)
            );

            Console.WriteLine(message.Sid);
        }
        public static void Menu()
        {
            Console.WriteLine("1.Регистрация\n");
            Console.WriteLine("2.Вход\n");
            Console.WriteLine("3.Выбор товара\n");
            Console.WriteLine("4.Корзина\n");

            string inputString = Console.ReadLine();
            int menuNumber = int.Parse(inputString);

            switch (menuNumber)
            {
                case 1:
                    users.Add(new User());
                    users[usersId].Id = usersId + 1;
                    Console.Clear();
                    Console.WriteLine("Login: ");
                    inputString = Console.ReadLine();
                    users[usersId].Login = inputString;

                    Console.WriteLine("Password: ");
                    inputString = Console.ReadLine();
                    users[usersId].Password = inputString;

                    Console.WriteLine("Full Name: ");
                    inputString = Console.ReadLine();
                    users[usersId].FullName = inputString;

                    Console.WriteLine("Phone Number(7XX XXX XXXX): ");  //7026283433 мой номер
                    inputString = Console.ReadLine();
                    long inputPhone = long.Parse(inputString);
                    users[usersId].PhoneNumber = inputPhone;

                    Random random = new Random();
                    int temp = random.Next(1000);
                    PhoneVerification(users[usersId].PhoneNumber, temp);

                    Console.WriteLine("Verification Number: ");
                    inputString = Console.ReadLine();
                    int inputCode = int.Parse(inputString);

                    if (inputCode == temp)
                    {
                        usersId++;
                        Console.Clear();
                        Console.WriteLine("Регистрация пройдена успешно! \n");
                        Menu();
                    }
                    else
                    {
                        Console.WriteLine("Неверный код подтверждения: ");
                        Menu();
                    }
                    break;

                case 2:
                    Console.Clear();
                    Console.WriteLine("Login: ");
                    string inputLogin = Console.ReadLine();

                    Console.WriteLine("Password: ");
                    string inputPassword = Console.ReadLine();
                    Console.Clear();

                    for (int i = 0; i < usersMaxCount; i++)
                    {
                        if (usersId > 0)
                        {
                            if (users[i].Login == inputLogin && users[i].Password == inputPassword)
                            {
                                Console.WriteLine("Здравствуйте, {0}\n", users[i].FullName);
                                cartUserId = i + 1;
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Неверный логин или пароль \n");
                                break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Нет зарегистрированных пользователей \n");
                            break;
                        }
                    }
                    Menu();
                    break;

                case 3:
                    Console.Clear();
                    foreach (Product p in products)
                    {
                        Console.WriteLine(p.Id + ". " + p.ProductName + ": " + p.ProductCount + "шт. " + p.ProductPrice + "тг.\n");
                    }
                    if (cartUserId != 0)
                    {
                        Console.WriteLine("Для покупки введите номер товара и количество: \n");
                        Console.WriteLine("Номер: ");
                        inputString = Console.ReadLine();
                        menuNumber = int.Parse(inputString);
                        cart.CartProduct = menuNumber;

                        Console.WriteLine("Количество: ");
                        inputString = Console.ReadLine();
                        menuNumber = int.Parse(inputString);
                        cart.ProductCount = menuNumber;

                        cart.CartUserId = cartUserId;
                        cart.Id = cartProductsId;
                        cart.PaymentSum += cart.ProductCount * products[cart.CartProduct - 1].ProductPrice;
                        cartProductsId++;
                        Menu();
                    }
                    else
                    {
                        Menu();
                    }
                    break;

                case 4:
                    Console.Clear();
                    Console.WriteLine(users[cart.CartUserId - 1].FullName + ": "  + products[cart.CartProduct - 1].ProductName + " " + cart.ProductCount + "шт. " + cart.PaymentSum + "тг.\n");
                    Console.WriteLine("1. Оплатить: ");
                    Console.WriteLine("2. Очистить корзину: ");
                    inputString = Console.ReadLine();
                    menuNumber = int.Parse(inputString);
                    switch (menuNumber)
                    {
                        case 1:
                            Console.WriteLine("Номер карты оплаты: ");
                            inputString = Console.ReadLine();
                            int cardNumber = int.Parse(inputString);
                            products[cart.CartProduct - 1].ProductCount -= cart.CartProduct;
                            break;
                        case 2:
                            cart.RemoveFromCart();
                            Menu();
                            break;
                    }
                    break;
            }
            Console.ReadLine();
        }
    }
}
