# Code First and Db First

## 安装所需Nuget Package

- Entity Framework Core (EF Core)
- Install-Package Microsoft.Entity.FrameworkCore.Sqlite
- Install-Package Microsoft.Entity.FrameworkCore.SqlSever

## CF

- 添加模型：

```C#
public class Blog
{
	public int ID {get;set;}
	public string Name {get;set;}
	public string Url {get;set;}
}
```

- 添加上下文

```C#
public class BlogContext:DbContext
{
	public DbSet<Blog> Blogs {get;set;}
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseSqlite(@"Filename=./blogging.db");
	}
}
```

- 创建数据库

  - Starup.cs / ConfigureServices

```C#
public void ConfigureServices(IServiceCollection services)
{
	var connection ="Filename=./Blogging.db";
	services.AddDbContext<DataContext>(options=>options.UseSqlite(connection));
	services.AddMvc();
}
```

- 安装Microsoft.Entity.FrameworkCore.Tools
  - Install-Package Microsoft.Entity.FrameworkCore.Tools -Pre
- 创建数据库
  - dotnet ef migrations add [Dbname]
  - dotnet ef database update
- 常见问题：
  - 1.存在多个Context时需要指定context
    - dotnet ef migrations add [Dbname] -c [Context Name]
        EG:`dotnet ef migrations add [Dbname] -c BloggingDbContext`
    - dotnet ef database update -c [Context Name]
        EG:`dotnet ef database update [Dbname] -c BloggingDbContext`
	- 添加上下文件工厂类：https://docs.microsoft.com/zh-cn/ef/core/miscellaneous/cli/dbcontext-creation
```C#
public class BloggingDbContextFactory : IDesignTimeDbContextFactory<BloggingDbContext>
{
    public BloggingDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<BloggingDbContext>();
        builder.UseSqlite("Data Source=blog.db");
        return new BloggingDbContext(builder.Options);
    }
}
```

## DF

- 引用EF Core
  - Install-Package Microsoft.Entity.FrameworkCore.Sqlite
  - Install-Package Microsoft.Entity.FrameworkCore.Tools -Pre
  - Install-Package Microsoft.Entity.FrameworkCore.Sqlite.Design
- 生成实体
  - 复制db文件到根目录下
  - 执行：dotnet ef dbcontext scaffold "FileName=[DB File Name].db" Microsoft.Entity.FrameworkCore.Sqlite
  - 执行：dotnet ef dbcontext scaffold "FileName=[DB File Name].db" Microsoft.Entity.FrameworkCore.Sqlite -OutputDir Models

