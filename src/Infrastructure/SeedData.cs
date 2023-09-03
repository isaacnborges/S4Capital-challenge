using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using S4Capital.Api.Domain.ValueObjects;

namespace S4Capital.Api.Infrastructure;

public static class SeedData
{
    private const string PublicRoleId = "b19a8b05-f7e3-4523-92fa-f93d46beef49";
    private const string WriterRoleId = "0d4f0338-b3fd-4c34-9bf0-b45f35c54b12";
    private const string EditorRoleId = "68dd6ac7-68f7-4d48-8846-9b5f0646fa39";

    private const string PublicUser1 = "acf620c4-e358-4a22-b676-2ea72d07c2f8";
    private const string PublicUser2 = "d3ad3c26-a840-4b2f-b463-e23eb816bfc4";

    private const string WriterUser1 = "efc59bb8-b027-4cf3-9f98-ac9c5d146ace";
    private const string WriterUser2 = "3f862f7a-a060-4a4e-a0bd-32589acf8317";

    private const string EditorUser1 = "27372113-9bda-4638-9207-f1ff812dd30b";
    private const string EditorUser2 = "b85bbf2f-d95e-4d5b-b819-807f825bc7b7";

    public static void Seed(this ModelBuilder builder)
    {
        SeedRoles(builder);
        SeedUsers(builder);
        SetUserRoles(builder);
    }

    private static void SeedRoles(ModelBuilder builder)
    {
        builder.Entity<IdentityRole>().HasData(new IdentityRole
        {
            Name = UserRole.Public,
            NormalizedName = UserRole.Public.ToUpper(),
            Id = PublicRoleId,
            ConcurrencyStamp = PublicRoleId
        });

        builder.Entity<IdentityRole>().HasData(new IdentityRole
        {
            Name = UserRole.Writer,
            NormalizedName = UserRole.Writer.ToUpper(),
            Id = WriterRoleId,
            ConcurrencyStamp = WriterRoleId
        });

        builder.Entity<IdentityRole>().HasData(new IdentityRole
        {
            Name = UserRole.Editor,
            NormalizedName = UserRole.Editor.ToUpper(),
            Id = EditorRoleId,
            ConcurrencyStamp = EditorRoleId
        });
    }

    private static void SeedUsers(ModelBuilder builder)
    {
        var public1 = new IdentityUser
        {
            Id = PublicUser1,
            Email = "public1@example.com",
            NormalizedEmail = "PUBLIC1@EXAMPLE.COM",
            EmailConfirmed = false,
            UserName = "public1",
            NormalizedUserName = "PUBLIC1"
        };

        var public2 = new IdentityUser
        {
            Id = PublicUser2,
            Email = "public2@example.com",
            NormalizedEmail = "PUBLIC2@EXAMPLE.COM",
            EmailConfirmed = false,
            UserName = "public2",
            NormalizedUserName = "PUBLIC2"
        };

        var writer1 = new IdentityUser
        {
            Id = WriterUser1,
            Email = "writer1@example.com",
            NormalizedEmail = "WRITER1@EXAMPLE.COM",
            EmailConfirmed = false,
            UserName = "writer1",
            NormalizedUserName = "WRITER1"
        };

        var writer2 = new IdentityUser
        {
            Id = WriterUser2,
            Email = "writer2@example.com",
            NormalizedEmail = "WRITER2@EXAMPLE.COM",
            EmailConfirmed = false,
            UserName = "writer2",
            NormalizedUserName = "WRITER2"
        };

        var editor1 = new IdentityUser
        {
            Id = EditorUser1,
            Email = "editor1@example.com",
            NormalizedEmail = "EDITOR1@EXAMPLE.COM",
            EmailConfirmed = false,
            UserName = "editor1",
            NormalizedUserName = "EDITOR1"
        };

        var editor2 = new IdentityUser
        {
            Id = EditorUser2,
            Email = "editor2@example.com",
            NormalizedEmail = "EDITOR2@EXAMPLE.COM",
            EmailConfirmed = false,
            UserName = "editor2",
            NormalizedUserName = "EDITOR2"
        };


        var password = "@Password123";
        var ph = new PasswordHasher<IdentityUser>();
        public1.PasswordHash = ph.HashPassword(public1, password);
        public2.PasswordHash = ph.HashPassword(public2, password);
        writer1.PasswordHash = ph.HashPassword(writer1, password);
        writer2.PasswordHash = ph.HashPassword(writer2, password);
        editor1.PasswordHash = ph.HashPassword(editor1, password);
        editor2.PasswordHash = ph.HashPassword(editor2, password);

        builder.Entity<IdentityUser>().HasData(public1);
        builder.Entity<IdentityUser>().HasData(public2);
        builder.Entity<IdentityUser>().HasData(writer1);
        builder.Entity<IdentityUser>().HasData(writer2);
        builder.Entity<IdentityUser>().HasData(editor1);
        builder.Entity<IdentityUser>().HasData(editor2);
    }

    private static void SetUserRoles(ModelBuilder builder)
    {
        builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
        {
            RoleId = PublicRoleId,
            UserId = PublicUser1
        });

        builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
        {
            RoleId = PublicRoleId,
            UserId = PublicUser2
        });

        builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
        {
            RoleId = WriterRoleId,
            UserId = WriterUser1
        });

        builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
        {
            RoleId = WriterRoleId,
            UserId = WriterUser2
        });

        builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
        {
            RoleId = EditorRoleId,
            UserId = EditorUser1
        });

        builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
        {
            RoleId = EditorRoleId,
            UserId = EditorUser2
        });
    }
}