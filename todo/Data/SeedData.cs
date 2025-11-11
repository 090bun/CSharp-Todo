using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
namespace todo.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new TodoListContext(
                serviceProvider.GetRequiredService<DbContextOptions<TodoListContext>>()))
            {
                //檢查是否已有資料
                if (!context.User.Any())
                {
                    var pass1 = Environment.GetEnvironmentVariable("User1") ?? "123456";
                    var pass2 = Environment.GetEnvironmentVariable("User2") ?? "123456";
                    var hashedPass1 = BCrypt.Net.BCrypt.HashPassword(pass1);
                    var hashedPass2 =BCrypt.Net.BCrypt.HashPassword( pass2);
                    context.User.AddRange(
                        new User
                        {
                            Name = "Alice",
                            Password = hashedPass1,
                            UserInfo = new UserInfo
                            {
                                Address = "123 Main St",
                                Birthday = "1990/01/01",
                                Phone = "0912345678"
                            }
                        },
                        new User
                        {
                            Name = "Bob",
                            Password = hashedPass2,
                            UserInfo = new UserInfo
                            {
                                Address = "456 Elm St",
                                Birthday = "1985/05/15",
                                Phone = "0987654321"
                            }
                        }
                    );
                    context.SaveChanges();
                }

                if (!context.Todo.Any())
                {
                    var user = context.User
                        .OrderBy(u => u.Id);

                    var firstUser = user.First();
                    var lastUser = user.Last();
                    context.Todo.AddRange(
                        new Todo
                        {
                            Title = "Buy groceries",
                            Descript = "Milk, Bread, Eggs",
                            UserId = firstUser.Id
                        },
                        new Todo
                        {
                            Title = "Walk the dog",
                            Descript = "Evening walk in the park",
                            UserId = lastUser.Id
                        }
                    );

                    context.SaveChanges();
                }
            }
        }
    }
}